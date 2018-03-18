using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct ScoreObj
{
    public int points;
    public string reason;
}

public class PlayerScoreManager : NetworkBehaviour {

    [SyncVar]
    int gameScore = 0;

    private void Update()
    {
        if(isServer)
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                ScoreObj bob = new ScoreObj();
                bob.points = 10000;
                bob.reason = "Unity sucks";

                Rpc_AddPoints(bob);
            }
            else if(Input.GetKeyDown(KeyCode.I))
            {
                ScoreObj bob = new ScoreObj();
                bob.points = 10000;
                bob.reason = "Unity sucks";

                Rpc_LosePoints(bob);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                ScoreObj bob = new ScoreObj();
                bob.points = 10;
                bob.reason = "Unity sucks";

                Cmd_AddPoints(bob);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                ScoreObj bob = new ScoreObj();
                bob.points = 10;
                bob.reason = "Unity sucks";

                Cmd_LosePoints(bob);
            }
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 60, 200, 25), gameScore.ToString());
    }

    [Command]
    public void Cmd_AddPoints(ScoreObj score)
    {
        AddPoints(score);
    }

    [Command]
    public void Cmd_LosePoints(ScoreObj score)
    {
        LosePoints(score);
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
