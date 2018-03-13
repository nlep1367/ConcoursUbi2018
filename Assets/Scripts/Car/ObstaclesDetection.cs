using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDetection : MonoBehaviour {

    CarEngine engine;
    // Collider obstacle;
    int counter = 0;

    private void Start()
    {
        engine = GetComponentInParent<CarEngine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // To ignore the collision with the other 'Trigger' collider
        if(!(other.GetComponent<DangerZone>() || other.CompareTag("Terrain") || other.GetComponent<ObstaclesDetection>()))
        {
            //obstacle = other;
            counter++;
            engine.hasObstacle = counter !=0;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        // To ignore the collision with the other 'Trigger' collider
        if (!(other.GetComponent<DangerZone>() || other.CompareTag("Terrain") || other.GetComponent<ObstaclesDetection>()))
        {
            //obstacle = null;
            counter--;
            engine.hasObstacle = counter !=0;
        }            
    }

    private void OnDrawGizmosSelected()
    {
        /*
        if (obstacle)
        {            
            Gizmos.DrawCube(obstacle.transform.position, new Vector3(5,5,5));
        }
        */
    }

}
