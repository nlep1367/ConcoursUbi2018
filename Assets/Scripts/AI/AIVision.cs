using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVision : MonoBehaviour {

    public float SightDegreeAngle = 160.0f;
    public float SightDistance = 10.0f;
    public float GirlDogProximityDistance = 5.0f;
    
    public string[] ViewAbleTags = { "Players" };

    private float NormalizedDist;

    private List<Transform> Players;

    private Transform GirlTransform;
    private Transform DoggoTransform;


    // Use this for initialization
    void Start () {
        GirlTransform = GameObject.FindGameObjectWithTag("Fille").transform;
        DoggoTransform = GameObject.FindGameObjectWithTag("Doggo").transform;
    }

    public void Update()
    {
        // Given an angle from 90 degree we find the sin that we require to determine if something is in vision radius
        NormalizedDist = Mathf.Sin((90 - (SightDegreeAngle / 2)) * Mathf.Deg2Rad);
    }

    public void OnEnable()
    {
       
    }

    public bool InsideVision(Vector3 DistanceVector)
    {
        return Vector3.Dot(transform.forward, DistanceVector.normalized) > NormalizedDist;
    }

    public bool SeeSomething(out Vector3 Target)
    {
        if (Vector3.Distance(GirlTransform.position, DoggoTransform.position) < GirlDogProximityDistance)
        {
            Target = transform.position;
            return false;
        }

        return IsDoggoInSight(out Target);
    }
    
    public bool IsDoggoInSight(out Vector3 Target)
    {
        Vector3 Result =   DoggoTransform.position - transform.position;
        float targetDistance = Result.magnitude;
            
        if (targetDistance < SightDistance && InsideVision(Result)) //Guard see the doggo
        {
            Target = DoggoTransform.position;
            return true;
        }
        else
        {
            Target = transform.position;
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        float sin = Mathf.Sin((90 - (SightDegreeAngle / 2)) * Mathf.Deg2Rad);
        float cos = Mathf.Cos((90 - (SightDegreeAngle / 2)) * Mathf.Deg2Rad);

        Vector3 forward = transform.forward * sin * SightDistance;
        Vector3 right = transform.right * cos * SightDistance;

        Vector3 RightArrival = (transform.position + forward + right);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, RightArrival);
        
        forward = transform.forward * sin * SightDistance;
        right = transform.right * -cos * SightDistance;

        Vector3 LeftArrival = (transform.position + forward + right);

        Gizmos.DrawLine(transform.position, LeftArrival);
    }
}