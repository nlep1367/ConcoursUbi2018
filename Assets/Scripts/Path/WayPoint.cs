using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {

    public TrafficLight trafficLight;
    public float yellowLightDistance = 7.5f;
    public float redLightDistance = 10f;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, redLightDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, yellowLightDistance);
    }

    public bool ShouldStop(Vector3 carPosition)
    {
        if (trafficLight)
        {
            float distance = Vector3.Distance(transform.position, carPosition);

            switch (trafficLight.lightState)
            {
                case TrafficLightState.Red:
                    if (distance < redLightDistance) return true;
                    break;
                case TrafficLightState.Yellow:
                    if (distance > yellowLightDistance && distance < redLightDistance) return true;
                    break;
            }
        }

        return false;
    }


}
