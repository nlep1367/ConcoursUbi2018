using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TriggerRead : NetworkBehaviour {
    public List<GameObject> MorphingObjects;
    public HintUI HintUI;

    private void OnTriggerEnter(Collider other)
    {
        if (GameEssentials.IsGirl(other))
        {
            HintUI.Display(Controls.X, "Read");
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (GameEssentials.IsGirl(other))
        {
            GameEssentials.PlayerGirl.ChangeState(StateEnum.READING);
            foreach (GameObject go in MorphingObjects)
            {
                Rpc_SetMorphActiveToServer(go);
            }
            HintUI.Hide();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameEssentials.IsGirl(other))
        {
            HintUI.Hide();
        }
    }

    [ClientRpc]
    public void Rpc_SetMorphActiveToServer(GameObject go)
    {
        go.GetComponent<MorphMaterial>().IsActive = true;
    }
}
