using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour {

    public Shader ToonShader;
    public Shader HighlightShader;
    public Color HighlightColor;
    public float OutlineWidth = 0.0005f;
    private Renderer Rend;
    // Use this for initialization
    void Start()
    {
        Rend = gameObject.GetComponent<Renderer>();
    }

    public void ToggleHighlight(bool bShowHighlight)
    {
        if (Rend && HighlightShader && ToonShader)
        {
            Rend.material.shader = bShowHighlight ? HighlightShader : ToonShader;
            Rend.material.SetColor("_OutlineColor", HighlightColor);
            Rend.material.SetFloat("_OutlineWidth", OutlineWidth);
        }
    }
}
