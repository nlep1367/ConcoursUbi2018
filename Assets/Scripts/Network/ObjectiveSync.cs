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
    public void Cmd_AddObjectiveToServer(Objective objectives)
    {
        GameEssentials.ObjectiveManager.Rpc_AddObjectiveToServer(objectives);
    }

    [Command]
    public void Cmd_RemoveObjectives()
    {
        GameEssentials.ObjectiveManager.Rpc_RemoveObjectives();
    }
    /*
    [Command]
    public void Cmd_FailObjectiveToServer(Objective objective)
    {
        GameEssentials.ObjectiveManager.Rpc_FailObjectiveToServer(objective);
    }*/
}