using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundsControl : MonoBehaviour {

	public AudioClip[] FootstepsClips;

	protected AudioSource[] m_AudioSource;

	// Use this for initialization
	void Awake () {
		m_AudioSource = GetComponents<AudioSource> ();
	}

	protected virtual void Step()
	{
		m_AudioSource[0].PlayOneShot(GetRandomClip (FootstepsClips));
	}

	protected AudioClip GetRandomClip(AudioClip[] clip)
	{
		return clip[Random.Range (0, clip.Length)];
	}
}
