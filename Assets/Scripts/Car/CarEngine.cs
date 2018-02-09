using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Transform path;
    public float maxSteer = 45f;
    public float maxMotorTorque = 150f;
    public float maxBreakTorque = 500f;
    public float maxSpeed = 100f;
    public float currentSpeed = 0f;

    // Max Distance between car and waypoint to advance
    public float distanceTolerance = 2f;
    public Vector3 centerOfMass;

    [Header("Wheel Coliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public bool isBreaking = false;
    public Texture2D textureNormal;
    public Texture2D textureBreaking;
    public Renderer carRenderer; // Should be Renderer

    [Header("Sensors")]
    public float sensorLength = 5f;

    private List<Transform> wayPoints;
    private int currentWayPoint = 0;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(centerOfMass + transform.position, 0.1f);        
    }

    // Update is called once per frame
    void FixedUpdate () {
        ApplySteer();
        Drive();
        CheckWayPointDistance();
        Breaking();
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

        if(currentSpeed < maxSpeed && !isBreaking)
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

    private void Breaking()
    {
        if (isBreaking)
        {
            carRenderer.material.mainTexture = textureBreaking;
            wheelRL.brakeTorque = maxBreakTorque;
            wheelRR.brakeTorque = maxBreakTorque;
        }
        else
        {
            carRenderer.material.mainTexture = textureNormal;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }

    private void Sensors
}
