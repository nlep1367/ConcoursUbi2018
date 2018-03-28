using System.Linq;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    public List<int> DialoguesId;

    private void OnTriggerEnter(Collider other)
    {
        if (GameEssentials.IsGirl(other))
        {

            foreach (int i in DialoguesId)
            {
                GameEssentials.DialogueSync.Cmd_ChangeDialogueToServer(GameEssentials.DialogueManager.Dialogues.Where(d => d.Id == i).First());
            }

            Destroy(this);
        }
    }
}
