using System.Collections.Generic;
using UnityEngine;

public class TriggerMorph32 : MonoBehaviour {
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
        if (StaticInput.GetButtonDown("X") && GameEssentials.IsGirl(other))
        {
            GameEssentials.PlayerGirl.ChangeState(StateEnum.READING);
            foreach (GameObject go in MorphingObjects)
            {
                go.GetComponent<MorphMaterial>().Rpc_SetActive();
            }
            HintUI.Hide();
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
