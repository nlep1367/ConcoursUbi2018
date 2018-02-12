using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Transform path;
    public float maxSteer = 45f;
    public float turnSpeed = 5f;
    public float maxMotorTorque = 150f;
    public float maxBreakTorque = 500f;
    public float maxSpeed = 100f;
    public float currentSpeed = 0f;

    // Max Distance between car and waypoint to advance
    public float distanceTolerance = 2f;
    public Vector3 centerOfMass;
    public Texture2D textureNormal;
    public Texture2D textureBreaking;
    public Renderer carRenderer; // Should be Renderer

    [Header("Wheel Coliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public bool isBreaking = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public Vector3 frontSensorPosition = new Vector3(0f,0f, 2f);
    public float frontSideSensorPosition = -1f;
    public float fontSensorAngle = 30f;


    private List<Transform> wayPoints;
    private int currentWayPoint = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0;

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
        Sensors();
        ApplySteer();
        Drive();
        CheckWayPointDistance();
        Breaking();
        LerpToSteerAngle();
    }

    private void ApplySteer()
    {
        if (avoiding) return;

        Vector3 relativeVector = transform.InverseTransformPoint(wayPoints[currentWayPoint].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteer;

        targetSteerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if(!isBreaking && currentSpeed < maxSpeed)
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

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;

        float avoidMultiplier = 0;
        avoiding = false;

        // Front right Sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1f;
            }
        }

        // Front right angle Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-fontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
            }
        }

        // Front left Sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1f;
            }
        }

        // Front left angle Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(fontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        if(avoidMultiplier == 0)
        {
            // Front center Sensor
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if(hit.normal.x < 0)
                    {
                        avoidMultiplier += 1f;
                    }
                    else
                    {
                        avoidMultiplier -= 1f;
                    }
                }
            }
        }

        if (avoiding)
        {
            targetSteerAngle = maxSteer * avoidMultiplier;
        }
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }
}
