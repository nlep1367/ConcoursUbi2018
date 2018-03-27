using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TriggerTalking : MonoBehaviour {
    public int DialogueId;
    public int ObjectiveId;
    public Dialogue Dialogue;
    public Objective Objective;
    public HintUI HintUI;
    public ObjectiveStateEnum ObjectiveState;

    private void Start()
    {
        Objective = GameEssentials.ObjectiveManager.Objectives.Where(objective => objective.Id == Objective.Id).First();
        Dialogue = GameEssentials.DialogueManager.Dialogues.Where(dialogue => dialogue.Id == DialogueId).First();
    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (Input.GetButton("Fire1") && networkBehaviour && networkBehaviour.isLocalPlayer && other.CompareTag(ConstantsHelper.PlayerGirlTag))
        {
            HintUI.Display(Controls.A, "Talk");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        NetworkBehaviour networkBehaviour = other.GetComponentInParent<NetworkBehaviour>();
        if (Input.GetButton("Fire1") && networkBehaviour && networkBehaviour.isLocalPlayer && other.CompareTag(ConstantsHelper.PlayerGirlTag))
        {
            GameEssentials.PlayerGirl.ChangeState(StateEnum.TALKING);
            GameEssentials.Npc.ChangeState(StateEnum.TALKING);

            GameEssentials.DialogueSync.Cmd_ChangeDialogueToServer(Dialogue);
            HintUI.Hide();

            switch (ObjectiveState)
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

    private void OnTriggerExit(Collider other)
    {
        HintUI.Hide();
    }
}
