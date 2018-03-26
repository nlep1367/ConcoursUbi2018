﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectiveSync : NetworkBehaviour
{
    void Awake()
    {
        GameEssentials.ObjectiveSync = this;
    }

    [Command]
    public void Cmd_AddObjectiveToServer(Objective objective)
    {
        GameEssentials.ObjectiveManager.Rpc_AddObjectiveToServer(objective);
    }

    [Command]
    public void Cmd_CompleteObjectiveToServer(Objective objective)
    {
        GameEssentials.ObjectiveManager.Rpc_CompleteObjectiveToServer(objective);
    }

    [Command]
    public void Cmd_FailObjectiveToServer(Objective objective)
    {
        GameEssentials.ObjectiveManager.Rpc_FailObjectiveToServer(objective);
    }
}