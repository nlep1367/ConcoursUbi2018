using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractable : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fille"))
        {
            GetComponent<ReplacementShaderObject>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fille") && Input.GetKeyDown(KeyCode.Space))
        {
            GetComponentInParent<TrafficLightNode>().HitPedestrianButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fille"))
        {
            GetComponent<ReplacementShaderObject>().enabled = false;
        }
    }
}
