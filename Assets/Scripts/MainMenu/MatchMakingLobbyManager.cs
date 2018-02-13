using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MatchMakingLobbyManager : NetworkLobbyManager
{
    public Transform contentPanel;
    public SimpleHostPool hostPool;
    public GameObject waitingPanel;

    // Use this for initialization
    void Start()
    {
        singleton.StartMatchMaker();
        singleton.matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);

        waitingPanel = GameObject.Find("WaitingPanel");
        waitingPanel.SetActive(false);

        GameObject.Find("CreateButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("CreateButton").GetComponent<Button>().onClick.AddListener(OnMMLMCreateMatch);

        GameObject.Find("RefreshButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("RefreshButton").GetComponent<Button>().onClick.AddListener(OnMMLMRefreshMatches);
    }

    public void OnMMLMRefreshMatches()
    {
        ClearDisplayedMatches();

        //Fills Network Manager matches
        singleton.matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);

        if (singleton.matches.Count > 0)
        {
            DisplayMatchesToList(matches);
        }
    }

    //For debug
    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        base.OnMatchList(success, extendedInfo, matchList);

        if (success)
        {
            Debug.Log("Matches listed");
        }
    }

    public void OnMMLMCreateMatch()
    {
        string matchName = GameObject.Find("NewGameField").GetComponent<Text>().text;

        if (matchName != "")
        {
            singleton.matchMaker.CreateMatch(matchName, 15, true, "", "", "", 0, 0, OnMatchCreate);
        }
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);

        if (success)
        {
            Debug.Log("Successfully created a match: " + matchInfo.networkId);
            OnMMLMRefreshMatches();
        }
        else
        {
            Debug.Log("Failed to create match: " + extendedInfo);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);

        if(ClientScene.localPlayers.Count < maxPlayers)
        {
            waitingPanel.SetActive(true);
        }
    }

    public void OnMMLMJoinMatch(MatchInfoSnapshot matchInfo)
    {
        singleton.matchMaker.JoinMatch(matchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    //For debug
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

    private void DisplayMatchesToList(List<MatchInfoSnapshot> matchesList)
    {
        foreach(MatchInfoSnapshot match in matchesList)
        {
            GameObject newHost = hostPool.GetObject();
            newHost.transform.SetParent(contentPanel);
            newHost.transform.localScale = new Vector3(1, 1, 1);

            HostGameItem hostItem = newHost.GetComponent<HostGameItem>();
            hostItem.SetUp(match, delegate { OnMMLMJoinMatch(match); });
        }
    }

    private void ClearDisplayedMatches()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            hostPool.ReturnObject(toRemove);
        }
    }
}
