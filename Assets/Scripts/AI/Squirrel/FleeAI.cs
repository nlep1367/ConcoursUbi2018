using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum SquirrelMode
{
    TwoState,
    ThreeState,
    ThreeStateLooping,
    Over
}

[RequireComponent(typeof(NavMeshAgent), typeof(FadeMaterial))]
public class FleeAI : MonoBehaviour
{
    public SquirrelMode Mode;

    public Transform FirstPoint;
    public Transform SecondPoint;
    public Transform ThirdPoint;
    
    public float SpookStrenght;
    public bool SPOOOKY;

    private Transform CurrentPoint;
    private NavMeshAgent Agent;
    private FadeMaterial Killer;
    private DogBark Woofer;

    private Animator Animator;

    public System.Action WasSpooked;

    void Start()
    {
        Animator = GetComponent<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        Killer = GetComponent<FadeMaterial>();
        CurrentPoint = FirstPoint;
        Agent.transform.position = FirstPoint.position;
    }
    
    void Spook(Vector3 SpookyLocation)
    {
        if (Vector3.Distance(SpookyLocation, transform.position) > Woofer.BarkRange)
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

        if(Mode != SquirrelMode.Over)
            Agent.SetDestination(CurrentPoint.position);
    }

    void Update()
    {
        if(SPOOOKY)
        {
            SPOOOKY = false;
            Spook(new Vector3( 0, 0, 0 ));
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
        Animator.SetFloat("Speed", Agent.velocity.magnitude);

    }

    void FinalFlee(Vector3 SpookyLocation)
    {
        Vector3 Dest = transform.position + ((transform.position - SpookyLocation) * SpookStrenght);
        Agent.SetDestination(Dest);

        Mode = SquirrelMode.Over;
        Killer.Rpc_Kill();
    }
}
