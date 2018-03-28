using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TriggerTalking : MonoBehaviour {
    public int DialogueId;
    public int ObjectiveId;
    public Dialogue Dialogue;
    public int[] Objectives;
    public HintUI HintUI;
    public ObjectiveStateEnum ObjectiveState;

    private void Start()
    {
        Dialogue = GameEssentials.DialogueManager.Dialogues.Where(dialogue => dialogue.Id == DialogueId).First();
    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (networkBehaviour && networkBehaviour.isLocalPlayer && other.CompareTag(ConstantsHelper.PlayerGirlTag))
        {
            HintUI.Display(Controls.X, "Talk");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (Input.GetButtonDown("X") && networkBehaviour && networkBehaviour.isLocalPlayer && other.CompareTag(ConstantsHelper.PlayerGirlTag))
        {
            GameEssentials.PlayerGirl.ChangeState(StateEnum.TALKING);
            GameEssentials.Npc.ChangeState(StateEnum.TALKING);

            GameEssentials.DialogueSync.Cmd_ChangeDialogueToServer(Dialogue);
            HintUI.Hide();

            GameEssentials.ObjectiveSync.Cmd_RemoveObjectives();

            foreach (int i in Objectives)
            {
                GameEssentials.ObjectiveSync.Cmd_AddObjectiveToServer(GameEssentials.ObjectiveManager.Objectives.Where(d => d.Id == i).First());
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        HintUI.Hide();
    }
}
