using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoSoundsControl : CharacterSoundsControl {
	
	public AudioClip JoyfulBarkClip;
	public AudioClip AggressiveBarkClip;
	public AudioClip PushClip;

	private bool m_IsPushPlaying = false;

	public void BarkJoyfully()
	{
		if (!m_IsPushPlaying) {
			m_AudioSource [1].PlayOneShot (JoyfulBarkClip);
		}
	}

	public void BarkAggressively()
	{
		if (!m_IsPushPlaying) {
			m_AudioSource [1].PlayOneShot (AggressiveBarkClip);
		}
	}

	public void StartPush()
	{
		if (m_AudioSource [1].clip != PushClip || !m_IsPushPlaying) {
			m_IsPushPlaying = true;
			m_AudioSource [1].loop = true;
			m_AudioSource [1].clip = PushClip;
			m_AudioSource [1].Play ();
		}
	}

	public void StopPush()
	{
		if (m_IsPushPlaying) {
			m_IsPushPlaying = false;
			m_AudioSource [1].Stop ();
			m_AudioSource [1].loop = false;
		}
	}
}
