using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractable : MonoBehaviour {

    private HintUI hintUI;

    private void Start()
    {
        hintUI = FindObjectOfType<HintUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fille") && other.GetComponentInParent<ObjectSync>().hasAuthority)
        {
            hintUI.Display(KeyCode.A, "Hit the button");
            GetComponent<ReplacementShaderObject>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fille") )
        {
            if(other.GetComponentInParent<ObjectSync>().hasAuthority && Input.GetButtonDown("A"))
            {
                GetComponentInParent<TrafficLightNode>().HitPedestrianButton();
                hintUI.Hide();
            }
                
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fille") && other.GetComponentInParent<ObjectSync>().hasAuthority)
        {
            hintUI.Hide();
            GetComponent<ReplacementShaderObject>().enabled = false;
        }
    }
}
