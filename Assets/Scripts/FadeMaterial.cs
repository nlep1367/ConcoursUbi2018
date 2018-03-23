using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FadeMaterial : NetworkBehaviour {

    public float TimeBeforeDestroy;
    public float FadeStartTime;

    private bool IsDieing;
    private float CurrentTime = 0;
    private MeshRenderer Render;
    
    void Start()
    {
        Render = GetComponent<MeshRenderer>();
    }

    [ClientRpc]
    public void Rpc_Kill()
    {
        IsDieing = true;
    }
	
	// Update is called once per frame
	void Update () {
		if(IsDieing)
        {
            CurrentTime += Time.deltaTime;

            if(CurrentTime > FadeStartTime)
            {
                float alpha = (TimeBeforeDestroy - CurrentTime) / (TimeBeforeDestroy - FadeStartTime);

                Color currentColor = Render.material.color;

                Render.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
        }

        if (CurrentTime > TimeBeforeDestroy)
            Destroy(gameObject);
	}
}
