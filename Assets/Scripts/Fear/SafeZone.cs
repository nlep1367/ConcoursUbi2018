using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour {

    public float fearDecrement = 5f;
    private int nextFearUpdate = 1;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= nextFearUpdate)
        {
            var player = other.GetComponent<Fear>();

            if (player != null)
            {
                player.DecreaseFear(fearDecrement);
                nextFearUpdate = Mathf.FloorToInt(Time.time) + 1;
            }
        }
    }
}
