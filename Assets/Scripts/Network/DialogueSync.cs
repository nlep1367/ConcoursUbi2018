using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueSync : NetworkBehaviour {
    void Awake()
    {
        GameEssentials.DialogueSync = this;
    }

    [Command]
    public void Cmd_ChangeDialogueToServer(Dialogue dialogue)
    {
        GameEssentials.DialogueManager.Rpc_ChangeDialogueToServer(dialogue);
    }
}
