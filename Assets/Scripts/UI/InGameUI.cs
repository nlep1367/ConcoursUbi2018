using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour {

    public ScaredEffect scaredEffect;
    public ObjectivesUI objectivesUI;
    public ScoreUI scoreUI;
    public HintUI hintUI;
    public StoryUI storyUI;
    public ControlsUI controlsUI;
    public HeartbeatsUI heartbeatsUI;

    public GameObject fille;

    // Cheats:
    List<string> objectives = new List<string>();

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if(fille == null)
        {
            fille = GameObject.FindGameObjectWithTag("Fille");

            if(fille != null)
            {
                if (fille.GetComponent<ObjectSync>().hasAuthority)
                    InitIrisUI();
                else
                    InitEchoUI();
            }
        }
    }

    void Init()
    {
        objectivesUI.transform.GetChild(0).gameObject.SetActive(true);
        scoreUI.transform.GetChild(0).gameObject.SetActive(true);
        hintUI.transform.GetChild(0).gameObject.SetActive(true);
    }

    void InitIrisUI()
    {
        Init();
        controlsUI.DisplayControls(true);
        heartbeatsUI.gameObject.SetActive(true);
        Destroy(scaredEffect.gameObject);
    }

    void InitEchoUI()
    {
        Init();
        controlsUI.DisplayControls(false);
        scaredEffect.transform.GetChild(0).gameObject.SetActive(true);
        Destroy(heartbeatsUI.gameObject);
    }

    
}
