using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ReplacementShaderCam : MonoBehaviour {
    public Shader ReplacementShader;
    void OnEnable()
    {
        if (ReplacementShader != null)
        {
            GetComponent<Camera>().SetReplacementShader(ReplacementShader, "RenderType");
        }
    }

    // Update is called once per frame
    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
