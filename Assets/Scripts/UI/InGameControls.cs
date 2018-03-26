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
        if (Input.GetButtonDown("PauseMenu"))
        {
            isDisplayed = !isDisplayed;
            menu.SetActive(isDisplayed);
			musicPlayer.SetMenuMusicActive (isDisplayed);
        }
	}
}
