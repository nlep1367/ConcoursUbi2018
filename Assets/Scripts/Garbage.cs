using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        ThrownableObject thrownable = other.GetComponent<ThrownableObject>();

        if (thrownable != null)
        {
            thrownable.IsInThrownZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ThrownableObject thrownable = other.GetComponent<ThrownableObject>();

        if (thrownable != null)
        {
            thrownable.IsInThrownZone = false;
        }
    }
}
