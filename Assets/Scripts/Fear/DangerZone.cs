using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour {

    public float fearIncrement = 10f;
    private float nextFearUpdate = 1;

    private void OnTriggerStay(Collider other)
    {
        Fear fear = other.GetComponent<Fear>();
        // If the collider has the fear component (it's the girl)
        if (fear != null)
        {
            if (Time.time >= nextFearUpdate)
            {
                fear.IncreaseFear(fearIncrement);
                nextFearUpdate = Time.time + 1;
            }
        }
        
    }
}
