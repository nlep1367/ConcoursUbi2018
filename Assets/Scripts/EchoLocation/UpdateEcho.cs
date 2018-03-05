using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class UpdateEcho : MonoBehaviour {

    public float VisionRadius = 10;
    public Transform PlayerPosRef;
    void Start()
    {
        Shader.SetGlobalFloat("_EchoRadius", VisionRadius);
    }
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_PlayerPosition", PlayerPosRef.position);
    }
}
