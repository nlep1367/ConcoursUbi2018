using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDog : Player
{
    public float MaxSpeed = 15f;
    public float RotationSpeed = 25f;
    public float Acceleration = 30f;

    public float LandingTime = 0.5f;
    public float MaxHeight = 2f;

    public float InAirAcceleration = 5f;

    public float FallModifier = 1.5f;
    public float LowJumpModifier = 1f;

    new private void Awake()
    {
        base.Awake();
        GameEssentials.PlayerDog = this;

        State = new PStateGroundedDog(this, Acceleration, MaxSpeed, RotationSpeed, MaxHeight); ;
        PreviousState = State;

        States = new Dictionary<StateEnum, PlayerState>
        {
            { StateEnum.GROUNDED, State},
            { StateEnum.JUMPING, new PStateJumping(this, InAirAcceleration, MaxSpeed, RotationSpeed, LowJumpModifier) },
            { StateEnum.FALLING, new PStateFalling(this, FallModifier) },
            { StateEnum.LANDING, new PStateLanding(this, Acceleration, MaxSpeed, RotationSpeed, LandingTime) },
            { StateEnum.GRABBING, new PStateGrabbing(this) },
            { StateEnum.BARKING, new PStateBarking(this, Acceleration, MaxSpeed, RotationSpeed) },
        };
    }

    public override void SetCamera(Camera camera)
    {
        Camera = camera;
        ((PStateGroundedDog)States[StateEnum.GROUNDED]).SetCamera(camera);
        ((PStateJumping)States[StateEnum.JUMPING]).SetCamera(camera);
        ((PStateLanding)States[StateEnum.LANDING]).SetCamera(camera);
        ((PStateBarking)States[StateEnum.BARKING]).SetCamera(camera);
    }
}
