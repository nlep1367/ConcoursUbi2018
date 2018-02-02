using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Game
{
    public string name;
}


public class HostList : MonoBehaviour {

    public Transform contentPanel;
    public SimpleHostPool hostPool;
    public Text nameField;

    private List<Game> gameList = new List<Game>();

    // Use this for initialization
    void Start()
    {
        RefreshDisplay();
	}

    public void RefreshDisplay()
    {
        RemoveGames();
        AddGames();
    }

    private void AddGames()
    {
        foreach(Game g in gameList)
        {
            GameObject newHost = hostPool.GetObject();
            newHost.transform.SetParent(contentPanel);
            newHost.transform.localScale = new Vector3(1, 1, 1);

            HostGameItem hostItem = newHost.GetComponent<HostGameItem>();
            hostItem.SetUp(g, this);
        }
    }

    public void CreateGame()
    {
        // TODO: Disable input
        Game game = new Game
        {
            name = nameField.text
        };

        AddGame(game);
        RefreshDisplay();        
    }

    private void RemoveGames()
    {
        while(contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            hostPool.ReturnObject(toRemove);
        }
    }

    public void AddGame(Game gameToAdd)
    {
        gameList.Add(gameToAdd);
    }

    private void RemoveGame(Game gameToRemove)
    {
        for (int i = gameList.Count - 1; i <= 0; --i)
        {
            if(gameList[i] == gameToRemove)
            {
                gameList.RemoveAt(i);
            }            
        }
    }
}
