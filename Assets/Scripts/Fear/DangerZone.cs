using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour {

    public float fearIncrement = 10f;
    public string reasonPointLoss = "";
    public int pointLoss = 10;
    private float nextFearUpdate = 1;

    public bool isStay = true;

    private void OnTriggerEnter(Collider other)
    {
        Fear fear = other.GetComponent<Fear>();
        // If the collider has the fear component (it's the girl)
        if (!isStay && fear != null)
        {
            fear.IncreaseFear(fearIncrement);
            GameEssentials.PlayerGirl.GetComponent<PlayerScoreManager>().Cmd_LosePoints(new ScoreObj(pointLoss * ((fear.fearState >= Fear.FearState.Stress) ? 2 : 1), reasonPointLoss));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Fear fear = other.GetComponent<Fear>();
        // If the collider has the fear component (it's the girl)
        if (isStay && fear != null)
        {
            if (Time.time >= nextFearUpdate)
            {
                fear.IncreaseFear(fearIncrement);

                GameEssentials.PlayerGirl.GetComponent<PlayerScoreManager>().Cmd_LosePoints(new ScoreObj(pointLoss * ((fear.fearState >= Fear.FearState.Stress) ? 2 : 1), reasonPointLoss));

                nextFearUpdate = Time.time + 1;
            }
        }    
    }
}
