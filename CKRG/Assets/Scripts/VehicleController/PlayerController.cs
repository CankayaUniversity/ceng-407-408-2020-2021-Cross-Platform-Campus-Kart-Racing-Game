using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode { Keyboard = 1, Touch = 2 };
public class PlayerController : MonoBehaviour
{
    KartController kc;
    public ControlMode controlMode;
    public GameObject mobileUI;
    float lastTimeMoving = 0;
    float horizontal;
    float vertical;
    float brake;
    Vector3 lastPos;
    Quaternion lastRot;
    CheckpointManager checkpointManager;

    float finishSteer;


    // Start is called before the first frame update
    void Start()
    {
        kc = GetComponent<KartController>();
        this.GetComponent<Ghost>().enabled = false;
        finishSteer = Random.Range(-1.0f, 1.0f);
    }

    void Update()
    {
        if (checkpointManager == null)
            checkpointManager = kc.rb.GetComponent<CheckpointManager>();

        if (checkpointManager.lap == RaceStarter.totalLaps + 1)
        {
            kc.highAccSound.Stop();
            kc.Go(0, finishSteer, 0);
            return;
        }
        if (controlMode == ControlMode.Keyboard)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            brake = Input.GetAxis("Jump");
            mobileUI.SetActive(false);
        }
        else
            mobileUI.SetActive(true);



        if (kc.rb.velocity.magnitude > 1 || !RaceStarter.raceStart)
            lastTimeMoving = Time.time;

        RaycastHit hit;
        if (Physics.Raycast(kc.rb.gameObject.transform.position, -Vector3.up, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "Road")
            {
                lastPos = kc.rb.gameObject.transform.position;
                lastRot = kc.rb.gameObject.transform.rotation;
            }

        }

        if (Time.time > lastTimeMoving + 4)
        {
            kc.rb.gameObject.transform.position = checkpointManager.lastCP.transform.position + Vector3.up * 2;
            kc.rb.gameObject.transform.rotation = checkpointManager.lastCP.transform.rotation;
            kc.rb.gameObject.layer = 8;
            this.GetComponent<Ghost>().enabled = true;
            Invoke("ResetLayer", 3);
        }



        if (!RaceStarter.raceStart) vertical = 0;

        kc.Go(vertical, horizontal, brake);
        kc.CheckForSkid();
        kc.CalculateEngineSound();
    }

    void ResetLayer()
    {
        kc.rb.gameObject.layer = 0;
        this.GetComponent<Ghost>().enabled = false;
    }

    #region Mobie
    public void AccelInput(float input) { vertical = input; }
    public void SteerInput(float input) { horizontal = input; }
    public void BrakeInput(float input) { brake = input; }
    #endregion
}
