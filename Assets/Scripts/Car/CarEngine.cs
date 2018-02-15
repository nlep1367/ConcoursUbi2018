using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {

    public Path path;
    public float maxSteer = 45f;
    public float turnSpeed = 5f;
    public float maxMotorTorque = 200f;
    public float maxBreakTorque = 600f;
    public float maxSpeed = 40f;
    public float currentSpeed = 0f;

    // Max Distance between car and waypoint to advance
    public float distanceTolerance = 1f;
    public Vector3 centerOfMass;
    public Texture2D textureNormal;
    public Texture2D textureBreaking;
    public Renderer carRenderer; // Should be Renderer

    [Header("Wheel Coliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    private bool isBreaking = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public Vector3 frontSensorPosition = new Vector3(0f,0f, 2f);
    public float frontSideSensorPosition = 1f;
    public float frontSensorAngle = 30f;
    
    private int currentWayPoint = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }

    public void Initialize(Vector3 pos, Quaternion rotation, Path p)
    {
        transform.position = pos;
        transform.rotation = rotation;
        path = p;
        currentWayPoint = 0;

        // Can eventually assign random values to motor torque, etc...
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
        if (isActiveAndEnabled)
        {
            CheckTrafficLight();
            Breaking();
            LerpToSteerAngle();
        }
    }

    private void ApplySteer()
    {
        if (avoiding) return;

        Vector3 relativeVector = transform.InverseTransformPoint(path.GetWayPoint(currentWayPoint).transform.position);
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
        if(Vector3.Distance(transform.position, path.GetWayPoint(currentWayPoint).transform.position) < distanceTolerance)
        {
            currentWayPoint = path.GetNextWayPoint(currentWayPoint);
            if (currentWayPoint == -1) gameObject.SetActive(false);
        }
    }

    private void Breaking()
    {
        if (isBreaking)
        {
            carRenderer.material.mainTexture = textureBreaking;
            wheelFL.brakeTorque = maxBreakTorque;
            wheelFR.brakeTorque = maxBreakTorque;
            wheelRL.brakeTorque = maxBreakTorque;
            wheelRR.brakeTorque = maxBreakTorque;
        }
        else
        {
            carRenderer.material.mainTexture = textureNormal;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
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
                avoidMultiplier -= 1f;
            }
        }

        // Front right angle Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
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
                avoidMultiplier += 1f;
            }
        }

        // Front left angle Sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
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
                        avoidMultiplier -= 1f;
                    }
                    else
                    {
                        avoidMultiplier += 1f;
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

    private void CheckTrafficLight()
    {
        if (path.GetWayPoint(currentWayPoint).ShouldStop(transform.position))
        {
            isBreaking = true;
        }
        else
        {
            isBreaking = false;
        }
        
    }
}
