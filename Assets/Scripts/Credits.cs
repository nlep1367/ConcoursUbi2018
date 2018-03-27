using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public Ability exit;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Submit"))
        {
            if(exit.DoAction())
                SceneManager.LoadSceneAsync("MainMenu");
        }
	}
}
