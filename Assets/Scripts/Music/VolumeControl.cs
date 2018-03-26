using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour {

	public AudioMixer masterMixer;

	private static float m_MusicVolume = 0;
	private static float m_SoundsVolume = 0;

	public void SetMusicVolume(float val)
	{
		m_MusicVolume = EaseInExpo (val);
		SetMusicVolume ();
	}

	public void SetMusicVolume()
	{
		masterMixer.SetFloat ("MusicVolume", m_MusicVolume);
	}

	public void SetSoundsVolume(float val)
	{
		m_SoundsVolume = EaseInExpo (val);
		SetSoundsVolume ();
	}

	public void SetSoundsVolume()
	{
		masterMixer.SetFloat ("SoundsVolume", m_SoundsVolume);
	}

	private float EaseInExpo(float val) {
		return (float)(80 * (-Math.Pow (2, -10 * val) + 1) - 80);
	}
}
