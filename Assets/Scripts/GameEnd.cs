using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameEnd : MonoBehaviour {

    public string matchName;

    public Leaderboard leaderboard;
    

    GameEnd ge;

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(gameObject);
    }
    
    public void Update()
    {
        EndGame ge = GameObject.FindObjectOfType<EndGame>();
        if(ge != null && ge.Ended)
        {
            Finish();
            ge.Ended = false;
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
