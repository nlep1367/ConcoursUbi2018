using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGrounded : PlayerState {
    private const string AnimatorAction = "Moving";

    private float _maxSpeed;
    private float _rotationSpeed;
    private float _acceleration;
    private Camera _camera;

    public PStateGrounded(Player player, float acceleration, float ms, float rs) : base(player)
    {
        _acceleration = acceleration;
        _maxSpeed = ms;
        _rotationSpeed = rs;
    }

    public override void InterpretInput()
    {
        MovementMode.ForwardModeCamRelative(_player, _acceleration, _maxSpeed, _rotationSpeed, _camera);
    }

    public override void OnEnter(object o)
    {
        _player.Animator.SetBool(AnimatorAction, true);
    }

    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }
}
