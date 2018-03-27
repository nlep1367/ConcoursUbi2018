using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Player {
    new private void Awake()
    {
        base.Awake();
        GameEssentials.Npc = this;
        State = new NPCStateIdle(this);
        PreviousState = State;

        States = new Dictionary<StateEnum, PlayerState>
        {
            { StateEnum.GROUNDED, State},
            { StateEnum.TALKING, new PStateTalking(this) }
        };
    }


    public override void SetCamera(Camera camera)
    {

    }
}
