
using System;
using UnityEngine;

public class PStateClimbing : PlayerState
{
    public float RotationSpeed;
    public float ClimbingSpeed;

    public PStateClimbing(Player player, float cs, float rs) : base(player)
    {
        ClimbingSpeed = cs;
        RotationSpeed = rs;
    }


public override void InterpretInput()
    {
        float VerticalAxis = Input.GetAxis("Vertical_Move");

        if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        {
            _player.RigidBody.AddForce(VerticalAxis * _player.transform.up * ClimbingSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }

        float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        if (HorizontalRotation != 0)
        {
            _player.transform.eulerAngles += new Vector3(0, Time.deltaTime * RotationSpeed * HorizontalRotation, 0);
        }
    }

    public override void OnEnter()
    {
        _player.RigidBody.useGravity = false;
    }

    public override void OnExit()
    {
        _player.RigidBody.useGravity = true;
    }
}
