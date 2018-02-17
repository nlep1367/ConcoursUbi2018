using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLadderEnd : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        TriggerFeet triggerFeet = other.GetComponent<TriggerFeet>();

        if (!(triggerFeet == null))
        {
            Player player = triggerFeet.GetComponentInParent<Player>();
            
            if (player.State == player.States[StateEnum.CLIMBING])
            {
                player.ChangeState(StateEnum.GETTING_OFF_LADDER);
            }
        }
    }
}
