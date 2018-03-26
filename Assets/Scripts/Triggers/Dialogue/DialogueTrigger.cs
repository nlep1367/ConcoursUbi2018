using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueTrigger : MonoBehaviour {
    public int DialogueId;
    public Dialogue Dialogue;
    private DialogueManager _dialogueManager;

    private void Start()
    {
        _dialogueManager = GameEssentials.DialogueManager;
        Dialogue = _dialogueManager.Dialogues.Where(dialogue => dialogue.Id == DialogueId).First();
    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (networkBehaviour && networkBehaviour.isLocalPlayer && other.gameObject.GetComponent<TriggerFeet>())
        {
            GameEssentials.DialogueSync.Cmd_ChangeDialogueToServer(Dialogue);
            Destroy(this);
        }
    }
}
