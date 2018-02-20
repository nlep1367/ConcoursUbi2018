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

    public void OnMMLMCreateMatch()
    {
        string matchName = GameObject.Find("NewGameField").GetComponent<Text>().text;

        if (matchName != "")
        {
            singleton.matchMaker.CreateMatch(matchName, 15, true, "", "", "", 0, 0, OnMatchCreate);
        }
    }

    public void OnMMLMJoinMatch(MatchInfoSnapshot matchInfo)
    {
        singleton.matchMaker.JoinMatch(matchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);

        if (ClientScene.localPlayers.Count < maxPlayers)
        {
            OnMMLMRefreshMatches();
            waitingPanel.SetActive(true);
        }
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        Vector3 position = new Vector3(0, 15, 0);
        playerPrefab = Instantiate(spawnPrefabs[conn.connectionId], position, Quaternion.identity);

        return playerPrefab;
    }


    private void OnMMLMRefreshMatches()
    {
        ClearDisplayedMatches();

        //Fills Network Manager matches
        singleton.matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);

        if (singleton.matches.Count > 0)
        {
            DisplayMatchesToList(matches);
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
