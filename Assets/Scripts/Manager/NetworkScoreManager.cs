using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct ScoreObj
{
    public int points;
    public string reason;

    public override string ToString()
    {
        string message = (points >= 0) ? "+" : "-";
        return message + points.ToString() + " " + reason;
    }
}

public class NetworkScoreManager : NetworkBehaviour {

    int gameScore = 0;
    public System.Action<ScoreObj> displayAddScore;
    public System.Action<ScoreObj> displayLoseScore;

    [ClientRpc]
    public void Rpc_AddPoints(ScoreObj score)
    {
        AddPoints(score);

        if(displayAddScore != null)
            displayAddScore.Invoke(score);
    }

    [ClientRpc]
    public void Rpc_LosePoints(ScoreObj score)
    {
        LosePoints(score);

        if(displayLoseScore != null)
            displayLoseScore.Invoke(score);
    }


    public void AddPoints(ScoreObj score)
    {
        gameScore += score.points;
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
    }
}
