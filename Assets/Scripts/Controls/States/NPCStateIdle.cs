using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateIdle : PlayerState
{
    private const string AnimatorAction = "Idle";

    public NPCStateIdle(Player player) : base(player)
    {
    }

    public override void InterpretInput()
    {
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
