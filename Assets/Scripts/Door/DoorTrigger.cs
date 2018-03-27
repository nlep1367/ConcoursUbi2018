using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public System.Action<bool> GirlTriggerState;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fille")
            if(GirlTriggerState != null)
            { 
                GirlTriggerState(true);

            }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fille")
            if (GirlTriggerState != null)
            { 
                GirlTriggerState(false);
            }
    }
}
