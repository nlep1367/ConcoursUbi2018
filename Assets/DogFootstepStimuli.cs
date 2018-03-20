using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Flags]
public enum Paws
{
    None = 0,
    FrontLeft = 1,
    FrontRight = 2,
    BackRight = 4,
    BackLeft = 8
}

public class DogFootstepStimuli : MonoBehaviour {

    public Transform Paw_FrontLeft;
    public Transform Paw_FrontRight;
    public Transform Paw_BackRight;
    public Transform Paw_BackLeft;

    public float Duration;

    private AIManager AIManager;

    // Use this for initialization
    void Start () {
        GameObject Manager = GameObject.FindGameObjectWithTag("AI-Manager");
        AIManager = Manager.GetComponent<AIManager>();
    }

    public void Footstep(int paw)
    {
        Paws paws = (Paws)paw;

        if((paws & Paws.FrontLeft) != Paws.None)
            AIManager.Cmd_AddStimuli(Duration, Paw_FrontLeft.position, 2.0f);

        if ((paws & Paws.FrontRight) != Paws.None)
            AIManager.Cmd_AddStimuli(Duration, Paw_FrontRight.position, 2.0f);

        if ((paws & Paws.BackRight) != Paws.None)
            AIManager.Cmd_AddStimuli(Duration, Paw_BackRight.position, 2.0f);

        if ((paws & Paws.BackLeft) != Paws.None)
            AIManager.Cmd_AddStimuli(Duration, Paw_BackLeft.position, 2.0f);

        // Call paw sound
    }
}
