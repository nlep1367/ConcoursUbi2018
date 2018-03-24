using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message
{
    public string name;
    public string content;
    private const float DURATION = 0.75f; 

    public Message(string n, string c)
    {
        name = n;
        content = c;
    }
    public float GetDuration()
    {
        int wordCount = content.Split(' ').Length;
        return wordCount * DURATION;
    }
};

public class StoryUI : MonoBehaviour {

    public GameObject StoryPanel;

    public Text characterName;
    public Text dialog;

    Queue<Message> messages;
    float duration = 0;
    float currentTime = 0;
    bool isShowing = false;

	// Use this for initialization
	void Start () {
        messages = new Queue<Message>();
        GameEssentials.DialogueManager.PropertyChanged += DialogueManager_PropertyChanged;
	}

    private void DialogueManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        Dialogue d = GameEssentials.DialogueManager.CurrentDialogue;
        AddMessage(d.Speaker, d.Text);
    }

    // Update is called once per frame
    void Update () {
		if(messages.Count > 0)
        {            
            currentTime += Time.deltaTime;
            if (!isShowing)
            {
                currentTime = 0;
                isShowing = true;

                // Display message
                Message m = messages.Peek();
                duration = m.GetDuration();

                characterName.text = m.name;
                dialog.text = m.content;

                if (!StoryPanel.activeSelf)
                {
                    StoryPanel.SetActive(true);
                }
            }
            else if(currentTime > duration)
            {
                isShowing = false;
                messages.Dequeue();
            }
        }
        else if(StoryPanel.activeSelf)
        {
            StoryPanel.SetActive(false);
        }
	}

    public void AddMessage(string name, string content)
    {
        messages.Enqueue(new Message(name, content));
    }

    public void AddMessages(List<Message> mess)
    {
        foreach(Message m in mess)
        {
            messages.Enqueue(m);
        }
    }





}
