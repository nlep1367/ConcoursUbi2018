using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HostGameItem : MonoBehaviour {

    public Button button;
    public Text gameName;

    private Game game;
    private HostList hostList;

	// Use this for initialization
	void Start () {
        button.onClick.AddListener(HandleClick);
	}
	
	public void SetUp(Game currentGame, HostList currentList)
    {
        game = currentGame;
        gameName.text = game.name;

        hostList = currentList;
    }

    public void HandleClick()
    {
        hostList.AddGame(game);
    }
}
