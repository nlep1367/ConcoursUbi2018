using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateJumping : PlayerState {
    private const string AnimatorAction = "Jumping";

    private float _movementSpeed;
    private float _rotationSpeed;

    private float _fallModifier;
    private float _lowJumpModifier;

    public PStateJumping(Player player, float ms, float rs, float fallModifier, float lowJumpModifier) : base(player)
    {
        _movementSpeed = ms;
        _rotationSpeed = rs;

        _fallModifier = fallModifier;
        _lowJumpModifier = lowJumpModifier;
    }

    public override void InterpretInput()
    {
        // In air movement
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

        //Gravity tampering
        if (_player.RigidBody.velocity.y < 0)
        {
            Vector3 force = Vector3.up * Physics.gravity.y * _fallModifier * Time.deltaTime;
            _player.RigidBody.AddForce(force, ForceMode.VelocityChange);
        }
        else if (_player.RigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            Vector3 force = Vector3.up * Physics.gravity.y * _lowJumpModifier * Time.deltaTime;
            _player.RigidBody.AddForce(force, ForceMode.VelocityChange);
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
