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

    public Button createButton;
    public Button refreshButton;
    public Text newGameField;

    // Use this for initialization
    void Start()
    {
    }

    public void InitComponents()
    { 
        hostPool = GameObject.Find("HostGamePool").GetComponent<SimpleHostPool>();
        GameObject allOptionsPanel = GameObject.Find("AllOptionsPanel");
        Transform[] children = allOptionsPanel.GetComponentsInChildren<Transform>(true);
        
        for (int i=0; i<children.Length; ++i)
        {
            if (children[i].name == "WaitingPanel") waitingPanel = children[i].gameObject;
        }
        
        content = GameObject.Find("GamesListContent").transform;
        createButton = GameObject.Find("CreateButton").GetComponent<Button>();
        refreshButton = GameObject.Find("RefreshButton").GetComponent<Button>();
        newGameField = GameObject.Find("NewGameField").GetComponent<Text>();

        SetComponents();
    }
    void SetComponents()
    {
        singleton.StartMatchMaker();
        singleton.matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);

        createButton.onClick.RemoveAllListeners();
        createButton.onClick.AddListener(OnMMLMCreateMatch);

        refreshButton.onClick.RemoveAllListeners();
        refreshButton.onClick.AddListener(OnMMLMRefreshMatches);
    }

    public override void OnLobbyClientDisconnect(NetworkConnection conn)
    {
        base.OnLobbyClientDisconnect(conn);
        ExitMatch();
    }
    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        base.OnLobbyServerDisconnect(conn);
        ExitMatch();
    }

    public void ExitMatch()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();

        MatchMakingLobbyManager.singleton.StopClient();
        MatchMakingLobbyManager.singleton.StopServer();

        NetworkServer.DisconnectAll();
        //Network.Disconnect ();

        StartCoroutine(ExitDelay());
    }

    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(0.1f);//attends un peu

        //Fader fader = new Fader();
        //fader.FadeIntoLevel("MainMenu");
        if (SendReturnToLobby())
        {
            InitComponents();
        }
        InitComponents();
    }

    public void OnMMLMCreateMatch()
    {
        string matchName = newGameField.text;

        if (matchName != "")
        {
            singleton.matchMaker.CreateMatch(matchName, 15, true, "", "", "", 0, 0, OnMatchCreate);
        }
    }

    public void OnMMLMJoinMatch(MatchInfoSnapshot matchInfo)
    {
        singleton.matchMaker.JoinMatch(matchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    private void OnMMLMRefreshMatches()
    {
        ClearDisplayedMatches();

        //Fills Network Manager matches
        singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);

        if (singleton.matches.Count > 0)
        {
            DisplayMatchesToList(matches);
        }
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
