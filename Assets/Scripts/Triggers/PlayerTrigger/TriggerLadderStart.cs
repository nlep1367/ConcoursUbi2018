using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLadderStart : MonoBehaviour {
    public Vector3 Position;
    public Quaternion Rotation;
    public bool GoingUp;

    const float PlayerHeight = 2f;
    const float PlayerLength = 0.5f;

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
                    //Teleport player to ladder
                    player.RigidBody.velocity = Vector3.zero;
                    player.transform.rotation = Rotation;

                    //Should change arbitrary constants by actual player collider size
                    player.transform.position = Position - PlayerLength * player.transform.forward + (PlayerHeight * player.transform.up * (GoingUp ? 1 : -1)) ;

                    player.ChangeState(StateEnum.CLIMBING);
                }
                else
                {
                    //Display command prompt? 
                }
            }
        }
    }
}
