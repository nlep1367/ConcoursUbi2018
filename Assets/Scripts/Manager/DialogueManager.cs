using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;


public class DialogueManager : NetworkBehaviour, INotifyPropertyChanged
{
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
                    Speaker = "Iris",
                    Title = "Intro0",
                    Text = "Let's see if we can get along...",
                },
                new Dialogue
                {
                    Id = 1,
                    Speaker = "Iris",
                    Title = "Intro1",
                    Text = "How do you feel about food, Echo?",
                },
                new Dialogue
                {
                    Id = 2,
                    Speaker = "Iris",
                    Title = "Intro3",
                    Text = "Lets go to the cafe.",
                },
                new Dialogue
                {
                    Id = 3,
                    Speaker = "Iris",
                    Title = "Intro5",
                    Text = "I think we'll get along just fine.",
                },
                new Dialogue
                {
                    Id = 4,
                    Speaker = "Iris",
                    Title = "Intro6",
                    Text = "Now, to get out of this parking lot...",
                },
                new Dialogue
                {
                    Id = 5,
                    Speaker = "Iris",
                    Title = "SquirrelEncounter0",
                    Text = "(SCARED) MAKE IT GO AWAY!",
                },
                new Dialogue
                {
                    Id = 6,
                    Speaker = "Iris",
                    Title = "SquirrelEncounter1",
                    Text = "Good boy!",
                },
                new Dialogue
                {
                    Id = 7,
                    Speaker = "Iris",
                    Title = "TutorialEnd0",
                    Text = "Finaly out. Where to next?",
                },
                new Dialogue
                {
                    Id = 8,
                    Speaker = "Iris",
                    Title = "StreetFail0",
                    Text = "Wow! Can't believe we made it alive. Maybe use the crosswalk button next time.",
                },
                new Dialogue
                {
                    Id = 9,
                    Speaker = "Iris",
                    Title = "NPCDialogue0",
                    Text = "Excuse me, would you know where the \"Coffee Bark\" is?",
                },
                new Dialogue
                {
                    Id = 10,
                    Speaker = "Iris",
                    Title = "NPCDialogue2",
                    Text = "I'm blind.",
                },
                new Dialogue
                {
                    Id = 11,
                    Speaker = "Iris",
                    Title = "LockDoor0",
                    Text = "Who puts TWO lockes on the only way out of an alley??? I bet it was the squirrels.",
                },


                new Dialogue
                {
                    Id = 12,
                    Speaker = "Iris",
                    Title = "SquirrelEncounter3",
                    Text = "Just how many squirrels are there in this town?",
                },
                new Dialogue
                {
                    Id = 13,
                    Speaker = "Iris",
                    Title = "StreetFail1",
                    Text = "Wow! We're not getting better at this...",
                },


                new Dialogue
                {
                    Id = 14,
                    Speaker = "Iris",
                    Title = "NPCDialogue4",
                    Text = "Hello stranger, do you know where I might find the dog cafe?",
                },
                new Dialogue
                {
                    Id = 15,
                    Speaker = "Iris",
                    Title = "NPCDialogue7",
                    Text = "I'm the best at looking out.",
                },
                new Dialogue
                {
                    Id = 16,
                    Speaker = "Iris",
                    Title = "LockedDoor1",
                    Text = "Awww, it's blocked! There must be a way to get you on this dumpster.",
                },
                new Dialogue
                {
                    Id = 17,
                    Speaker = "Iris",
                    Title = "SquirrelEncounter5",
                    Text = "WHY?!?",
                },
                new Dialogue
                {
                    Id = 18,
                    Speaker = "Iris",
                    Title = "Market0",
                    Text = "Mmmhh! I can already smell the croissants!",
                },
                new Dialogue
                {
                    Id = 19,
                    Speaker = "Iris",
                    Title = "Read0",
                    Text = "I can't expect you to read the signs. Gothing they wrote it in braille.",
                },
                new Dialogue
                {
                    Id = 20,
                    Speaker = "Iris",
                    Title = "MarketFail0",
                    Text = "Wow, we really need to work on our teamwork. I'm surprised I still have my legs.",
                },
                new Dialogue
                {
                    Id = 21,
                    Speaker = "Iris",
                    Title = "MarketMid0",
                    Text = "Well, the town isn't burning or anything and we made it. I still think we could have done better.",
                },
                new Dialogue
                {
                    Id = 22,
                    Speaker = "Iris",
                    Title = "MarketGood0",
                    Text = "It almost feels like you're another person. I don't see how it could have gone better! Good boy!",
                },
                new Dialogue
                {
                    Id = 23,
                    Speaker = "Echo",
                    Title = "Intro2",
                    Text = "*Barks*",
                },
                new Dialogue
                {
                    Id = 24,
                    Speaker = "Echo",
                    Title = "Intro4",
                    Text = "*Barks twice*",
                },
                new Dialogue
                {
                    Id = 25,
                    Speaker = "Echo",
                    Title = "SquirrelEncounter2",
                    Text = "(HAPPY)*Barks*",
                },
                new Dialogue
                {
                    Id = 26,
                    Speaker = "Echo",
                    Title = "SquirrelEncounter4",
                    Text = "*Barks*",
                },
                new Dialogue
                {
                    Id = 27,
                    Speaker = "Echo",
                    Title = "StreetFail1",
                    Text = "(ASHAMED)*Barks*",
                },
                new Dialogue
                {
                    Id = 28,
                    Speaker = "Echo",
                    Title = "StreetSuccess1",
                    Text = "*(HAPPY)*Barks**",
                },
                new Dialogue
                {
                    Id = 29,
                    Speaker = "Echo",
                    Title = "Market1",
                    Text = "*(HAPPY)Barks*",
                },
                new Dialogue
                {
                    Id = 30,
                    Speaker = "Echo",
                    Title = "MarketFail1",
                    Text = "*(SAD)Barks*",
                },
                new Dialogue
                {
                    Id = 31,
                    Speaker = "Echo",
                    Title = "MarketMid1",
                    Text = "*Barks*",
                },
                new Dialogue
                {
                    Id = 32,
                    Speaker = "Echo",
                    Title = "MarketGood1",
                    Text = "*(HAPPY)Barks twice*",
                },
                new Dialogue
                {
                    Id = 33,
                    Speaker = "Passerby",
                    Title = "NPCDialogue1",
                    Text = "Can't you look it up?",
                },
                new Dialogue
                {
                    Id = 34,
                    Speaker = "Passerby",
                    Title = "NPCDialogue3",
                    Text = "Oh. OH! Well.. uh... you need to go through the alley. The streets are blocked.",
                },
                new Dialogue
                {
                    Id = 35,
                    Speaker = "Passerby",
                    Title = "NPCDialogue5",
                    Text = "It's on the other side of this barrier. I would give you the my key, but I lost it. Darn squirrels took it away.",
                },
                new Dialogue
                {
                    Id = 36,
                    Speaker = "Passerby",
                    Title = "NPCDialogue6",
                    Text = "Also, look out; I think a tree fell down.",
                },
                new Dialogue
                {
                    Id = 37,
                    Speaker = "Narrator",
                    Title = "End",
                    Text = "They ate croissants and lived happily ever after.",
                },
                new Dialogue
                {
                    Id = 38,
                    Speaker = "Iris",
                    Title = "End",
                    Text = "Look out world, Iris and Echo are not to be messed with!",
                },
                new Dialogue
                {
                    Id = 39,
                    Speaker = "Iris",
                    Title = "End",
                    Text = " No seriously, look out, because I certainly can't.",
                },
                new Dialogue
                {
                    Id = 40,
                    Speaker = "Echo",
                    Title = "End",
                    Text = "(Annoyed) Bark…",
                },
                new Dialogue
                {
                    Id = 41,
                    Speaker = "Iris",
                    Title = "End",
                    Text = "You don't see the beauty of these puns. Well. I don't either.",
                },
                new Dialogue
                {
                    Id = 42,
                    Speaker = "Iris",
                    Title = "End",
                    Text = "Psssht. Who needs Google Maps when you've got Echo Maps. Good boi!",
                },
                new Dialogue
                {
                    Id = 43,
                    Speaker = "Iris",
                    Title = "End",
                    Text = "How is there so much to do in such a small street?",
                }
            };
    }


    [ClientRpc]
    public void Rpc_ChangeDialogueToServer(Dialogue dialogue)
    {
        CurrentDialogue = dialogue;
    }
}

