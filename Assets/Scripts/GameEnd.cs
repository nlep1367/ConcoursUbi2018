using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour {

    public string matchName;

    public Leaderboard leaderboard;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}

    public void Finish()
    {
        leaderboard.SavePlayerProgress(matchName);
    }




}
