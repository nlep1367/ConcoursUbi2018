using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/** Use this class on object that have the EchosShader shader to generate independant echolocations **/
public class UpdateSideEchos : MonoBehaviour {
    private Transform ObjPos;
    private bool DoEcho;
    public Color EchoColor = Color.white;
    public float EchoWidth = 0.2f;
    public float MaxRad = 10f;
    public float Duration = 3f;
    public float StillDuration = 2f;
    private float Increment = 0f;
    private float CurrRad, Timer = 0f;
    private Renderer Rend;
    
    // Use this for initialization
    void Start ()
    {
        Increment = MaxRad / Duration;
        ObjPos = transform;
        Rend = gameObject.GetComponent<Renderer>();
        if (Rend && ObjPos)
        {
            Rend.material.SetColor("_Color", EchoColor);
            Rend.material.SetVector("_EchoCenter", ObjPos.position);
            Rend.material.SetFloat("_EchoRadius", CurrRad);
        }
    }

    void OnEnable()
    {
        DoEcho = true;
        if(Rend && ObjPos)
        {
            Rend.material.SetColor("_Color", EchoColor);
            Rend.material.SetVector("_EchoCenter", ObjPos.position);
        }
    }
	// Update is called once per frame
	void Update () {
		if(enabled && DoEcho && Rend)
        {
            if (CurrRad < MaxRad)
            {
                CurrRad += Increment * Time.deltaTime;
                Rend.material.SetFloat("_EchoRadius", CurrRad);
            }
            else
            {
                Timer += Time.deltaTime;
                if(Timer >= StillDuration)
                {
                    CurrRad = 0;
                    Rend.material.SetFloat("_EchoRadius", CurrRad);
                    Timer = 0;
                    enabled = false;
                }
            }
        }
	}
    void OnDisable()
    {
        if (Rend)
        {
            Rend.material.SetFloat("_EchoRadius", 0.0F);
        }
        CurrRad = 0f;
    }
}
