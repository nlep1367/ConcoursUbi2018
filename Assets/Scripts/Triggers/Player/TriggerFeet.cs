using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFeet : MonoBehaviour {
    Player _player;

    public StateEnum From;
    public StateEnum SwapState;
    void Start()
    {
        _player = this.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_player.IsState(From))
        {
            return;
        }

        MeshRenderer ground = other.GetComponentInParent<MeshRenderer>();
        if (ground != null)
        {
            _player.ChangeState(SwapState);
        }
    }
}
