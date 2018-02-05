using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MatchMakingLobbyManager : NetworkLobbyManager
{
    public List<MatchInfoSnapshot> matchesList = new List<MatchInfoSnapshot>();

    // Use this for initialization
    void Start()
    {
        MMLStart();
        MMLMListMatches();
    }

    public void MMLStart()
    {
        StartMatchMaker();
    }

    public void MMLMListMatches()
    {
        matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);

        if (success)
        {
            matchesList = matchList;
        }
    }

    public void MMLCreateMatch(string matchName)
    {
        matchMaker.CreateMatch(matchName, 15, true, "", "", "", 0, 0, OnMatchCreate);
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);

        if (!success)
        {
            Debug.Log("Failed to create match: " + extendedInfo);
        }
        else
        {
            Debug.Log("Successfully created a match: " + matchInfo.networkId);
        }
    }

    public void MMLJoinMatch(MatchInfoSnapshot matchInfo)
    {
        matchMaker.JoinMatch(matchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchJoined(success, extendedInfo, matchInfo);

        if (!success)
        {
            Debug.Log("Failed to join match: " + extendedInfo);
        }
        else
        {
            Debug.Log("Successfully joined a match: " + matchInfo.networkId);
        }
    }
}
