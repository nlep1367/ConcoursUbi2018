using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine;

public enum SquirrelMode
{
    TwoState,
    ThreeState,
    ThreeStateLooping,
    Over
}

[RequireComponent(typeof(NavMeshAgent), typeof(FadeMaterial))]
public class FleeAI : NetworkBehaviour
{
    public SquirrelMode Mode;

    public Transform FirstPoint;
    public Transform SecondPoint;
    public Transform ThirdPoint;

    public Transform LastPoint;
    
    public bool SPOOOKY;

    private Transform CurrentPoint;
    private NavMeshAgent Agent;
    private FadeMaterial Killer;
    private DogBark Woofer;

    private Animator Animator;

    public System.Action WasSpooked;

    private bool sendInfo;

    void Start()
    {
        if (FirstPoint != null)
            Initialize();

    }

    public void Initialize()
    {
        Animator = GetComponent<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        Killer = GetComponent<FadeMaterial>();

        CurrentPoint = FirstPoint;
        Agent.transform.position = FirstPoint.position;
    }

    [Server]
    void Spook(Vector3 SpookyLocation)
    {
        if (Vector3.Distance(SpookyLocation, transform.position) > Adaptation.BarkRange)
            return;

        if (WasSpooked != null)
            WasSpooked.Invoke();

        switch (Mode)
        {
            case SquirrelMode.TwoState:
                if (CurrentPoint == FirstPoint)
                    FinalFlee(SpookyLocation);
                break;

            case SquirrelMode.ThreeState:
                if (CurrentPoint == FirstPoint)
                    CurrentPoint = SecondPoint;
                else if (CurrentPoint == SecondPoint)
                    FinalFlee(SpookyLocation);
                break;

            case SquirrelMode.ThreeStateLooping:
                if (CurrentPoint == FirstPoint)
                    CurrentPoint = SecondPoint;
                else if (CurrentPoint == SecondPoint)
                    CurrentPoint = ThirdPoint;
                else if (CurrentPoint == ThirdPoint)
                    CurrentPoint = FirstPoint;
                break;
        }

		if (Mode != SquirrelMode.Over) {
			Agent.SetDestination (CurrentPoint.position);
		}

		transform.GetComponent<SquirrelSoundControl> ().PlaySpooked ();
    }

    void Update()
    {
        if (Animator == null)
            return;

        if(SPOOOKY)
        {
            SPOOOKY = false;
            Spook(new Vector3( 0, 0, 0 ));
			transform.GetComponent<SquirrelSoundControl> ().PlayInitSpooked ();
        }
        
        if(!Woofer)
        {
            GameObject doggo = GameObject.FindGameObjectWithTag("Doggo");

            if(doggo)
            {
                Woofer = doggo.GetComponent<DogBark>();
                Woofer.HasBarked += Spook;
            }
        }

        ServerUpdate();
    }


    [Server]
    void ServerUpdate()
    {
        Animator.SetFloat("Speed", Agent.velocity.magnitude);

    }
    void FinalFlee(Vector3 SpookyLocation)
    {
        Agent.SetDestination(LastPoint.position);

        Mode = SquirrelMode.Over;

		Woofer.HasBarked -= Spook;
        Killer.Rpc_Kill();
    }
}
