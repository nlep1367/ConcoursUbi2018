using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

[System.Serializable]
public class Game
{
    public string name;
}


public class HostList : MonoBehaviour {

    public Transform contentPanel;
    public SimpleHostPool hostPool;
    public Text nameField;

    private MatchMakingLobbyManager lobbyMgr;

    // Use this for initialization
    void Start()
    {
        lobbyMgr = new MatchMakingLobbyManager();
        RefreshDisplay();
	}

    public void RefreshDisplay()
    {
        lobbyMgr.MMLMListMatches();
        RemoveGames();
        AddGames();
    }

    private void AddGames()
    {
        foreach(MatchInfoSnapshot g in lobbyMgr.matchesList)
        {
            GameObject newHost = hostPool.GetObject();
            newHost.transform.SetParent(contentPanel);
            newHost.transform.localScale = new Vector3(1, 1, 1);

            HostGameItem hostItem = newHost.GetComponent<HostGameItem>();
            //hostItem.SetUp(g, this);
        }
    }

    public void CreateGame()
    {
        // TODO: Disable input
        lobbyMgr.MMLCreateMatch(nameField.text);

        // TODO: Enter game and leave Main Menu        
    }

    private void RemoveGames()
    {
        while(contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            hostPool.ReturnObject(toRemove);
        }
    }
}
