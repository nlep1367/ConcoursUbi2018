using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour {

    public Color ColorStart = Color.white;
    public Color ColorEnd = Color.magenta;
    public float duration = 2f;
    public Camera Cam;

    void OnEnable ()
    {
        if (Cam)
        {
            Cam.clearFlags = CameraClearFlags.SolidColor;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(enabled && Cam)
        {
            float t = Mathf.PingPong(Time.time, duration) / duration;
            Cam.backgroundColor = Color.Lerp(ColorStart, ColorEnd, t);
        }   
    }

    void OnDisable()
    {
        if (Cam)
        {
            Cam.backgroundColor = ColorStart;
        }
    }
}
