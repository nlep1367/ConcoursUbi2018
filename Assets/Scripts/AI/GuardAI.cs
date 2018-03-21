using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class GuardAI : BaseAI {
    
    public float ChaseLeashRange;
    public float SoundLeashRange;
    
    public float ChaseSpeed = 7.0f;
    public float NormalSpeed = 3.5f;
    public float HearingSpeed = 4.5f;

    public IdleBehaviour AI_IdleBehaviour;
    public AIVision Vision;

    private NavMeshAgent Agent;
    private Vector3 StartPosition;
    private bool Busy = false;

    // Use this for initialization
    void Start ()
    {
        StartPosition = transform.position;
        Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    [Server]
    void Update () {

        Vector3 EnemySeen;

        if(!Vision.SeeSomething(out EnemySeen))
        {
            Busy = false;
            // Maybe I hear something
            if (CurrentStimuli != null && // We have a stimuli
                (transform.position - CurrentStimuli.GetPosition()).magnitude < HearingRange && // Hearing something around me
                (transform.position - StartPosition).magnitude < SoundLeashRange) // Am I too far?
            {
                Agent.destination = CurrentStimuli.GetPosition();
                Agent.speed = HearingSpeed;
            }
            else
            {
                Agent.destination = AI_IdleBehaviour.Process();
                Agent.speed = NormalSpeed;
            }
        }
        else
        {
            Busy = true;
            // I see something
            if ((transform.position - StartPosition).magnitude < ChaseLeashRange)
            {
                Agent.destination = EnemySeen;
                Agent.speed = ChaseSpeed;
            }
            else
            {
                Agent.destination = AI_IdleBehaviour.Process();
                Agent.speed = NormalSpeed;
            }

        }
	}
    
    public override bool IsBusy() 
    {
        return Busy;
    }

    private void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying)
        { 
            StartPosition = transform.position;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(StartPosition, SoundLeashRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, HearingRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(StartPosition, ChaseLeashRange);
    }
}
