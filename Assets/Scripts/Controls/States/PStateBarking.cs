using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateBarking : PStateGrounded
{
    private const string AnimatorAction = "Barking";
    private const string ClipName = "Dog_Barking";

    private float _time;

    private float _clipLength;

    public PStateBarking(Player player, float acceleration, float ms, float rs) : base(player, acceleration, ms, rs)
    {
        foreach (AnimationClip clip in _player.Animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == ClipName)
            {
                _clipLength = clip.length;
                break;
            }
        }
    }

    public override void InterpretInput()
    {
        base.InterpretInput();
        _time += Time.deltaTime;
        if (_time > _clipLength)
        {
            _player.ChangeState(StateEnum.GROUNDED);
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
