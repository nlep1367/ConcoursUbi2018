using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public int pointsForCollectible = 0;

    ScoreObj scoreObj;

    private void Start()
    {
        scoreObj = new ScoreObj(pointsForCollectible, "Rare Collectible Discovered.");
    }

    public ScoreObj GetCollectibleScoreObj()
    {
        return scoreObj;
    }
}
