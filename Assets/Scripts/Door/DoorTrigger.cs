using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
    public HintUI hUI;
    public System.Action<bool> GirlTriggerState;

    public DoorState DoorS;

    public void Start()
    {
        if(DoorS == null)
        {
            Debug.LogError("Veuillez ajouter la référence au DoorState SVP.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fille")
            if(GirlTriggerState != null)
            { 
                GirlTriggerState(true);

                if (other.gameObject.GetComponent<ObjectSync>().hasAuthority)
                {
                    if(DoorS.Locks.Count > 0 && DoorS.IsKeyRelatedToLocks())
                    {
                        GameObject.FindGameObjectWithTag("Fille").GetComponent<PickupObject>().SetForceHideHintUi(true);
                        hUI.Display(Controls.A, "Use Key");
                    }
                    else
                    {
                        hUI.Display(Controls.A, "Open Door");
                    }
                }
            }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fille")
            if (GirlTriggerState != null)
            { 
                GirlTriggerState(false);

                if (other.gameObject.GetComponent<ObjectSync>().hasAuthority)
                {
                    GameObject.FindGameObjectWithTag("Fille").GetComponent<PickupObject>().SetForceHideHintUi(false);
                    hUI.Hide();
                }
            }
    }
}
