using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Networking.Match;


public class HostGameItem : MonoBehaviour {

    public Button button;
    public Text gameName;

    private MatchInfoSnapshot match;

	// Use this for initialization
	void Start ()
    {
	}

    public void SetUp(MatchInfoSnapshot currentMatch, UnityAction callback)
    {
        match = currentMatch;
        gameName.text = currentMatch.name;

        button.onClick.AddListener(callback);
    }
}
