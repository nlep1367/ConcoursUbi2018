using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Transform path;
    public float maxSteer = 45f;
    public float maxMotorTorque = 150f;
    public float maxSpeed = 100f;
    public float currentSpeed = 0f;
    public float distanceTolerance = 2f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;

    private List<Transform> wayPoints;
    private int currentWayPoint = 0;

	// Use this for initialization
	void Start () {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        wayPoints = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; ++i)
        {
            if (pathTransforms[i] != path.transform)
            {
                wayPoints.Add(pathTransforms[i]);
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        ApplySteer();
        Drive();
        CheckWayPointDistance();
	}

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(wayPoints[currentWayPoint].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteer;

        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if(currentSpeed < maxSpeed)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }

    }

    private void CheckWayPointDistance()
    {
        if(Vector3.Distance(transform.position, wayPoints[currentWayPoint].position) < distanceTolerance)
        {
            currentWayPoint = (++currentWayPoint) % wayPoints.Count;            
        }
    }
}
