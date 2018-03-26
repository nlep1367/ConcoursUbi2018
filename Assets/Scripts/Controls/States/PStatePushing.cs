using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStatePushing : PlayerState
{
    private const string AnimatorAction = "Pushing";

    float _movementSpeed;
    Rigidbody _rigidBodyToPush;
    float _mass;

    public PStatePushing(Player player, float ms) : base(player)
    {
        _movementSpeed = ms;
    }

    public override void InterpretInput()
    {
        float VerticalAxis = Input.GetAxis("Vertical_Move");
        float HorizontalAxis = Input.GetAxis("Horizontal_Move");

        //TODO INPUT
        if(Input.GetButtonDown("A"))
        {
            _player.ChangeState(StateEnum.GROUNDED);
        }
        else if (VerticalAxis >= float.Epsilon)
        {
           // Vector3 force = VerticalAxis * _player.transform.forward * _movementSpeed * Time.deltaTime;
            _player.RigidBody.velocity = _player.transform.forward * _movementSpeed * VerticalAxis;
        }
        else if (VerticalAxis <= -0.3f || HorizontalAxis <= -0.3f || HorizontalAxis >= 0.3f)
        {
            _player.ChangeState(StateEnum.GROUNDED);
        }
    }

    public override void OnEnter(object o)
    {
        _rigidBodyToPush = (Rigidbody)o;
        _mass = _rigidBodyToPush.mass;
        _rigidBodyToPush.mass = 0.01f;

        _player.Animator.SetBool(AnimatorAction, true);
    }

    public override void OnExit()
    {
        _rigidBodyToPush.mass = _mass;

        _player.Animator.SetBool(AnimatorAction, false);
    }
}
