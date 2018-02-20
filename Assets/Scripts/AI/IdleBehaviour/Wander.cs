using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : IdleBehaviour
{
    public float WanderMinDistance;
    public float WanderMaxDistance;

    public float WanderWaitingMin;
    public float WanderWaitingMax;

    public float WanderWaitingTime;

    private Vector3 StartPosition;
    private Vector3 CurrentWanderVector;
    
    private void Start()
    {
        StartPosition = transform.position;

        Vector2 wanderVector = Random.insideUnitCircle;
        float wanderDistance = Random.Range(WanderMinDistance, WanderMaxDistance);

        CurrentWanderVector = StartPosition + new Vector3(wanderVector.x * wanderDistance, 0, wanderVector.y * wanderDistance);

        WanderWaitingTime = Random.Range(WanderWaitingMin, WanderWaitingMax);
    }

    public override Vector3 Process()
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

        return CurrentWanderVector;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            StartPosition = transform.position;
        }

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(CurrentWanderVector, 1.0f);
    }
}
