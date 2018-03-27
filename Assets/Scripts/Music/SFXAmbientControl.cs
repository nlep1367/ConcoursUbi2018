using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXAmbientControl : MonoBehaviour {
	
	public AudioMixerSnapshot[] clipTransitions;

	// Index for m_AmbientAudioSources
	public uint Ambient = 0;
	public float Transition;

	private GameObject m_AmbientPlayer;
	private AudioSource[] m_AmbientAudioSources;
	private uint m_AmbientAudioClipIndex;

	private float m_QuarterNote;
	private float m_CurrentTime;

	private int m_InTransitionning = -1;

	private bool[] m_AreAmbiantClipsPlaying;
	private bool[] m_AreAmbiantClipsStopping;

	// Use this for initialization
	void Start () {
		m_AmbientPlayer = transform.Find  ("SFXAmbientPlayer").gameObject;
		m_AmbientAudioSources = m_AmbientPlayer.GetComponents<AudioSource>();
		m_AmbientAudioClipIndex = Ambient;

		m_AreAmbiantClipsPlaying = new bool[m_AmbientAudioSources.Length];
		m_AreAmbiantClipsStopping = new bool[m_AmbientAudioSources.Length];

		for (uint i = 0; i < m_AmbientAudioSources.Length; ++i) 
		{
			m_AreAmbiantClipsPlaying [i] = false;
			m_AreAmbiantClipsStopping [i] = false;
		}

		_PlayAmbient (0);
		clipTransitions [m_AmbientAudioClipIndex].TransitionTo (0);
	}
	
	// Update is called once per frame
	void Update () {
		// Go to next ambient clip
		if (m_AmbientAudioClipIndex != Ambient) 
		{
			OnChangeAmbient ();
		}
			
		if (m_InTransitionning != -1) 
		{
			m_CurrentTime += Time.deltaTime;

			if (m_CurrentTime > Transition) 
			{
				_ConcludeAmbiant (m_InTransitionning);
			} 
		}
	}

	void OnChangeAmbient()
	{
		uint index = m_AmbientAudioClipIndex;

		if (Ambient < m_AmbientAudioSources.Length) 
		{
			index = Ambient;
		}
		else 
		{
			// The updated index is invalid
			Ambient = m_AmbientAudioClipIndex;
			return;
		}

		// Play the new ambiant
		_PlayAmbient(index);
		clipTransitions [index].TransitionTo (Transition);

		// Stop the old ambiant
		_StopAmbiant(m_AmbientAudioClipIndex);

		// Update the index of the current ambient clip
		m_AmbientAudioClipIndex = index;
	}

	void _PlayAmbient(uint index)
	{	
		if (!m_AreAmbiantClipsPlaying [index]) 
		{
			m_AreAmbiantClipsPlaying [index] = true;
			m_AmbientAudioSources [index].Play ();
		}
	}

	void _StopAmbiant(uint index)
	{
		if (m_AreAmbiantClipsPlaying [index] && !m_AreAmbiantClipsStopping[index]) 
		{
			m_InTransitionning = (int)index;
			m_CurrentTime = 0;
		}
	}

	void _ConcludeAmbiant(int index)
	{
		m_AreAmbiantClipsPlaying [index] = false;
		m_AreAmbiantClipsStopping[index] = false;
		m_InTransitionning = -1;

		m_AmbientAudioSources [index].Stop();
	}
}
