using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateLanding : PStateGrounded { 
    private const string AnimatorAction = "Landing";
    private const string ClipName = "Dog_Landing";

    private float _landingTime;

    private float _time;
    private float _clipLength;

    public PStateLanding(Player player, float acceleration, float ms, float rs, float landingTime) : base(player, acceleration, ms, rs)
    {
        _landingTime = landingTime;

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

        if(_time > _landingTime)
        {
            _player.ChangeState(StateEnum.GROUNDED);
        }
    }

    public override void OnEnter(object o)
    {
        _player.Animator.SetBool(AnimatorAction, true);
        _time = 0f;
        //Calcul the time left in air

        float speed = _clipLength / _landingTime;
        _player.Animator.SetFloat("Speed", speed);
    }

    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }
}
