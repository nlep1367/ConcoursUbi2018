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

    private GameObject doggo;

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

        if(Camera != null)
        {
            if (doggo == null)
            {
                doggo = GameObject.FindGameObjectWithTag("Doggo");
            }
            else
            {
                doggo.GetComponent<ObjectSync>().Rpc_SetScaredEffectColor(Camera.backgroundColor);
            }
        }
    }

    public override void SetCamera(Camera camera)
    {
        Camera = camera;
        ((PStateGrounded)States[StateEnum.GROUNDED]).SetCamera(camera);
    }
}
