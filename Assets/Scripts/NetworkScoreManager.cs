using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct ScoreObj
{
    public int points;
    public string reason;
}

public class NetworkScoreManager : NetworkBehaviour {

    int gameScore = 0;

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 60, 200, 25), gameScore.ToString());
    }

    [ClientRpc]
    public void Rpc_AddPoints(ScoreObj score)
    {
        AddPoints(score);
    }

    [ClientRpc]
    public void Rpc_LosePoints(ScoreObj score)
    {
        LosePoints(score);
    }


    public void AddPoints(ScoreObj score)
    {
        gameScore += score.points;

        //do someting with the reason
    }

    public void LosePoints(ScoreObj score)
    {
        int currentScore = gameScore - score.points;

        if (currentScore < 0)
        {
            gameScore = 0;
        }
        else
        {
            gameScore = currentScore;
        }

        //do something with the reason
    }
}
