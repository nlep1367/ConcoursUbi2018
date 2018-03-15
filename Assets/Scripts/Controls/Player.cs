using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public abstract class Player : NetworkBehaviour
{
    public Rigidbody RigidBody;

    public PlayerState State;
    public PlayerState PreviousState;

    public Dictionary<StateEnum, PlayerState> States;

    public Animator Animator;

    // Use this for initialization
    public virtual void Start()
    {
        RigidBody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
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
