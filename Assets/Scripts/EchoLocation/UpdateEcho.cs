using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class UpdateEcho : MonoBehaviour {

    public float VisionRadius = 10;
    public Transform PlayerPosRef;
    public Camera Cam;
    void Start()
    {
        Shader.SetGlobalFloat("_EchoRadius", VisionRadius);
    }
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_PlayerPosition", PlayerPosRef.position);
        Shader.SetGlobalColor("_ColorBG", Cam.backgroundColor);
    }
}
