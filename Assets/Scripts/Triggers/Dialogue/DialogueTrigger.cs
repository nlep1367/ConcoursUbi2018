﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueTrigger : MonoBehaviour {
    public List<int> DialoguesId;

    private void OnTriggerEnter(Collider other)
    {
        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (networkBehaviour && networkBehaviour.isLocalPlayer && other.tag == "Fille")
        {

            foreach (int i in DialoguesId)
            {
                GameEssentials.DialogueSync.Cmd_ChangeDialogueToServer(GameEssentials.DialogueManager.Dialogues.Where(d => d.Id == i).First());
            }

            Destroy(this);
        }
    }
}
