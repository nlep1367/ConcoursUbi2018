using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Rigidbody RigidBody;

    public PlayerState State;
    public PlayerState PreviousState;

    public Dictionary<StateEnum, PlayerState> States;

    public float MovementSpeed;
    public float RotationSpeed;
    public float PushingMovementSpeed;

    public float ClimbingSpeed;

    // Use this for initialization
    void Start()
    {
        RigidBody = GetComponent<Rigidbody>();

        //Initialize currentstate and possible states
        State = new PStateGrounded(this, MovementSpeed, RotationSpeed);
        PreviousState = State;
        States = new Dictionary<StateEnum, PlayerState>
        {
            { StateEnum.GROUNDED, State},
            { StateEnum.CLIMBING, new PStateClimbing(this, ClimbingSpeed, RotationSpeed) },
            { StateEnum.GETTING_OFF_LADDER, new PStateScriptedLadder(this) },
            { StateEnum.PUSHING, new PStatePushing(this, PushingMovementSpeed) }
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        State.InterpretInput();
    }

    public void ChangeState(StateEnum newStateEnum)
    {
        ChangeState(newStateEnum, null);
    }

    public void ChangeState(StateEnum newStateEnum, Object o)
    {
        if (States[newStateEnum] != State)
        {
            PreviousState = State;
            State.ChangeState(States[newStateEnum], o);
        }
    }
}
