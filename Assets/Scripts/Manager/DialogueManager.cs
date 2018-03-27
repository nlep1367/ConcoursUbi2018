using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueManager : NetworkBehaviour, INotifyPropertyChanged {
    public List<Dialogue> Dialogues;
    private Dialogue _currentDialogue;

    public event PropertyChangedEventHandler PropertyChanged;

    public Dialogue CurrentDialogue
    {
        get { return _currentDialogue; }
        set { _currentDialogue = value; NotifyPropertyChanged("CurrentDialogue"); }
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    void Awake()
    {
        GameEssentials.DialogueManager = this;
        Dialogues = new List<Dialogue> {
            new Dialogue
            {
                Id = 0,
                Speaker = "Narrator",
                Title = "Intro text 0",
                Text = "Once upon a time...",
            },
            new Dialogue
            {
                Id = 1,
                Speaker = "Narrator",
                Title = "Intro text 1",
                Text = "there was a girl...",
            },
            new Dialogue
            {
                Id = 2,
                Speaker = "Narrator",
                Title = "Intro text 2",
                Text = "who couldn't see shit...",
            },
                        new Dialogue
            {
                Id = 3,
                Speaker = "Passerby",
                Title = "NPC dialogue",
                Text = "Did you try finding the keys of the back alley? It's straight across the street. Watch out... I mean look out... I mean... mind the cars, will you?",
            }
        };
    }

    [ClientRpc]
    public void Rpc_ChangeDialogueToServer(Dialogue dialogue)
    {
        CurrentDialogue = dialogue;
    }
}
