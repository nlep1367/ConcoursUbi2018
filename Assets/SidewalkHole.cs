using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewalkHole : MonoBehaviour {
    public int Points = 5;

    void Awake()
    {
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fille")
        {
            PlayerScoreManager psm = other.GetComponent<PlayerScoreManager>();

            if (psm)
                psm.Cmd_LosePoints(new ScoreObj(Points, "Hole"));
        }
    }
}
