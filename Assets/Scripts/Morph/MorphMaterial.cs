using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphMaterial : MonoBehaviour {
    public float MorphTime = 1f;

    public bool IsActive;

    private Renderer _renderer;

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

            if (_time > MorphTime)
            {
                Destroy(this);
            }
        }
    }
}
