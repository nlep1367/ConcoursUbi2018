using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDetection : MonoBehaviour {

    public CarAI engine;
    // Collider obstacle;
    int counter = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        // To ignore the collision with the other 'Trigger' collider
        if(other.CompareTag("Fille") || other.CompareTag("Car") || other.CompareTag("Doggo"))
        {
            //obstacle = other;
            counter++;
            engine.isBreaking = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        // To ignore the collision with the other 'Trigger' collider
        if (other.CompareTag("Fille") || other.CompareTag("Car") || other.CompareTag("Doggo"))
        {
            //obstacle = null;
            counter--;
            if (counter == 0)
                engine.isBreaking = false;
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
