using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelSoundControl : MonoBehaviour {
	public AudioClip InitialSpookedNoise;
	public AudioClip SpookedNoise;

	public bool ToPlayInitSpooked = false;
	public bool ToPlaySpooked = false;

	private AudioSource m_AudioSource;

	private float lowPitchRange = .75F;
	private float highPitchRange = 1.5F;

	void Awake()
	{
		m_AudioSource = transform.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ToPlayInitSpooked) 
		{
			PlayInitSpooked ();
			ToPlayInitSpooked = false;
		}

		if (ToPlaySpooked) 
		{
			PlaySpooked ();
			ToPlaySpooked = false;
		}
	}

	public void PlayInitSpooked ()
	{
		if (!m_AudioSource.isPlaying) {
			m_AudioSource.pitch = 1.0f;
			m_AudioSource.PlayOneShot (InitialSpookedNoise);
		}
	}

	public void PlaySpooked ()
	{
		if (!m_AudioSource.isPlaying) {
			m_AudioSource.pitch = Random.Range (lowPitchRange,highPitchRange);
			m_AudioSource.PlayOneShot (SpookedNoise);
		}
	}
}
