using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGirl : Player {
    public float MaxMovementSpeed = 25f;
    public float MovementSpeed = 10f;
    public float RotationSpeed = 45f;
    public float Acceleration = 30f;
    public float PushingMovementSpeed = 5f;

    public float ClimbingSpeed = 10f;

    public bool shaderActivated = true;

    private ObjectSync doggoOS;
    public Fear fear;

    new private void Awake()
    {
        base.Awake();
        GameEssentials.PlayerGirl = this;
        State = new PStateGroundedGirl(this, Acceleration, MovementSpeed, RotationSpeed);
        PreviousState = State;

        States = new Dictionary<StateEnum, PlayerState>
        {
            { StateEnum.GROUNDED, State},
            { StateEnum.CLIMBING, new PStateClimbing(this, ClimbingSpeed, RotationSpeed) },
            { StateEnum.GETTING_OFF_LADDER, new PStateScriptedLadder(this) },
            { StateEnum.PUSHING, new PStatePushing(this, PushingMovementSpeed) },
            { StateEnum.GRABBING, new PStateGrabbing(this) },
            { StateEnum.TALKING, new PStateTalking(this) },
            { StateEnum.READING, new PStateReading(this) },
        };
    }

    new void Update()
    {
        base.Update();

        AdjustMovementSpeed();
    }

    public override void SetCamera(Camera camera)
    {
        Camera = camera;
        ((PStateGrounded)States[StateEnum.GROUNDED]).SetCamera(camera);
    }

    public void AdjustMovementSpeed()
    {
        float ratio = 0;
        switch (fear.fearState)
        {
            case Fear.FearState.Calm:
                ratio = 1f;
                break;

            case Fear.FearState.Anxious:
                ratio = .75f;
                break;

            case Fear.FearState.Stress:
                ratio = .5f;
                break;

            case Fear.FearState.Panic:
                ratio = .25f;
                break;

            case Fear.FearState.NearDeath:
                ratio = .1f;
                break;
        }

        if (Camera != null)
        {
            LerpColor lc = Camera.GetComponent<LerpColor>();
            if (ratio <= 0.5f && lc.GetCurrentEmotion() == Emotion.Positive)
            {
                lc.SetEmotions(Emotion.Negative);
            }
            else if (ratio > 0.5f && lc.GetCurrentEmotion() == Emotion.Negative)
            {
                lc.SetEmotions(Emotion.Positive);
            }
        }


        PStateGrounded state = States[StateEnum.GROUNDED] as PStateGrounded;
        state.SetMovementSpeed(MaxMovementSpeed * ratio);
    }
}
