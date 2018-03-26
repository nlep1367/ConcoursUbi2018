using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMusicControl : MonoBehaviour {

	public AudioClip[] Clips;

	private AudioSource m_AudioSource;

	// Use this for initialization
	void Awake () {
		m_AudioSource = GetComponent<AudioSource> ();
	}

	private void Step()
	{
		AudioClip clip = GetRandomClip ();
		m_AudioSource.PlayOneShot(clip);
	}

	private AudioClip GetRandomClip()
	{
		return Clips[Random.Range (0, Clips.Length)];
	}
}
