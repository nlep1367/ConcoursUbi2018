using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct ScoreObj
{
    public int points;
    public string reason;

    public ScoreObj(int p, string s)
    {
        points = p;
        reason = s;
    }

    public override string ToString()
    {
        string message = (points >= 0) ? "+" : "-";
        return message + points.ToString() + " " + reason;
    }
}

public class NetworkScoreManager : NetworkBehaviour {
    public float BarkRangeAdaptModifier = 0.1f;
    public float BarkRadiusAdaptModifier = 0.1f;
    public float TerrorAdaptModifier = 0.1f;


    private void Awake()
    {
        GameEssentials.ScoreManager = this;
    }

    int _gameScore = 0;

    public int GameScore
    {
        get { return _gameScore; }
        set { Adapt(value - _gameScore); _gameScore = value; }
    }

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
        GameScore += score.points;
    }

    public void LosePoints(ScoreObj score)
    {
        int currentScore = GameScore - score.points;

        if (currentScore < 0)
        {
            GameScore = 0;
        }
        else
        {
            GameScore = currentScore;
        }
    }

    private void Adapt(int diff)
    {
        Adaptation.BarkRange += diff * BarkRangeAdaptModifier;
        Adaptation.MaximumBarkRadius += diff * BarkRadiusAdaptModifier;
        Adaptation.TerrorMultiplier += diff * TerrorAdaptModifier;
    }
}
