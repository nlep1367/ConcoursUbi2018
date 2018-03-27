using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour {

    
    public Dictionary<string, int> scores;

	// Use this for initialization
	void Start () {
        GetAllScores();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void GetAllScores()
    {
        // https://unity3d.com/learn/tutorials/topics/scripting/loading-game-data-json
    }

    void AddScore()
    {

    }

}
