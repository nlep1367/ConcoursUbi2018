using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGrabbing : PlayerState
{
    private const string AnimatorAction = "Grabbing";
    private const string ClipNameDog = "Dog_Grabbing";
    private const string ClipNameGirl = "Girl_Grabbing";

    private float _time;

    private float _clipLength;

    public PStateGrabbing(Player player) : base(player)
    {
        string clipName = player is PlayerGirl ? ClipNameGirl : ClipNameDog;

        foreach (AnimationClip clip in _player.Animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                _clipLength = clip.length;
                break;
            }
        }
    }

    public override void InterpretInput()
    {
        _time += Time.deltaTime;
        if(_time > _clipLength)
        {
            _player.ChangeState(StateEnum.GROUNDED);
        }
    }

    public override void OnEnter(object o)
    {
        _player.RigidBody.velocity = Vector3.zero;
        _player.Animator.SetBool(AnimatorAction, true);
        _time = 0;
    }

    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }
}
