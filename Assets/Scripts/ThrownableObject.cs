using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThrownableObject : NetworkBehaviour {

    public bool IsInThrownZone = false;

    public void ThrowAway()
    {
        GameEssentials.ObjectiveSync.Cmd_CompleteObjectiveToServer(GameEssentials.ObjectiveManager.Objectives[3]);
        gameObject.GetComponent<ObjectSync>().Rpc_Destroy();
    }
}
