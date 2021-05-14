using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
    public float avoidPath = 0;
    public float avoidTime = 0;
    public float wonderDistance = 4;
    public float avoidLength = 1;
    // Start is called before the first frame update

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Car") return;

        Rigidbody otherCar = collision.rigidbody;
        avoidTime = Time.time + avoidLength;

        Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
        float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
        avoidPath = wonderDistance * -Mathf.Sign(otherCarAngle);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Car") return;
        avoidTime = 0;

    }
}
