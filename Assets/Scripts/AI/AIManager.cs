using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stimuli
{
    float Duration;

    public Vector3 Position;

    public float Size;

    bool Done;

    public Stimuli(float Duration, Vector3 Position, float Size)
    {
        this.Duration = Duration;
        this.Position = Position;
        this.Size = Size;
        this.Done = false;
    }

    public void Update()
    {
        Duration -= Time.deltaTime;

        if(Duration <= 0.0f)
        {
            Done = true;
        }
    }

    public bool IsOver()
    {
        return Done;
    }

    public Vector3 GetPosition()
    {
        return Position;
    }
}

public class AIManager : MonoBehaviour {

    public List<BaseAI> AllAI = new List<BaseAI>();

    private List<Stimuli> SoundStimulus = new List<Stimuli>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Update the Stimilus

		for(int i = 0; i < SoundStimulus.Count; i++)
        {
            SoundStimulus[i].Update();

            if(SoundStimulus[i].IsOver())
            {
                SoundStimulus.RemoveAt(i);
            }
        }

        // Update the AIs
        foreach(BaseAI ai in AllAI)
        {
            if (ai.IsBusy())
                break;

            Stimuli NearestStimuli = null;
            float CurrentDist = 100.0f;

            foreach (Stimuli s in SoundStimulus)
            {
                float dist = Vector3.Distance(s.GetPosition(), ai.transform.position);

                if(dist < CurrentDist)
                {
                    NearestStimuli = s;
                    CurrentDist = dist;
                }
            }

            if(NearestStimuli != null)
            { 
                ai.SetSoundStimuli(NearestStimuli);
            }
        }


	}

    public void AddStimuli(float Duration, Vector3 Position, float Size)
    {
        SoundStimulus.Add(new Stimuli(Duration, Position, Size));
    }

    private void OnDrawGizmos()
    {
        foreach (Stimuli s in SoundStimulus)
        {
            Gizmos.DrawWireSphere(s.GetPosition(), 1.0f);
        }
    }
}
