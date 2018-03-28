using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoSoundsControl : CharacterSoundsControl {
	
	public AudioClip JoyfulBarkClip;
	public AudioClip AggressiveBarkClip;

	public void BarkJoyfully()
	{
		m_AudioSource [1].PlayOneShot (JoyfulBarkClip);
	}

	public void BarkAggressively()
	{
		m_AudioSource [1].PlayOneShot (AggressiveBarkClip);
	}
}
