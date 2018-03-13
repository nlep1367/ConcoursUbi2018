using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateFalling : PlayerState {
    private const string AnimatorAction = "Falling";
    private const string ClipName = "Dog_Falling";
    private float _fallModifier;

    private float _clipLength;

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
    }
    
    public override void InterpretInput()
    {
        //Gravity tampering
        Vector3 force = Vector3.up * Physics.gravity.y * _fallModifier * Time.deltaTime;
        _player.RigidBody.AddForce(force, ForceMode.VelocityChange);
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