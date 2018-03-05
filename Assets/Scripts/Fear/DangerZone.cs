using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour {

    public float fearIncrement = 10f;
    private float nextFearUpdate = 1;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= nextFearUpdate)
        {
            var player = other.GetComponent<Fear>();

            if (player != null)
            {
                player.IncreaseFear(fearIncrement);
                nextFearUpdate = Time.time + 1;
            }
        }
    }
}
