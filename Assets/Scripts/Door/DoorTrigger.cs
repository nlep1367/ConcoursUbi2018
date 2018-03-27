using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
    public HintUI hUI;
    public System.Action<bool> GirlTriggerState;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fille")
            if(GirlTriggerState != null)
            { 
                GirlTriggerState(true);

                if (other.gameObject.GetComponent<ObjectSync>().hasAuthority)
                {
                    hUI.Display(Controls.A, "Open Door");
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
                    hUI.Hide();
                }
            }
    }
}
