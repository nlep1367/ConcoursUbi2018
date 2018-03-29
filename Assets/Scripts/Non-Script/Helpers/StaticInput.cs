using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticInput {
    public static bool InputInactive = true;

    public static bool GetButton(string buttonName)
    {
        if (InputInactive) return false;
        return Input.GetButton(buttonName);
    }

    public static bool GetButtonDown(string buttonName)
    {
        if (InputInactive) return false;
        return Input.GetButtonDown(buttonName);
    }

    public static bool GetButtonUp(string buttonName)
    {
        if (InputInactive) return false;
        return Input.GetButtonUp(buttonName);
    }

    public static float GetAxis(string axisName)
    {
        if (InputInactive) return 0f;
        return Input.GetAxis(axisName);
    }
}