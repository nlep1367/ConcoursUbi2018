using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class CarAI : NetworkBehaviour {

    public Material[] RandomColor;
    public bool isBreaking = false;
    public float Speed;
    public NavMeshAgent Agent;
    public Renderer Render;
    public Animator Anim;
    public NetworkAnimator Animdsa;

    private Path path;
    private int CurrentWaypoint;
    private float DistanceFromWaypoint = 5;
    private NetworkSpawner carSpawner;
    

    // Use this for initialization
    void Awake() {
        Render.material = RandomColor[Random.Range(0, RandomColor.Length)];
        Animdsa.SetParameterAutoSend(0, true);
    }
	
	// Update is called once per frame
    [Server]
	void Update () {
        if (isBreaking || path.GetWayPoint(CurrentWaypoint).ShouldStop(transform.position))
        {
            Agent.speed = 0;
            Agent.velocity = Vector3.zero;
        }
        else
        {
            Agent.speed = Speed;
        }

        Anim.SetFloat("Speed", Agent.speed);

        if (Vector3.Distance(path.GetWayPoint(CurrentWaypoint).transform.position, transform.position) < DistanceFromWaypoint)
        {
            CurrentWaypoint = path.GetNextWayPoint(CurrentWaypoint);

            if (CurrentWaypoint == -1)
            {
                CurrentWaypoint = 0;
                carSpawner.ReturnToPool(gameObject);
            }
            else
            {
                Agent.SetDestination(path.GetWayPoint(CurrentWaypoint).transform.position);
            }

        }
    }

    public void Initialize(NetworkSpawner cs, Path p)
    {
        carSpawner = cs;
        path = p;
        CurrentWaypoint = 0;
        Agent.speed = Speed;

        Agent.SetDestination(path.GetWayPoint(CurrentWaypoint).transform.position);
        // Can eventually assign random values to motor torque, etc...
    }
}
