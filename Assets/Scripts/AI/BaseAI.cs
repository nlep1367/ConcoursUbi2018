using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseAI : NetworkBehaviour {

    protected Stimuli CurrentStimuli;

    public float HearingRange;

    public virtual bool IsBusy()
    {   
        return false;
    }

    public void SetSoundStimuli(Stimuli NewStim)
    {
        CurrentStimuli = NewStim;
    }
}
