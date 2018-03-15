using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateCamera : NetworkBehaviour {

    public GameObject CameraToInstanciate;

	// Use this for initialization
	void Start () {
		if(isLocalPlayer)
        {
            GameObject cam = GameObject.Instantiate(CameraToInstanciate);
            cam.GetComponent<ThirdPerson>().SetPlayer(this.gameObject.transform);

            Camera camera = cam.GetComponent<Camera>();

            if (this.GetComponent<PlayerGirl>())
            {
                this.GetComponent<UpdateEcho>().Cam = camera;
            }

            this.GetComponent<Player>().Camera = camera;
            Destroy(this);
        }
	}
}
