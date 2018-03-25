using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateFalling : PlayerState {
    private const string AnimatorAction = "Falling";
    private const string ClipName = "Dog_Falling";
    private float _fallModifier;

    private float _clipLength;
    private float _height;
    public PStateFalling(Player player, float fallModifier) : base(player)
    {
        _fallModifier = fallModifier;

        foreach (AnimationClip clip in _player.Animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == ClipName)
            {
                _clipLength = clip.length;
                break;
            }
        }

        // TODO : Calcul true height
        _height = 2;
    }
    
    public override void InterpretInput()
    {
        //Gravity tampering
        Vector3 force = Physics.gravity * _fallModifier * Time.deltaTime;
        _player.RigidBody.AddForce(force, ForceMode.VelocityChange);

        RaycastHit hit;
        Physics.Raycast(_player.transform.position, Vector3.down, out hit);

        if (hit.distance < _height)
        {
            _player.ChangeState(StateEnum.LANDING);
        }
    }
    
    public override void OnEnter(object o)
    {
        _player.Animator.SetBool(AnimatorAction, true);

        //Calcul the time left in air
        RaycastHit hit;
        Physics.Raycast(_player.transform.position, Vector3.down, out hit);

        if (hit.distance == 0) return;
        float height = hit.distance;
        float timeLeft = Mathf.Sqrt(Mathf.Abs(2 * height / (Physics.gravity.y * (1+_fallModifier))));
        float speed = _clipLength / timeLeft;
         _player.Animator.SetFloat("Speed", speed);
    }
    
    public override void OnExit()
    {
        _player.Animator.SetBool(AnimatorAction, false);
    }
}