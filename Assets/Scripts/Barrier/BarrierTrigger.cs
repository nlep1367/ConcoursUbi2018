using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierTrigger : MonoBehaviour {

    public System.Action<bool> GirlTriggerState;
    public HintUI hUI;

    private PickupObject puOb;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fille")
        {
            if (puOb == null)
            {
                puOb = other.gameObject.GetComponent<PickupObject>();
            }

            if (GirlTriggerState != null)
            {
                GirlTriggerState(true);
                if (other.gameObject.GetComponent<ObjectSync>().hasAuthority)
                { 
                    GameObject carried = puOb.GetCarriedObject();
                    puOb.CanDrop = false;
                    if (carried != null)
                        hUI.Display(Controls.Y, "Activate Gate");
                    else
                        hUI.Display(Controls.Y, "Insert Object");
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fille")
        { 
            if (GirlTriggerState != null)
            {
                GirlTriggerState(false);
                if (other.gameObject.GetComponent<ObjectSync>().hasAuthority)
                {
                    hUI.Hide();
                    puOb.CanDrop = true;
                }
            }
        }
    }
}
