using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateJumping : PlayerState {
    private const string AnimatorAction = "Jumping";
    private const string ClipName = "Dog_Jumping";

    private float _movementSpeed;
    private float _rotationSpeed;

    private float _lowJumpModifier;

    private float _clipLength;

    public PStateJumping(Player player, float ms, float rs, float lowJumpModifier) : base(player)
    {
        _movementSpeed = ms;
        _rotationSpeed = rs;

        _lowJumpModifier = lowJumpModifier;

        
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
        //// In air movement
        //float HorizontalAxis = Input.GetAxis("Horizontal_Move");

        //if (HorizontalAxis >= float.Epsilon || HorizontalAxis <= -float.Epsilon)
        //{
        //    _player.RigidBody.AddForce(HorizontalAxis * _player.transform.right * _movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        //}

        //float VerticalAxis = Input.GetAxis("Vertical_Move");

        //if (VerticalAxis >= float.Epsilon || VerticalAxis <= -float.Epsilon)
        //{
        //    _player.RigidBody.AddForce(VerticalAxis * _player.transform.forward * _movementSpeed * Time.deltaTime, ForceMode.VelocityChange);
        //}

        //float HorizontalRotation = Input.GetAxis("Horizontal_Rotation");

        //if (HorizontalRotation != 0)
        //{
        //    _player.transform.eulerAngles += new Vector3(0, Time.deltaTime * _rotationSpeed * HorizontalRotation, 0);
        //}

        if (_player.RigidBody.velocity.y < 0)
        {
            _player.ChangeState(StateEnum.FALLING);
        }


        float timeLeft;

        //Gravity tampering
        if (_player.RigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            Vector3 force = Vector3.up * Physics.gravity.y * _lowJumpModifier * Time.deltaTime;
            _player.RigidBody.AddForce(force, ForceMode.VelocityChange);

            //Calcul timeLeft before end of animation (before the jump reaches the peek)
            timeLeft = Mathf.Max(-_player.RigidBody.velocity.y / (Physics.gravity.y * (1+_lowJumpModifier)), 0f);
            
            _player.Animator.SetFloat("Speed", 1f / _clipLength / timeLeft);
        }
        else
        {
            float time = Mathf.Max(-_player.RigidBody.velocity.y / (Physics.gravity.y), 0);

            timeLeft = Mathf.Max(-_player.RigidBody.velocity.y / (Physics.gravity.y), 0f);
        }

        //Adjust animation speed to match jump peek
        if (timeLeft != 0f)
        {
            _player.Animator.SetFloat("Speed", _clipLength / timeLeft);
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
