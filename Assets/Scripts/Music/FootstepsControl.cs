using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsControl : MonoBehaviour {

	public AudioClip[] Clips;
	public AudioClip[] CanneClips;

	private AudioSource[] m_AudioSource;

	// Use this for initialization
	void Awake () {
		m_AudioSource = GetComponents<AudioSource> ();
	}

	private void Step()
	{
		AudioClip clip = GetRandomClip ();
		m_AudioSource[0].PlayOneShot(clip);

		if (CanneClips.Length != 0)
		{
			// Iris canne sound
			AudioClip canne = CanneClips[Random.Range (0, CanneClips.Length)];
			m_AudioSource [1].PlayOneShot (canne);
		}
	}

	private AudioClip GetRandomClip()
	{
		return Clips[Random.Range (0, Clips.Length)];
	}
}
