using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateMusicPlayer : NetworkBehaviour {

	public GameObject MusicPlayerToInstantiate;

	// Use this for initialization
	void Start () {
		if(isLocalPlayer)
		{
			GameObject musicPlayer = GameObject.Instantiate(MusicPlayerToInstantiate);
			musicPlayer.transform.parent = this.transform;
			Destroy(this);
		}
	}
}
