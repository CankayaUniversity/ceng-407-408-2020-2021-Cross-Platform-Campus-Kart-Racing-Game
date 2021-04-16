using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public GameObject[] wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 2000f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float brake = Input.GetAxis("Jump");

        Go(vertical, horizontal,brake);


    }

    void Go(float accel, float steer,float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        float thrustTorque = accel * torque;
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = thrustTorque;

            if (i < 2)
                wheelColliders[i].steerAngle = steer;
            else
                wheelColliders[i].brakeTorque = brake;

            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheels[i].transform.position = position;
            wheels[i].transform.rotation = quat;
        }

    }
}
