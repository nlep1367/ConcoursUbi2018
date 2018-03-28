using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class MorphMaterial : NetworkBehaviour {
    public float MorphTime = 1f;

    public bool IsActive;

    private Renderer _renderer;
    public Renderer _renderer_bat;
    public int nbr;

    private float _time;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (IsActive)
        {
            _time += Time.deltaTime;

            _renderer.material.SetFloat("_Value", Mathf.Clamp(_time / MorphTime, 0f, 1f));
            _renderer_bat.materials[nbr].SetFloat("_Value", Mathf.Clamp(_time / MorphTime, 0f, 1f));

            if (_time > MorphTime)
            {
                Destroy(this);
            }
        }
    }

    [ClientRpc]
    public void Rpc_SetActive()
    {
        IsActive = true;
    }
}
