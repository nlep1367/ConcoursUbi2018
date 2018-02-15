using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour {

    protected Stimuli CurrentStimuli;

    public virtual bool IsBusy()
    {   
        return false;
    }

    public void SetSoundStimuli(Stimuli NewStim)
    {
        CurrentStimuli = NewStim;
    }
}
