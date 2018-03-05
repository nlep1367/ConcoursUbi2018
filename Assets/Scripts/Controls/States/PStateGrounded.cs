using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGrounded : PlayerState {
    private const string AnimatorAction = "Moving";

    public float MovementSpeed;
    public float RotationSpeed;

    public PStateGrounded(Player player, float ms, float rs) : base(player)
    {
        MovementSpeed = ms;
        RotationSpeed = rs;
    }

    public override void InterpretInput()
    {
        float HorizontalAxis = Input.GetAxis("Horizontal_Move");

        if (HorizontalAxis >= float.Epsilon || HorizontalAxis <= -float.Epsilon)
        {
            _player.RigidBody.AddForce(HorizontalAxis * _player.transform.right * MovementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            _player.RigidBody.AddForce(VerticalAxis * _player.transform.forward * MovementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (HorizontalRotation != 0)
        {
            _player.transform.eulerAngles += new Vector3(0, Time.deltaTime * RotationSpeed * HorizontalRotation, 0);
        }

        _player.Animator.SetFloat("Speed", _player.RigidBody.velocity.magnitude);
    }

    public override void OnEnter()
    {
        _player.Animator.SetBool(AnimatorAction, true);
    }

    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }
}
