using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour {

	public AudioMixer masterMixer;

	public void SetMusicVolume(float val)
	{
		masterMixer.SetFloat ("MusicVolume", EaseInExpo(val));
	}

	public void SetSoundsVolume(float val)
	{
		masterMixer.SetFloat ("SoundsVolume", EaseInExpo(val));
	}

	private float EaseInExpo(float val) {
		return (float)(80 * (-Math.Pow (2, -10 * val) + 1) - 80);
	}
}
