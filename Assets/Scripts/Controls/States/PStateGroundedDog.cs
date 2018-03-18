using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGroundedDog : PStateGrounded
{
    private const string AnimatorAction = "Moving";

    private float _jumpSpeed;

    public PStateGroundedDog(Player player, float acceleration, float ms, float rs, float maxHeight) : base(player, acceleration, ms, rs)
    {
        _jumpSpeed = Mathf.Sqrt(Mathf.Abs(2 * maxHeight * Physics2D.gravity.y));
    }

    public override void InterpretInput()
    {
        base.InterpretInput();
        _player.Animator.SetFloat("Speed", _player.RigidBody.velocity.magnitude);

        if (Input.GetButton("Jump"))
        {
            _player.RigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.VelocityChange);
            _player.ChangeState(StateEnum.JUMPING);
        }
    }
}