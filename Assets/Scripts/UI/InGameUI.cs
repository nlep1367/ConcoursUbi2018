using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour {

    public ObjectivesUI objectivesUI;
    public ScoreUI scoreUI;
    public HintUI hintUI;
    public StoryUI storyUI;
    public ControlsUI controlsUI;

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

            if(fille != null && fille.GetComponent<ObjectSync>().hasAuthority)
            {
                Destroy(GetComponent<Transform>().Find("ScaredEffect").gameObject);
            }
        }

        //Cheats to test UI
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            string s = (Random.Range(0f, 1f) < 0.5f) ? "Get a GF" : " This is a long objective! Name that should use the long prefab";
            objectives.Add(s);
            UpdateObjective(objectives);
        }
        /*else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //AddScore(100, "Good boi");
        }*/
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DisplayHint(Controls.B, "Interact B");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HideHint();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddMessage("Georgio", "Abedelapoupi! VANDETTA VANDETTA!");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ToggleControls();
        }
    }

    public void UpdateObjective(List<string> messages)
    {
        objectivesUI.UpdateObjectives(messages);
    }

    /*public void AddScore(int points, string message)
    {
        //scoreUI.AddScore(Random.Range(0,200), "new score");
    }*/

    public void DisplayHint(Controls button, string action)
    {
        hintUI.Display(button, action);
    }
    public void HideHint()
    {
        hintUI.Hide();
    }

    public void AddMessage(string name, string message)
    {
        storyUI.AddMessage(name, message);
    }
    public void AddMessages(List<Message> messages)
    {
        storyUI.AddMessages(messages);
    }

    public void ToggleControls()
    {
        controlsUI.ToggleControls(); 
    }
}
