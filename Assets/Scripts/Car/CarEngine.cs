using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarEngine : NetworkBehaviour
{

    public NetworkSpawner carSpawner;
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
    public Color ColorNoBrake;
    public Color ColorBraking;
    public Renderer carRenderer; // Should be Renderer

    [Header("Wheel Coliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public bool isBreaking = false;
    public bool hasObstacle = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0f, 2f);
    public float frontSideSensorPosition = 1f;
    public float frontSensorAngle = 30f;

    public int currentWayPoint = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0;

    public Color[] RandomColor;

    private Rigidbody Mybody;

	// Use this for initialization
	void Start () {
        Mybody = GetComponent<Rigidbody>();
        Mybody.centerOfMass = centerOfMass;
        carRenderer.material.color = RandomColor[Random.Range(0, RandomColor.Length - 1)];
    }

    public void Initialize(NetworkSpawner cs, Path p)
    {
        carSpawner = cs;
        path = p;
        currentWayPoint = 0;

        // Can eventually assign random values to motor torque, etc...
    }

    private void OnDrawGizmosSelected()
    {/*
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(centerOfMass + transform.position, 0.1f);

        Vector3 sensorStartPos = transform.position + centerOfMass;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        //center
        Gizmos.DrawLine(sensorStartPos, sensorStartPos + new Vector3(0, 0, sensorLength));
        //right-1
        sensorStartPos += transform.right * frontSideSensorPosition/2;
        Gizmos.DrawLine(sensorStartPos, sensorStartPos + new Vector3(0, 0, sensorLength));
        //right
        sensorStartPos += transform.right * frontSideSensorPosition/2;
        Gizmos.DrawLine(sensorStartPos, sensorStartPos + new Vector3(0, 0, sensorLength));
        //left
        sensorStartPos -= transform.right * frontSideSensorPosition*2;
        Gizmos.DrawLine(sensorStartPos, sensorStartPos + new Vector3(0, 0, sensorLength));
        //left-1
        sensorStartPos += transform.right * frontSideSensorPosition/2;
        Gizmos.DrawLine(sensorStartPos, sensorStartPos + new Vector3(0, 0, sensorLength));
        */

        if (path)
        {
            WayPoint w = path.GetWayPoint(currentWayPoint);
            Gizmos.DrawCube(w.transform.position, new Vector3(3,3,3));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            WheelCollider[] wheelColliders = GetComponentsInChildren<WheelCollider>();

            for (int i = 0; i < wheelColliders.Length; ++i)
                Physics.IgnoreCollision(collision.collider, wheelColliders[i], true);
        }      
    }

    // Update is called once per frame
    [Server]
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

    [Server]
    private void ApplySteer()
    {
        if (avoiding) return;

        Vector3 relativeVector = transform.InverseTransformPoint(path.GetWayPoint(currentWayPoint).transform.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteer;

        targetSteerAngle = newSteer;
    }

    [Server]
    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60f / 1000f;

        if(!isBreaking && !hasObstacle && currentSpeed < maxSpeed)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else if (isBreaking || hasObstacle)
        {
            wheelFL.motorTorque = -maxMotorTorque;
            wheelFR.motorTorque = -maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    [Server]
    private void CheckWayPointDistance()
    {
        if(Vector3.Distance(transform.position, path.GetWayPoint(currentWayPoint).transform.position) < distanceTolerance)
        {
            currentWayPoint = path.GetNextWayPoint(currentWayPoint);
            if (currentWayPoint == -1)
            {
                currentWayPoint = 0;
                carSpawner.ReturnToPool(gameObject);
            }
        }
    }

    [Server]
    private void Breaking()
    {
        if (isBreaking || hasObstacle)
        {
            carRenderer.material.SetColor("_EmissionColor", ColorNoBrake);
            GetComponentInParent<Rigidbody>().isKinematic = true;
            wheelFL.brakeTorque = maxBreakTorque;
            wheelFR.brakeTorque = maxBreakTorque;
            wheelRL.brakeTorque = maxBreakTorque;
            wheelRR.brakeTorque = maxBreakTorque;
        }
        else
        {
            carRenderer.material.SetColor("_EmissionColor", ColorNoBrake);

            GetComponentInParent<Rigidbody>().isKinematic = false;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
        
    }   

    [Server]
    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position + centerOfMass;
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
    
    [Server]
    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    [Server]
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
