using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class ObjectiveTrigger : NetworkBehaviour {
    public int ObjectiveId;
    public Objective Objective;
    public ObjectiveStateEnum Status;
    private ObjectiveManager _objectiveManager;

    private void Start()
    {
        _objectiveManager = GameEssentials.ObjectiveManager;
        Objective = _objectiveManager.Objectives.Where(obj => obj.Id == ObjectiveId).First();
    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (networkBehaviour && other.gameObject.GetComponent<TriggerFeet>() && networkBehaviour.isLocalPlayer)
        {
            switch (Status)
            {
                case ObjectiveStateEnum.FAIL:
                    GameEssentials.ObjectiveSync.Cmd_FailObjectiveToServer(Objective);

                    break;
                case ObjectiveStateEnum.SUCCESS:
                    GameEssentials.ObjectiveSync.Cmd_CompleteObjectiveToServer(Objective);
                    break;
                case ObjectiveStateEnum.PROGRESS:
                    GameEssentials.ObjectiveSync.Cmd_AddObjectiveToServer(Objective);

                    break;
                default:
                    break;
            }
        }
    }
}
