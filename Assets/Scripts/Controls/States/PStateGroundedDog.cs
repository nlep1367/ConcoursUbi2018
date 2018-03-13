using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGroundedDog : PlayerState
{
    private const string AnimatorAction = "Moving";

    private float _maxSpeed;

    private float _acceleration;
    private float _rotationSpeed;
    private float _jumpSpeed;

    public PStateGroundedDog(Player player, float acceleration, float ms, float rs, float maxHeight) : base(player)
    {
        _maxSpeed = ms;
        _acceleration = acceleration;
        _rotationSpeed = rs;

        _jumpSpeed = Mathf.Sqrt(Mathf.Abs(2 * maxHeight * Physics2D.gravity.y));
    }

    public override void InterpretInput()
    {
        Vector3 force = Vector3.zero;
        float HorizontalAxis = Input.GetAxis("Horizontal_Move");
       
        if (HorizontalAxis >= float.Epsilon || HorizontalAxis <= -float.Epsilon)
        {
            force += HorizontalAxis * _player.transform.right * _acceleration;
            //_player.RigidBody.AddForce(HorizontalAxis * _player.transform.right * _movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            force += VerticalAxis * _player.transform.forward * _acceleration;
            //_player.RigidBody.AddForce(VerticalAxis * _player.transform.forward * _movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        //Clamping speed
        _player.RigidBody.AddForce(Vector3.ClampMagnitude(force, _acceleration), ForceMode.Acceleration);
        _player.RigidBody.velocity = Vector3.ClampMagnitude(_player.RigidBody.velocity, _maxSpeed);

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

    public override void OnEnter(object o)
    {
        _player.Animator.SetBool(AnimatorAction, true);
    }

    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }
}