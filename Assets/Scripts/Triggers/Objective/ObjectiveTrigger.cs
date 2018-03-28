using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class ObjectiveTrigger : NetworkBehaviour
{
    public int[] ObjectiveId;
    public Objective Objective;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectiveManager _objectiveManager = GameEssentials.ObjectiveManager;

        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (networkBehaviour && networkBehaviour.isLocalPlayer && other.gameObject.GetComponent<TriggerFeet>())
        {
            GameEssentials.ObjectiveSync.Cmd_RemoveObjectives();
            
            foreach (int i in ObjectiveId)
            {
                GameEssentials.ObjectiveSync.Cmd_AddObjectiveToServer(_objectiveManager.Objectives.Where(d => d.Id == i).First());
            }

            Destroy(this);
        }
    }
}
