using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientMusicControl : MonoBehaviour {

	public bool ToStartStress = false;
	public bool ToNextAmbient = false;
	public bool ToStopStress = false;

	public AudioMixerSnapshot[] clipTransitions;

	public AudioClip[] ObjectiveSting;
	public AudioSource stingSource;

	public float bpm = 53;


	private GameObject m_AmbientPlayer;

	private StressMusicControl m_StressController;

	private AudioSource[] m_AmbientAudioSources;
	private AudioSource m_NextAmbientAudioSource;
	private int m_AmbientAudioClipIndex;

	private float m_Transition;
	private float m_QuarterNote;
	private float m_CurrentTime;

	private bool m_IsPlayingStress = false;
	private bool m_IsTransitionning = false;

	private bool[] m_AreAmbiantClipsPlaying;
	private bool[] m_AreAmbiantClipsStopping;

	// Use this for initialization
	void Start () {
		m_QuarterNote = 60 / bpm;
		m_Transition = m_QuarterNote * 8;

		m_AmbientPlayer = transform.Find  ("AmbientPlayer").gameObject;
		m_AmbientAudioSources = m_AmbientPlayer.GetComponents<AudioSource>();
		m_AmbientAudioClipIndex = 0;

		m_StressController = GetComponent<StressMusicControl>(); 

		m_AreAmbiantClipsPlaying = new bool[m_AmbientAudioSources.Length];
		m_AreAmbiantClipsStopping = new bool[m_AmbientAudioSources.Length];

		for (uint i = 0; i < m_AmbientAudioSources.Length; ++i) {
			m_AreAmbiantClipsPlaying [i] = false;
			m_AreAmbiantClipsStopping [i] = false;
		}

		OnPlayAmbient (0);
	}

	// Update is called once per frame
	void Update () 
	{
		// Start scared effect
		if (ToStartStress) {
			m_StressController.OnPlayStress (m_AmbientAudioClipIndex);
			m_IsPlayingStress = true;
			ToStartStress = false;
		}

		// Stop scared effect
		if (ToStopStress) {
			m_StressController.OnStopStress (m_AmbientAudioClipIndex);
			ToStopStress = false;
		}

		// Go to next ambient clip
		if (ToNextAmbient) {
			OnChangeAmbient ();
			ToNextAmbient = false;
		}
			
		if (m_IsPlayingStress)
			m_StressController.Tick ();

		if (m_IsTransitionning) {
			m_CurrentTime += Time.deltaTime;

			if (m_CurrentTime > m_Transition) {
				int index = (m_AmbientAudioClipIndex != 0) ? m_AmbientAudioClipIndex - 1 : m_AmbientAudioSources.Length - 1;
				OnConcludeAmbiant (index);
			} 
		}
	}

	void OnChangeAmbient()
	{
		// Update the index of the current ambient clip
		int index;
		int tailleMax = m_AmbientAudioSources.Length;
		// For test purpose
		// To loop through the ambient clips
		if (m_AmbientAudioClipIndex == tailleMax - 1)
			m_AmbientAudioClipIndex = index = 0;
		else
			index = ++m_AmbientAudioClipIndex;

		// Play the new ambiant
		OnPlayAmbient(index);
		clipTransitions [index].TransitionTo (m_Transition);

		// Stop the old ambiant
		OnStopAmbiant(index != 0 ? index - 1 : tailleMax - 1);
	}

	void OnPlayAmbient(int index)
	{	
		if (!m_AreAmbiantClipsPlaying [index]) {
			m_AreAmbiantClipsPlaying [index] = true;
			m_AmbientAudioSources [index].Play ();
		}
	}

	void OnStopAmbiant(int index)
	{
		if (m_AreAmbiantClipsPlaying [index] && !m_AreAmbiantClipsStopping[index]) {
			m_IsTransitionning = true;
			m_CurrentTime = 0;
		}
	}

	void OnConcludeAmbiant(int index)
	{
		m_AreAmbiantClipsPlaying [index] = false;
		m_AreAmbiantClipsStopping[index] = false;
		m_IsTransitionning = false;

		m_AmbientAudioSources [index].Stop();
	}

	void PlaySting()
	{
		//		int randClip = Random.Range (0, stings.Length);
		//		stingSource.clip = stings[randClip];
		//		stingSource.Play();
	}
}
