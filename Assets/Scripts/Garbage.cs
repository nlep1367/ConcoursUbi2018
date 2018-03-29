using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        PickupObject po = other.GetComponent<PickupObject>();

        if (other.CompareTag("Fille") && po != null)
        {
            GameObject go = po.GetCarriedObject();
            if(go != null)
            {
                ThrownableObject to = go.GetComponent<ThrownableObject>();
                if (to != null)
                    to.IsInThrownZone = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupObject po = other.GetComponent<PickupObject>();

        if (other.CompareTag("Fille") && po != null)
        {
            GameObject go = po.GetCarriedObject();
            if (go != null)
            {
                ThrownableObject to = go.GetComponent<ThrownableObject>();
                if (to != null)
                    to.IsInThrownZone = false;
            }
        }
    }
}
