using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public GameObject[] wheels;
    public float torque = 200f;
    public float maxSteerAngle = 30f;
    public float maxBrakeTorque = 500f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float brake = Input.GetAxis("Jump");
        Drive(vertical, horizontal,brake);

    }

    void Drive(float accel,float steer,float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        float _torque = accel * torque;
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = _torque;
            if (i < 2)
                wheelColliders[i].steerAngle = steer;
            else
                wheelColliders[i].brakeTorque = brake;

            Quaternion quaternion;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quaternion);
            wheels[i].transform.position = position;
            wheels[i].transform.rotation = quaternion;

        }
    }
}
