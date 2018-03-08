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

            GetComponent<UpdateEcho>().Cam = cam.GetComponent<Camera>();

            Destroy(this);
        }
	}
}
