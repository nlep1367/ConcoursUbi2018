using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        PickupObject po = other.GetComponent<PickupObject>();

        if (po != null)
        {
            po.GetCarriedObject().GetComponent<ThrownableObject>().IsInThrownZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupObject po = other.GetComponent<PickupObject>();

        if (po != null)
        {
            po.GetCarriedObject().GetComponent<ThrownableObject>().IsInThrownZone = false;
        }
    }
}
