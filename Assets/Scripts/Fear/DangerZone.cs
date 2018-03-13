using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour {

    public float fearIncrement = 10f;
    private float nextFearUpdate = 1;

    private void OnTriggerStay(Collider other)
    {
        // If the collider has the fear component (it's the girl)
        if (other.GetComponent<Fear>())
        {
            if (Time.time >= nextFearUpdate)
            {
                Fear fear = other.GetComponent<Fear>();
                fear.IncreaseFear(fearIncrement);
                nextFearUpdate = Time.time + 1;
            }
        }
        
    }
}
