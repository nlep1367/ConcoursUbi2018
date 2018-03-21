﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectiveSync : NetworkBehaviour
{
    public enum Instruction { ADD, FAIL, COMPLETE }

    private ObjectiveManager _objectiveManager;

    void Awake()
    {
        GameEssentials.ObjectiveSync = this;
    }

    void Start()
    {
        _objectiveManager = GameEssentials.ObjectiveManager;
        _objectiveManager.CurrentObjectives.Callback = OnObjectivesChanged;
    }

    private void OnObjectivesChanged(SyncListObjectives.Operation op, int index)
    {
    }

    [Command]
    public void Cmd_AddObjectiveToServer(Objective objective)
    {
        _objectiveManager.Rpc_AddObjectiveToServer(objective);
    }

    [Command]
    public void Cmd_CompleteObjectiveToServer(Objective objective)
    {
        _objectiveManager.Rpc_CompleteObjectiveToServer(objective);
    }

    [Command]
    public void Cmd_FailObjectiveToServer(Objective objective)
    {
        _objectiveManager.Rpc_FailObjectiveToServer(objective);
    }
}