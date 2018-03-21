using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateGroundedGirl : PStateGrounded {
    public PStateGroundedGirl(Player player,float acceleration, float ms, float rs) : base(player, acceleration, ms, rs)
    {
    }

    public override void InterpretInput()
    {
        base.InterpretInput();
        _player.Animator.SetFloat("Speed", _player.RigidBody.velocity.magnitude);

    }
}
