using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimuliCreator : MonoBehaviour {

    private float time = 0;
    private AIManager AIManager;

    // Use this for initialization
    void Start () {
        GameObject Manager = GameObject.FindGameObjectWithTag("AI-Manager");
        AIManager = Manager.GetComponent<AIManager>();
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
		if(time > 1.0f)
        {
            AIManager.AddStimuli(3.0f, transform.position, 2.0f);
            time = 0.0f;
        }
	}
}
