using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutPanelGUI : GUIControls {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
        }
    }
}
