using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TriggerTalking : MonoBehaviour {
    public GameObject NpcGo;

    public List<int> DialoguesId;
    public List<int> ObjectivesId;
    public HintUI HintUI;
    public ObjectiveStateEnum ObjectiveState;

    private NPC _npc;
    private void Start()
    {
        _npc = NpcGo.GetComponent<NPC>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameEssentials.IsGirl(other))
        {
            HintUI.Display(Controls.X, "Talk");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("X") && GameEssentials.IsGirl(other))
        {
            GameEssentials.PlayerGirl.ChangeState(StateEnum.TALKING);
            _npc.ChangeState(StateEnum.TALKING);

            HintUI.Hide();

            //GameEssentials.ObjectiveSync.Cmd_RemoveObjectives();

            foreach (int i in DialoguesId)
            {
                GameEssentials.DialogueSync.Cmd_ChangeDialogueToServer(GameEssentials.DialogueManager.Dialogues.Where(dialogue => dialogue.Id == i).First());
            }

            GameEssentials.ApplyObjectives(ObjectivesId, ObjectiveState);
            Destroy(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameEssentials.IsGirl(other))
        {
            HintUI.Hide();
        }
    }
}
