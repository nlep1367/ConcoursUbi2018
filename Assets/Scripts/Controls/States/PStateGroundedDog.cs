using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGroundedDog : PlayerState
{
    private const string AnimatorAction = "Moving";

    private float _movementSpeed;
    private float _rotationSpeed;
    private float _jumpSpeed;

    public PStateGroundedDog(Player player, float ms, float rs, float maxHeight) : base(player)
    {
        _movementSpeed = ms;
        _rotationSpeed = rs;

        _jumpSpeed = Mathf.Sqrt(Mathf.Abs(2 * maxHeight * Physics2D.gravity.y));
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

        if(Input.GetButton("Jump"))
        {
            _player.RigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.VelocityChange);
            _player.ChangeState(StateEnum.JUMPING);
        }
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