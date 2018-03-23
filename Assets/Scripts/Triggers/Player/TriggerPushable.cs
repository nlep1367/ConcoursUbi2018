using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPushable : MonoBehaviour {

    private HintUI hintUI;

    private void Start()
    {
        hintUI = FindObjectOfType<HintUI>();
    }

    public void OnTriggerEnter(Collider other)
    {
        ObjectSync os = other.GetComponentInParent<ObjectSync>();

        if (os != null && os.hasAuthority && (os.CompareTag("Doggo") || os.CompareTag("Fille")))
        {
            hintUI.Display(KeyCode.A, "Push the box");
        }
    }

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
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        ObjectSync os = other.GetComponentInParent<ObjectSync>();

        if (os != null && os.hasAuthority && (os.CompareTag("Doggo") || os.CompareTag("Fille")))
        {
            hintUI.Hide();
        }
    }
}
