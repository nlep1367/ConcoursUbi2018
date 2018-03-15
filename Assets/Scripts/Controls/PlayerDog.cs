using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDog : Player
{

    public float MovementSpeed = 15f;
    public float RotationSpeed = 25f;
    public float Acceleration = 30f;

    public float MaxHeight = 5f;

    public float InAirMovementSpeed = 5f;

    public float FallModifier = 1.5f;
    public float LowJumpModifier = 1f;

    new void Start()
    {
        GroundedState = new PStateGroundedDog(this, Acceleration, MovementSpeed, RotationSpeed, MaxHeight);
        State = GroundedState;
        base.Start();

        //Initialize currentstate and possible states;

        PreviousState = State;
        States = new Dictionary<StateEnum, PlayerState>
        {
            { StateEnum.GROUNDED, State},
            { StateEnum.JUMPING, new PStateJumping(this, InAirMovementSpeed, RotationSpeed, LowJumpModifier) },
            { StateEnum.FALLING, new PStateFalling(this, FallModifier) }
        };
    }
}
