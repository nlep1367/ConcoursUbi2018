using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateScriptedLadder : PlayerState {
    Vector3 _forceUp = 10f * Vector3.up;
    Vector3 _forceForward = 10f * Vector3.forward;

    float _time = 0.0f;

    const float ForwardTime = 0.3f;
    const float EndTime = 0.6f;

    public PStateScriptedLadder(Player player) : base(player)
    {
    }

    public override void OnEnter()
    {
        _player.RigidBody.velocity = Vector3.zero;
        _time = 0.0f;
        _forceUp = 10f * _player.transform.up;
        _forceForward = 10f * _player.transform.forward;
    }
    public override void InterpretInput()
    {
        _time += Time.deltaTime;
        if (_time < ForwardTime)
        {
            _player.RigidBody.AddForce(_forceUp, ForceMode.Acceleration);
            _player.RigidBody.AddForce(_forceForward, ForceMode.Acceleration);

        }
        else if (_time < EndTime)
        {
            _player.RigidBody.AddForce(_forceForward, ForceMode.Acceleration);

        }
        else
        {
            _player.ChangeState(StateEnum.GROUNDED);
        }
    }
}
