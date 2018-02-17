using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFeet : MonoBehaviour {
    Player _player;

    void Start()
    {
        _player = this.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        MeshRenderer ground = other.GetComponentInParent<MeshRenderer>();
        if (ground != null)
        {
            _player.ChangeState(StateEnum.GROUNDED);
        }
    }
}
