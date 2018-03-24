﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController_Graphics : MonoBehaviour {

    #region Variables

    [Header("_Graphics Options_")]
    [Space(5)]
    public Text toggleFullscreen_text;
    [HideInInspector]
    public int toggleFullscreen;
    [Space(10)]

    [HideInInspector]
    public int toggleAnisoFilt;
    public Text AnisoFiltering_text;
    [Space(10)]

    [HideInInspector]
    public int toggleAntiAlias;
    public Text AntiAlias_text;
    [Space(10)]

    [HideInInspector]
    public int toggleVsync;
    public Text toggleVsync_text;
    [Space(10)]

    [HideInInspector]
    public int toggleShadows;
    public Text toggleShadows_text;
    [Space(10)]

    [HideInInspector]
    public int toggleTextureQuality;
    public Text toggleTextureQuality_text;
    [Space(10)]

    [HideInInspector]
    public int currentScreenResolutionCount;
    public Text currentScreenResolution_text;
    Resolution[] allScreenResolutions;
    #endregion

    // Use this for initialization
    void Start () {

        //load graphics settings on start 
        gfx_setDefaults();
	}

    #region _Graphics Options_

    /// <summary>
    /// Toggle Fullscreen
    /// </summary>
    public void gfx_fullScreen()
    {
        //perform toggle
        if (toggleFullscreen == 0)
        {
            toggleFullscreen = 1;
            toggleFullscreen_text.text = "Yes";
        }
        else
        {
            toggleFullscreen = 0;
            toggleFullscreen_text.text = "No";
        }

        //set values
        Screen.SetResolution(Screen.width, Screen.height, toggleFullscreen == 1 ? true : false);

        //override new setting
        #if !EMM_ES2
        PlayerPrefs.SetInt("toggleFullscreen", toggleFullscreen);
        #else
        ES2.Save(toggleFullscreen, "toggleFullscreen");
        #endif
       

        //play click sound
        EasyAudioUtility.instance.Play("Hover");
    }

    /// <summary>
    /// only set the value
    /// </summary>
    void gfx_setFullScreen()
    {
        //set values
        Screen.SetResolution(Screen.width, Screen.height, toggleFullscreen == 1 ? true : false);

        //set text
        toggleFullscreen_text.text = toggleFullscreen == 1 ? "Yes" : "No";
    }

    /// <summary>
    /// Scroll through various screen resolutions
    /// </summary>
    public void gfx_ScreenResolution()
    {
        //if the count is less, it means we can increase more resolution
       if(currentScreenResolutionCount < allScreenResolutions.Length)
        {
            Screen.SetResolution(allScreenResolutions[currentScreenResolutionCount].width,
               allScreenResolutions[currentScreenResolutionCount].height, toggleFullscreen == 1 ? true : false);
            
            //increment so that we increase it next time
            currentScreenResolutionCount++;
        }
        else
        {
            //start the count from zero
            currentScreenResolutionCount = 0;
            Screen.SetResolution(allScreenResolutions[currentScreenResolutionCount].width,
              allScreenResolutions[currentScreenResolutionCount].height, toggleFullscreen == 1 ? true : false);
        }

        //save finally
        #if !EMM_ES2
        PlayerPrefs.SetInt("currentScreenResolutionCount", currentScreenResolutionCount);
        #else
        ES2.Save(currentScreenResolutionCount, "currentScreenResolutionCount");
        #endif


    }

    /// <summary>
    /// Set the value of last saved screen resolution
    /// </summary>
    void gfx_setScreenResolution()
    {
        //retrieve all resolutions on start
        allScreenResolutions = Screen.resolutions;

        //set resolution
        // if the resolution has been set before
        if (PlayerPrefs.HasKey("currentScreenResolutionCount"))
        {
            Screen.SetResolution(allScreenResolutions[currentScreenResolutionCount].width,
                allScreenResolutions[currentScreenResolutionCount].height, toggleFullscreen == 1 ? true : false);
        }
        else
        {

            //set current resolution of the system
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height,
                                toggleFullscreen == 1 ? true : false);
        }
    }

    /// <summary>
    /// sets Default gfx settings 
    /// </summary>
    void gfx_setDefaults()
    {
        //retrieve previous setting
        #if !EMM_ES2

        toggleFullscreen = PlayerPrefs.GetInt("toggleFullscreen",1); 
        toggleAnisoFilt = PlayerPrefs.GetInt("toggleAnisoFilt",1); 
        toggleVsync = PlayerPrefs.GetInt("toggleVsync",1); 
        toggleShadows = PlayerPrefs.GetInt("toggleShadows",1);
        toggleTextureQuality = PlayerPrefs.GetInt("toggleTextureQuality",1);
        toggleAntiAlias = PlayerPrefs.GetInt("toggleAntiAlias",1);
        currentScreenResolutionCount = PlayerPrefs.GetInt("currentScreenResolutionCount", currentScreenResolutionCount);

        #else

        toggleFullscreen = ES2.Exists("toggleFullscreen") ? ES2.Load<int>("toggleFullscreen") : 1;
        toggleAnisoFilt = ES2.Exists("toggleAnisoFilt") ? ES2.Load<int>("toggleAnisoFilt") : 1;
        toggleVsync = ES2.Exists("toggleVsync") ? ES2.Load<int>("toggleVsync") : 1;
        toggleShadows = ES2.Exists("toggleShadows") ? ES2.Load<int>("toggleShadows") : 1;
        toggleTextureQuality = ES2.Exists("toggleTextureQuality") ? ES2.Load<int>("toggleTextureQuality") : 1;
        toggleAntiAlias = ES2.Exists("toggleAntiAlias") ? ES2.Load<int>("toggleAntiAlias") : 1;
        currentScreenResolutionCount = ES2.Exists("currentScreenResolutionCount") ? ES2.Load<int>("currentScreenResolutionCount") : 1;

        #endif


        //set values accordingly
        gfx_setFullScreen();
        gfx_setScreenResolution();
    }
    #endregion

}
