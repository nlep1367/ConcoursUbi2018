using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGrounded : PlayerState {
    private const string AnimatorAction = "Moving";

    public float _movementSpeed;
    public float _rotationSpeed;

    public PStateGrounded(Player player, float ms, float rs) : base(player)
    {
        _movementSpeed = ms;
        _rotationSpeed = rs;
    }

    public override void InterpretInput()
    {
        float HorizontalAxis = Input.GetAxis("Horizontal_Move");

        if (HorizontalAxis >= float.Epsilon || HorizontalAxis <= -float.Epsilon)
        {
            _player.RigidBody.AddForce(HorizontalAxis * _player.transform.right * _movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            _player.RigidBody.AddForce(VerticalAxis * _player.transform.forward * _movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (HorizontalRotation != 0)
        {
            _player.transform.eulerAngles += new Vector3(0, Time.deltaTime * _rotationSpeed * HorizontalRotation, 0);
        }

        _player.Animator.SetFloat("Speed", _player.RigidBody.velocity.magnitude);
    }   

    public override void OnEnter(object o)
    {
        _player.Animator.SetBool(AnimatorAction, true);
    }

    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }
}
