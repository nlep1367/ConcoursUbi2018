using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPushable : MonoBehaviour {
    public void OnTriggerStay(Collider other)
    {
        TriggerFeet triggerFeet = other.GetComponent<TriggerFeet>();

        if (!(triggerFeet == null))
        {
            Player player = triggerFeet.GetComponentInParent<Player>();

            if (player.State != player.States[StateEnum.CLIMBING])
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    
                    Vector3 pushDirection = this.transform.parent.position - this.transform.position;
                    pushDirection.y = 0;
                    player.transform.forward = pushDirection;

                    player.ChangeState(StateEnum.PUSHING, this.GetComponentInParent<Rigidbody>());
                }
                else
                {
                    //Display cmd prompt?   
                }
            }
        }
    }
}
