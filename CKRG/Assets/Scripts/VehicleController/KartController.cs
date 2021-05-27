using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public GameObject[] wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 6000f;

    public AudioSource skidSound;
    public AudioSource highAccSound;

    public Transform SkidTrailPrefab;
    Transform[] skidTrails = new Transform[4];

    public ParticleSystem smokePrefab;
    ParticleSystem[] skidSmoke = new ParticleSystem[4];

    public Rigidbody rb;
    public float gearLength = 3;
    public float currentSpeed { get { return rb.velocity.magnitude * gearLength; } }
    public float lowPitch = 1f;
    public float highPitch = 6f;
    public float numGears = 5;
    float rpm;
    int currentGear = 1;
    float currentGearPerc;
    public float maxSpeed = 200;  

    public GameObject playerNamePrefab;
    public Renderer kartMesh;
    public string networkName = "";
    string[] aiNames = { "Buðra", "Okan", "Cem", "Esra", "Murat", "Ahmet", "Mehmet", "Veli" };

    void Start()
    {

        for (int i = 0; i < wheels.Length; i++)
        {
            skidSmoke[i] = Instantiate(smokePrefab);
            skidSmoke[i].Stop();
        }

        GameObject playerName = Instantiate(playerNamePrefab);
        playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;

        if (GetComponent<AIController>() != null)
            if (networkName != "")
                playerName.GetComponent<Text>().text = networkName;
            else
                playerName.GetComponent<Text>().text = aiNames[Random.Range(0, aiNames.Length)];
        else
            playerName.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");

        playerName.GetComponent<NameUIController>().carRenderer = kartMesh;
    }



    public void CalculateEngineSound()
    {
        float gearPercentage = (1 / (float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed));
        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5f);
        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPerc = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float)numGears) * (currentGear + 1);
        float downGearMax = (1 / (float)numGears) * (currentGear);

        if (currentGear > 0 && speedPerc < downGearMax)
            currentGear--;
        if (speedPerc > upperGearMax && (currentGear < (numGears - 1)))
            currentGear++;

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);
        highAccSound.pitch = Mathf.Min(highPitch, pitch) * 0.25f;
    }

    public void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;


        float thrustTorque = 0;

        if (currentSpeed < maxSpeed)
            thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        {

            wheelColliders[i].motorTorque = thrustTorque;
            wheelColliders[i].brakeTorque = brake;

            if (i < 2)
                wheelColliders[i].steerAngle = steer;



            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheels[i].transform.position = position;
            wheels[i].transform.rotation = quat;
        }      

    }

    public void CheckForSkid()
    {
        int numSkid = 0;
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelHit wheelHit;
            wheelColliders[i].GetGroundHit(out wheelHit);
            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.4f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f)
            {
                numSkid++;
                if (!skidSound.isPlaying)
                {
                    skidSound.Play();

                }
                // StartSkidTrail(i);
                skidSmoke[i].transform.position = wheelColliders[i].transform.position - wheelColliders[i].transform.up * wheelColliders[i].radius;
                skidSmoke[i].Emit(1);
            }
            else
            {
                //   EndSkidTrail(i);
            }
        }
        if (numSkid == 0 && skidSound.isPlaying)
        {
            skidSound.Stop();
        }
    }

    public void StartSkidTrail(int i)
    {
        if (skidTrails[i] == null)
            skidTrails[i] = Instantiate(SkidTrailPrefab);
        skidTrails[i].parent = wheelColliders[i].transform;
        skidTrails[i].localRotation = Quaternion.Euler(90, 0, 0);
        skidTrails[i].localPosition = -Vector3.up * wheelColliders[i].radius;
    }
    public void EndSkidTrail(int i)
    {
        if (skidTrails[i] == null)
            return;
        Transform holder = skidTrails[i];
        skidTrails[i] = null;
        holder.parent = null;
        holder.rotation = Quaternion.Euler(90, 0, 0);
        Destroy(holder.transform.gameObject, 30);
    }
}
