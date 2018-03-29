using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardUI : MonoBehaviour {

    Leaderboard leaderboard;
    bool hasDisplayed = false;

    public Transform content;
    public Ability exit;

    // Use this for initialization
    void Start () {
        leaderboard = GameObject.FindObjectOfType<Leaderboard>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasDisplayed)
        {
            hasDisplayed = true;
            leaderboard.Display(content);
        }

        if (StaticInput.GetButtonDown("Submit"))
        {
            if (exit.DoAction())
                SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
