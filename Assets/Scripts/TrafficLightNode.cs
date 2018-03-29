using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class TrafficLightNode : NetworkBehaviour {

    private List<TrafficLight> all;
    public List<TrafficLight> horizontal;
    public List<TrafficLight> vertical;

    // red must equal green + yellow
    public float redLightDuration = 3f;
    public float yellowLightDuration = 1f;
    public float greenLightDuration = 2f;
    public float pedestrianDurationMultiplicator = 5f;

    private bool isReadyV = true;
    private bool isReadyH = true;
    private bool isReadyP = true;
    public bool hasPedestrian = false;

	private TrafficLightSoundsControl soundController;

    public override void OnStartServer()
    {
        InitLight();

        all = horizontal.Concat(vertical).ToList();
    }

    public void InitLight()
    {
        foreach (TrafficLight tf in horizontal)
        {
            tf.SetState(TrafficLightState.Green);
            tf.Rpc_SendStateToServer(TrafficLightState.Green);
        }

        foreach (TrafficLight tf in vertical)
        {
            tf.SetState(TrafficLightState.Red);
            tf.Rpc_SendStateToServer(TrafficLightState.Red);
        }
    }

    [Server]
    public void Update()
    {
        if (!hasPedestrian)
        {
            /*
            if (isReadyV)
            {
                isReadyV = false;
                CountdownToNextLight(false);
            }

            if (isReadyH)
            {
                isReadyH = false;
                CountdownToNextLight(true);
            }
            */
        }
        else
        {
            if (isReadyP)
            {
				isReadyP = false;
				soundController.PlayBeep ();
                CountdownPedestrian();
            }   
        }

    }

    [Server]
    void CountdownPedestrian()
    {
        foreach (TrafficLight tf in all)
        {
            tf.Rpc_SendStateToServer(TrafficLightState.Red);
        }

        StartCoroutine(Wait());
    }

    [Server]
    void CountdownToNextLight(bool isHorizontal)
    {
        List<TrafficLight> tfs = isHorizontal ? horizontal : vertical;

        if (tfs.Count == 0)
            return;

        float timeToWait = 0.0f;

        switch (tfs[0].getLightState())
        {
            case TrafficLightState.Green:
                timeToWait = greenLightDuration;
                break;

            case TrafficLightState.Yellow:
                timeToWait = yellowLightDuration;
                break;

            case TrafficLightState.Red:
                timeToWait = redLightDuration;
                break;
        }

        StartCoroutine(Wait(timeToWait, tfs, isHorizontal));
    }

    [Server]
    public IEnumerator Wait(float timeToWait, List<TrafficLight> tfs, bool isHorizontal)
    {
        yield return new WaitForSeconds(timeToWait);
        SetNextLight(tfs, isHorizontal);
    }

    [Server]
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(redLightDuration * pedestrianDurationMultiplicator);
        SetNextLight();
    }

    [Server]
    public void SetNextLight(List<TrafficLight> tfs, bool isHorizontal)
    {
        foreach(TrafficLight tf in tfs)
        {
            int nextVal = (int)tf.getLightState() + 1;
            if(!hasPedestrian)
                tf.Rpc_SendStateToServer((TrafficLightState)Enum.ToObject(typeof(TrafficLightState), nextVal % (int)TrafficLightState.Count));
        }

        if(isHorizontal)
            isReadyH = true;
        else
            isReadyV = true;
    }

    [Server]
    public void SetNextLight()
    {
        InitLight();

        isReadyH = isReadyV = isReadyP = true;
        hasPedestrian = false;
    }

    public void HitPedestrianButton()
    {
		if (soundController == null)
			soundController = GetComponent<TrafficLightSoundsControl> ();
		
		soundController.PlayButtonPressed ();
        hasPedestrian = true;
    }
}
