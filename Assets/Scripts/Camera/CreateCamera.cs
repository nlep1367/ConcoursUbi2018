using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateCamera : NetworkBehaviour {

    public GameObject CameraToInstantiate;

	// Use this for initialization
	void Start () {
		if(isLocalPlayer)
        {
            GameObject cam = GameObject.Instantiate(CameraToInstantiate);
            cam.GetComponent<ThirdPerson>().SetPlayer(this.gameObject.transform);

            Camera camera = cam.GetComponent<Camera>();

            if (this.GetComponent<PlayerGirl>())
            {
                
                UpdateEcho updateEcho = cam.GetComponent<UpdateEcho>();
                updateEcho.Cam = camera;
                updateEcho.PlayerPosRef = this.transform;
            }

            this.GetComponent<Player>().SetCamera(camera);
            Destroy(this);
        }
	}
}
