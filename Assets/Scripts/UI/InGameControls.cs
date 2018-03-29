using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameControls : MonoBehaviour {

    public GameObject menu;
	public AmbientMusicControl musicPlayer;

    bool isDisplayed = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (StaticInput.GetButtonDown("PauseMenu") && GameEssentials.MusicPlayer != null)
        {
            isDisplayed = !isDisplayed;
            menu.SetActive(isDisplayed);
			GameEssentials.MusicPlayer.SetMenuMusicActive (isDisplayed);
        }
	}
}
