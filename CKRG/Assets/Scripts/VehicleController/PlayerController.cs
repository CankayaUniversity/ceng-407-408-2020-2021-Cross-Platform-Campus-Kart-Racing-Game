using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    KartController kc;
    // Start is called before the first frame update
    void Start()
    {
        kc = GetComponent<KartController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float brake = Input.GetAxis("Jump");

        kc.Go(vertical, horizontal, brake);
        kc.CheckForSkid();
        kc.CalculateEngineSound();
    }
}
