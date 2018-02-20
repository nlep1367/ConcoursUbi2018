using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVision : MonoBehaviour {

    public enum SightPriority
    {
        Nearest,
        Farthest
    }

    public SightPriority Priority;

    public float SightDegreeAngle = 160.0f;
    public float SightDistance = 10.0f;
    
    public string[] ViewAbleTags = { "Players" };

    private float NormalizedDist;

    private List<Transform> Players;


	// Use this for initialization
	void Start () {
        Players = new List<Transform>();
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

    public void InitializePlayerList()
    {
        Players.Clear();

        for (int j = 0; j < ViewAbleTags.Length; ++j)
        {
            GameObject[] GOs = GameObject.FindGameObjectsWithTag(ViewAbleTags[j]);

            for (int i = 0; i < GOs.Length; ++i)
            {
                Players.Add(GOs[i].transform);
            }
        }
    }
    
    public bool SeeSomething(out Vector3 Target)
    {
        if (Players == null)
        { 
            Target = transform.position;
            return false;
        }

        if (Players.Count == 0)
        {
            InitializePlayerList();
        }

        switch(Priority)
        {
            case SightPriority.Farthest :
                return Furthest(out Target);
            case SightPriority.Nearest:
                return Nearest(out Target);
            default :
                Target = transform.position;
                return false;
        }


    }
    
    public bool Furthest(out Vector3 Target)
    {
        Transform CurrentTarget = null;

        foreach (Transform target in Players)
        {
            Vector3 Result = target.position - transform.position;
            float targetDistance = Result.magnitude;
            
            if (targetDistance > SightDistance) // Too far
                continue;
            if (!InsideVision(Result)) // not inside the sight radius
                continue;
            else if (CurrentTarget == null) // First target found within range
                CurrentTarget = target;
            else if (targetDistance < (CurrentTarget.position - transform.position).magnitude) // Is the new target further and the one we have right now
                CurrentTarget = target;
            else // The target is not the furthest
                continue;
        }
        
        if (CurrentTarget == null)
        { 
            Target = transform.position;
            return false;
        }
        else
        {
            Target = CurrentTarget.position;
            return true;
        }
    }

    public bool Nearest(out Vector3 Target)
    {
        Transform CurrentTarget = null;

        foreach (Transform target in Players)
        {
            Vector3 Result = target.position - transform.position;
            float targetDistance = Result.magnitude;

            if (targetDistance > SightDistance) // Too far
                continue;
            if (!InsideVision(Result)) // not inside the sight radius
                continue;
            else if (CurrentTarget == null) // First target found within range
                CurrentTarget = target;
            else if (targetDistance > (CurrentTarget.position - transform.position).magnitude) // Is the new target further and the one we have right now
                CurrentTarget = target;
            else // The target is not the furthest
                continue;
        }

        if (CurrentTarget == null)
        {
            Target = transform.position;
            return false;
        }
        else
        {
            Target = CurrentTarget.position;
            return true;
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