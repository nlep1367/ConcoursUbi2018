using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightSoundsControl : MonoBehaviour {
	public AudioClip ButtonPressedNoise;
	public AudioClip BeepingNoise;

	public bool ToPlayButtonPressedSpooked = false;
	public bool ToPlayBeeping = false;

	public uint NbSecToBeep;

	private AudioSource m_AudioSource;

	private bool m_isInitBeepPlaying = false;
	private bool m_isBeepPlaying = false;
	private float m_currentTime;

	void Awake()
	{
		m_AudioSource = transform.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (ToPlayButtonPressedSpooked) 
		{
			PlayButtonPressed ();
			ToPlayButtonPressedSpooked = false;
		}

		if (ToPlayBeeping) 
		{
			PlayBeep ();
			ToPlayBeeping = false;
		}

		if (!m_isBeepPlaying && !m_isInitBeepPlaying)
			return;

		if (m_isInitBeepPlaying) {
			m_currentTime += Time.deltaTime;

			if (m_currentTime > ButtonPressedNoise.length) {
				m_isInitBeepPlaying = false;
			}
		}

		if (m_isBeepPlaying) {
			m_currentTime += Time.deltaTime;

			if (m_currentTime > NbSecToBeep) {
				StopBeep ();
			}
		}
	}

	public void PlayButtonPressed()
	{
		if (!m_AudioSource.isPlaying) {
			m_AudioSource.clip = ButtonPressedNoise;
			m_AudioSource.Play ();
			m_isInitBeepPlaying = true;
		}
	}

	public void PlayBeep ()
	{
		if (m_AudioSource.isPlaying)
			m_AudioSource.Stop ();

		m_AudioSource.loop = true;
		m_AudioSource.clip = BeepingNoise;
		m_AudioSource.Play ();
		m_isBeepPlaying = true;
	}

	public void StopBeep()
	{
		if (m_isBeepPlaying) {
			m_AudioSource.Stop();
			m_AudioSource.clip = null;
			m_AudioSource.loop = false;
			m_isBeepPlaying = false;
		}
	}
}
