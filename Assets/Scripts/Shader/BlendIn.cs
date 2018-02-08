using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicTools.ButtonInspector;

public class BlendIn : MonoBehaviour {

    [Range(0, 1)]
    public float Blend = 0;

    [Tooltip("Time it takes to blend in (seconds)")]
    public float BlendingTime = 1;

    public bool Triggered = false;

    public Material mat1;
    public Material mat2;

    [Button("Reset", "Reset")]
    public bool button_1;

    private Renderer Rend;


    public void Start()
    {
        Rend = GetComponent<Renderer>() as Renderer;
    }
    
    public void Reset()
    {
        Rend.material = mat1;
        Triggered = false;
        Blend = 0;
       // Rend.material.SetFloat("_BlendValue", Blend);
       // Rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Rend.receiveShadows = false;
    }

    public void Update()
    {
        if (Blend < 1 && Triggered)
        {
            Blend += Time.deltaTime / BlendingTime;
            //Rend.receiveShadows = true;

            Rend.material.Lerp(mat1, mat2, 0);
        }
        else if(Triggered)
        {
            Triggered = false;
            Blend = 1;
           // Rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        
      //  Rend.material.SetFloat("_BlendValue", Blend);
    }

    public void OnPreRender()
    {
    }
}
