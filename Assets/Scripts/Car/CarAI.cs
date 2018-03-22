using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class CarAI : NetworkBehaviour {

    public Color[] RandomColor;

    public bool isBreaking = false;
    private Path path;
    public float Speed;

    private int CurrentWaypoint;
    private float DistanceFromWaypoint = 5;
    public NavMeshAgent Agent;
    public Renderer Render;
    private NetworkSpawner carSpawner;

    // Use this for initialization
    void Awake() {
        Render.material.color = RandomColor[Random.Range(0, RandomColor.Length)];
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

		if(Vector3.Distance(path.GetWayPoint(CurrentWaypoint).transform.position, transform.position) < DistanceFromWaypoint)
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
