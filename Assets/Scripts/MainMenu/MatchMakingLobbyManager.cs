using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class MatchMakingLobbyManager : NetworkLobbyManager
{
    public SimpleHostPool hostPool;
    public GameObject waitingPanel;
    public Transform content;

    public GameObject createButton;
    public GameObject refreshButton;
    public GameObject newGameField;

    // Use this for initialization
    void Start()
    {
        singleton.StartMatchMaker();
        singleton.matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);

        createButton.GetComponent<Button>().onClick.RemoveAllListeners();
        createButton.GetComponent<Button>().onClick.AddListener(OnMMLMCreateMatch);

        refreshButton.GetComponent<Button>().onClick.RemoveAllListeners();
        refreshButton.GetComponent<Button>().onClick.AddListener(OnMMLMRefreshMatches);
    }

    public void OnMMLMCreateMatch()
    {
        string matchName = newGameField.GetComponent<Text>().text;

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
        //Vector3 position = new Vector3(0, 15, 0);
        playerPrefab = Instantiate(spawnPrefabs[conn.connectionId], GetStartPosition().position, Quaternion.identity);

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
            newHost.transform.SetParent(content);
            newHost.transform.localScale = new Vector3(1, 1, 1);

            newHost.transform.localPosition = new Vector3(0f, 0f, 0f);
            HostGameItem hostItem = newHost.GetComponent<HostGameItem>();
            hostItem.SetUp(match, delegate { OnMMLMJoinMatch(match); });
        }
    }

    private void ClearDisplayedMatches()
    {
        while (content.childCount > 0)
        {
            GameObject toRemove = content.GetChild(0).gameObject;
            hostPool.ReturnObject(toRemove);
        }
    }
}
