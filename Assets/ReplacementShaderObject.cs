using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ReplacementShaderObject : MonoBehaviour
{
    private List<MeshRenderer> meshRenderer;
    private Shader originalShader;
    public Shader replacementShader;

    void Awake()
    {
        //meshRenderer = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        //originalShader = meshRenderer[0].material.shader;
    }
    void OnEnable()
    {
        if (replacementShader != null)
        {
           // foreach(var ms in meshRenderer)
                //ms.material.shader = replacementShader;
        }
    }

    // Update is called once per frame
    void OnDisable()
    {
        if(meshRenderer != null)
        {
            //foreach (var ms in meshRenderer)
               // ms.material.shader = originalShader;
        }
    }
}
