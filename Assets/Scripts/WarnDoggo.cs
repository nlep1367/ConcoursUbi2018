using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WarnDoggo : NetworkBehaviour {

    public float Cooldown = 10.0f;
    private float _startTime = 0.0f;
    private Dialogue _dialogue;
    private List<Dialogue> _dialogues;

    private void Start()
    {
        _dialogues = new List<Dialogue>();

        _dialogues.Add(new Dialogue
        {
            Title = "Come back",
            Speaker = "Iris",
            Text = "Echo, Come back to me!!!"
        });

        _dialogues.Add(new Dialogue
        {
            Title = "Come back",
            Speaker = "Iris",
            Text = "Help me Echo!!!"
        });

        _dialogues.Add(new Dialogue
        {
            Title = "Come back",
            Speaker = "Iris",
            Text = "*Whistle* Come back!"
        });

        _dialogues.Add(new Dialogue
        {
            Title = "Come back",
            Speaker = "Iris",
            Text = "Come here my boy!!!"
        });

        _dialogues.Add(new Dialogue
        {
            Title = "Come back",
            Speaker = "Iris",
            Text = "To my side!!!"
        });


    }

    [Client]
    // Update is called once per frame
    void Update () {
        if (StaticInput.GetButtonDown("Y") && (Time.time - _startTime) > Cooldown && GameEssentials.PlayerGirl.hasAuthority)
        {
            _startTime = Time.time;
            GameEssentials.DialogueSync.Cmd_ChangeDialogueToServer(_dialogues[Random.Range(0, _dialogues.Count-1)]);
        }
	}
}
