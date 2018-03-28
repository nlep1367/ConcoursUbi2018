using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class ObjectiveTrigger : NetworkBehaviour
{
    public int[] ObjectivesId;
    public ObjectiveStateEnum state;

    private void OnTriggerEnter(Collider other)
    {
        ObjectiveManager _objectiveManager = GameEssentials.ObjectiveManager;

        if(GameEssentials.IsGirl(other))
        {
            GameEssentials.ApplyObjectives(ObjectivesId, state);

            Destroy(this);
        }
    }
}
