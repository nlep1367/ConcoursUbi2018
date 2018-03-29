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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Finish();
        }
    }

    public void Finish()
    {
        FindObjectOfType<InGameUI>().endStoryboard.gameObject.SetActive(true);
        leaderboard.SavePlayerProgress(matchName);

    }

    public void ExitToLoby()
    {

    }




}
