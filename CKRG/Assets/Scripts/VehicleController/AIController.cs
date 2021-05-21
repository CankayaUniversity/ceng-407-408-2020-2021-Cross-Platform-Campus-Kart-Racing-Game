using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Waypoint waypoint;
    KartController kc;
    public float steeringSens = 0.01f;
    public float brakeSens = 1.1f;
    public float accelSens = 0.3f;
    Vector3 target;
    Vector3 nextTarget;
    int currentWP = 0;
    float totalDistanceToTarget;

    GameObject tracker;
    int currentTrackerWP = 0;
    public float lookAhead = 10f;

    float lastTimeMoving = 0;

    CheckpointManager checkpointManager;
    float finishSteer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (waypoint == null)
            waypoint = GameObject.FindGameObjectWithTag("Waypoint").GetComponent<Waypoint>();
        kc = GetComponent<KartController>();
        target = waypoint.waypoints[currentWP].transform.position;
        nextTarget = waypoint.waypoints[currentWP + 1].transform.position;
        totalDistanceToTarget = Vector3.Distance(target, kc.rb.gameObject.transform.position);

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = kc.rb.gameObject.transform.position;
        tracker.transform.rotation = kc.rb.gameObject.transform.rotation;
        this.GetComponent<Ghost>().enabled = false;
        finishSteer = Random.Range(-1.0f, 1.0f);
    }

    void ProgressTracker()
    {
        //Debug.DrawLine(kc.rb.gameObject.transform.position, tracker.transform.position);
        if (Vector3.Distance(kc.rb.gameObject.transform.position, tracker.transform.position) > lookAhead) return;
        tracker.transform.LookAt(waypoint.waypoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, 1.0f);

        if (Vector3.Distance(tracker.transform.position, waypoint.waypoints[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            if (currentTrackerWP >= waypoint.waypoints.Length)
                currentTrackerWP = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!RaceStarter.raceStart)
        {
            lastTimeMoving = Time.time;
            return;
        }

        if (checkpointManager == null)
            checkpointManager = kc.rb.GetComponent<CheckpointManager>();

        if(checkpointManager.lap == RaceStarter.totalLaps+1)
        {
            kc.highAccSound.Stop();
            kc.Go(0, finishSteer, 0);
            return;
        }


        ProgressTracker();
        Vector3 localTarget;
        float targetAngle;


        if (kc.rb.velocity.magnitude > 1)
            lastTimeMoving = Time.time;

        if (Time.time > lastTimeMoving + 4)
        {
            

            kc.rb.gameObject.transform.position = checkpointManager.lastCP.transform.position + Vector3.up * 2;
            kc.rb.gameObject.transform.rotation = checkpointManager.lastCP.transform.rotation;

            tracker.transform.position = checkpointManager.lastCP.transform.position;
            kc.rb.gameObject.layer = 8;
            this.GetComponent<Ghost>().enabled = true;
            Invoke("ResetLayer", 3);
        }

        if (Time.time < kc.rb.GetComponent<AvoidDetector>().avoidTime)
        {
            localTarget = tracker.transform.right * kc.rb.GetComponent<AvoidDetector>().avoidPath;
        }
        else
        {
            localTarget = kc.rb.gameObject.transform.InverseTransformPoint(tracker.transform.position);
        }

        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSens, -1, 1) * Mathf.Sign(kc.currentSpeed);

        float speedFactor = kc.currentSpeed / kc.maxSpeed;

        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90f;

        float brake = 0;
        if (corner > 10 && speedFactor > 0.1f)
            brake = Mathf.Lerp(0, 1 + speedFactor * brakeSens, cornerFactor);

        float accel = 1f;
        if (corner > 20 && speedFactor > 0.2f)
            accel = Mathf.Lerp(0, 1 * accelSens, 1 - cornerFactor);




        kc.Go(accel, steer, brake);

        kc.CheckForSkid();
        kc.CalculateEngineSound();
    }

    void ResetLayer()
    {
        kc.rb.gameObject.layer = 0;
        this.GetComponent<Ghost>().enabled = false;
    }
}
