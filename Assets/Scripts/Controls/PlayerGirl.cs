using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGirl : Player {
    public float MovementSpeed = 10f;
    public float RotationSpeed = 45f;
    public float Acceleration = 30f;
    public float PushingMovementSpeed = 5f;

    public float ClimbingSpeed = 10f;

    public bool shaderActivated = true;


    new private void Awake()
    {
        base.Awake();

        State = new PStateGroundedGirl(this, Acceleration, MovementSpeed, RotationSpeed);
        PreviousState = State;

        States = new Dictionary<StateEnum, PlayerState>
        {
            { StateEnum.GROUNDED, State},
            { StateEnum.CLIMBING, new PStateClimbing(this, ClimbingSpeed, RotationSpeed) },
            { StateEnum.GETTING_OFF_LADDER, new PStateScriptedLadder(this) },
            { StateEnum.PUSHING, new PStatePushing(this, PushingMovementSpeed) },
        };
    }

    new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Camera cam= GetComponent<UpdateEcho>().Cam;
            shaderActivated = !shaderActivated;
            cam.GetComponent<ReplacementShaderCam>().enabled = shaderActivated;
        }
    }

    public override void SetCamera(Camera camera)
    {
        ((PStateGrounded)States[StateEnum.GROUNDED]).SetCamera(camera);
    }
}
