using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : BaseAI {
    
    public float ChaseLeashRange;
    public float SoundLeashRange;

    public float WanderMinDistance;
    public float WanderMaxDistance;

    public float WanderWaitingMin;
    public float WanderWaitingMax;

    private NavMeshAgent Agent;
    private Vector3 StartPosition;

    private bool SeeSomething = false;
    private Vector3 CurrentWanderVector;

    public float WanderWaitingTime;

    // Use this for initialization
    void Start () {
        Agent = GetComponent<NavMeshAgent>();
        StartPosition = transform.position;

        Vector2 wanderVector = Random.insideUnitCircle;
        float wanderDistance = Random.Range(WanderMinDistance, WanderMaxDistance);

        CurrentWanderVector = StartPosition + new Vector3(wanderVector.x * wanderDistance, 0, wanderVector.y * wanderDistance);

        WanderWaitingTime = Random.Range(WanderWaitingMin, WanderWaitingMax);
    }
	
	// Update is called once per frame
	void Update () {
        
        if(!SeeSomething)
        {
            if(CurrentStimuli != null && Vector3.Distance(StartPosition, CurrentStimuli.GetPosition()) < SoundLeashRange)
            {
                Agent.destination = CurrentStimuli.GetPosition();
            }
            else
            {
                Wander();
                Agent.destination = CurrentWanderVector;
            }

        }
	}

    public override bool IsBusy() 
    {
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying)
        { 
            StartPosition = transform.position;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(StartPosition, SoundLeashRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(StartPosition, ChaseLeashRange);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(CurrentWanderVector, 1.0f);
    }

    private void Wander()
    {
        float Distance = Vector3.Distance(CurrentWanderVector, transform.position);

        if (Distance < 0.5f && WanderWaitingTime <= 0)
        {
            Vector2 wanderVector = Random.insideUnitCircle;
            float wanderDistance = Random.Range(WanderMinDistance, WanderMaxDistance);

            CurrentWanderVector = StartPosition + new Vector3(wanderVector.x * wanderDistance, 0, wanderVector.y * wanderDistance);

            WanderWaitingTime = Random.Range(WanderWaitingMin, WanderWaitingMax);
        }
        else if (Distance < 0.5f)
        {
            WanderWaitingTime -= Time.deltaTime;
        }
    }
}
