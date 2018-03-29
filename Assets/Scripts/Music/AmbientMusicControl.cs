using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientMusicControl : MonoBehaviour 
{
	public enum StingStates 
	{
		Start,
		Win,
		Fail,
		Action1,
		Action2,
		Action3,
		Action4
	}

	public bool IsMainMenu;

	public bool ToStartStress = false;
	public bool ToStopStress = false;
	public bool ToNextAmbient = false;
	public bool ToWin = false;
	public bool ToFail = false;
	public bool ToAction1 = false;
	public bool ToAction2 = false;
	public bool ToAction3 = false;
	public bool ToAction4 = false;

	public AudioMixerSnapshot InGame;
	public AudioMixerSnapshot[] clipTransitions;
	public AudioMixerSnapshot InStoryboard;

	public AudioClip StartSting;
	public AudioClip WinSting;
	public AudioClip FailSting;

	public AudioClip ActionSting1;
	public AudioClip ActionSting2;
	public AudioClip ActionSting3;
	public AudioClip ActionSting4;

	public float bpm = 53;

	private GameObject m_AmbientPlayer;
	private GameObject m_StingPlayer;

	private StressMusicControl m_StressController;
	private MenuMusicControl m_MenuController;
	private VolumeControl m_VolumeController;

	private AudioSource m_StingSource;

	private AudioSource[] m_AmbientAudioSources;
	private AudioSource m_NextAmbientAudioSource;
	private int m_AmbientAudioClipIndex;

	private float m_Transition;
	private float m_QuarterNote;
	private float m_CurrentTime;

	private bool m_IsTransitionning = false;

	private bool[] m_AreAmbiantClipsPlaying;
	private bool[] m_AreAmbiantClipsStopping;

	// Use this for initialization
	void Start () 
	{
		m_QuarterNote = 60 / bpm;
		m_Transition = m_QuarterNote * 8;

		m_StingPlayer = transform.Find  ("StingPlayer").gameObject;
		m_StingSource = m_StingPlayer.GetComponent<AudioSource>();

		m_AmbientPlayer = transform.Find  ("AmbientPlayer").gameObject;
		m_AmbientAudioSources = m_AmbientPlayer.GetComponents<AudioSource>();
		m_AmbientAudioClipIndex = 0;

		m_StressController = GetComponent<StressMusicControl>(); 
		m_MenuController = GetComponent<MenuMusicControl>(); 
		m_VolumeController = GetComponent<VolumeControl>(); 

		m_AreAmbiantClipsPlaying = new bool[m_AmbientAudioSources.Length];
		m_AreAmbiantClipsStopping = new bool[m_AmbientAudioSources.Length];

		for (uint i = 0; i < m_AmbientAudioSources.Length; ++i) 
		{
			m_AreAmbiantClipsPlaying [i] = false;
			m_AreAmbiantClipsStopping [i] = false;
		}

		m_MenuController.Initialize ();

		if (IsMainMenu) 
		{
			m_MenuController.OnPlayMenuMusic ();
		} 
		else 
		{
			m_StressController.Initialize ();
			m_VolumeController.SetMusicVolume ();
			m_VolumeController.SetSoundsVolume ();

			InGame.TransitionTo (0);
			_PlayAmbient (0);
			PlaySting (StingStates.Start);
		}
	}

	void Awake()
	{
		GameEssentials.MusicPlayer = this;
	}

	// Update is called once per frame
	void Update () 
	{
		m_MenuController.Tick ();
		if (IsMainMenu) 
		{
			return;
		}

		_ReadCommands ();

		m_StressController.Tick ();

		if (m_IsTransitionning) 
		{
			m_CurrentTime += Time.deltaTime;

			if (m_CurrentTime > m_Transition) 
			{
				int index = (m_AmbientAudioClipIndex != 0) ? m_AmbientAudioClipIndex - 1 : m_AmbientAudioSources.Length - 1;
				_ConcludeAmbiant (index);
			} 
		}
	}

	public void SetMenuMusicActive(bool val)
	{
		if (val)
			OnPlayMusicMenu ();
		else
			OnStopMusicMenu ();
	}

	void OnPlayMusicMenu()
	{
		m_MenuController.OnPlayMenuMusic ();
	}

	void OnStopMusicMenu()
	{
		m_MenuController.OnStopMenuMusic ();
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
		_PlayAmbient(index);
		clipTransitions [index].TransitionTo (m_Transition);

		// Stop the old ambiant
		_StopAmbiant(index != 0 ? index - 1 : tailleMax - 1);

		PlaySting (StingStates.Start);
	}

	void _PlayAmbient(int index)
	{	
		if (!m_AreAmbiantClipsPlaying [index]) 
		{
			m_AreAmbiantClipsPlaying [index] = true;
			m_AmbientAudioSources [index].Play ();
		}
	}

	void _StopAmbiant(int index)
	{
		if (m_AreAmbiantClipsPlaying [index] && !m_AreAmbiantClipsStopping[index]) 
		{
			m_IsTransitionning = true;
			m_CurrentTime = 0;
		}
	}

	void _ConcludeAmbiant(int index)
	{
		m_AreAmbiantClipsPlaying [index] = false;
		m_AreAmbiantClipsStopping[index] = false;
		m_IsTransitionning = false;

		m_AmbientAudioSources [index].Stop();
	}

	public void PlaySting(StingStates state)
	{
		if (!m_StingSource.isPlaying) 
		{
			AudioClip clip = _SelectClip (state);

			if (clip != null) 
			{
				m_StingSource.clip = _SelectClip(state);
				m_StingSource.Play();
			}
		}
	}

	AudioClip _SelectClip(StingStates state) 
	{
		AudioClip clip;

		switch (state) 
		{
		case StingStates.Start: 
			clip = StartSting;
			break;
		case StingStates.Win: 
			clip = WinSting;
			break;
		case StingStates.Fail: 
			clip = FailSting;
			break;
		case StingStates.Action1: 
			clip = ActionSting1;
			break;
		case StingStates.Action2: 
			clip = ActionSting2;
			break;
		case StingStates.Action3: 
			clip = ActionSting3;
			break;
		case StingStates.Action4: 
			clip = ActionSting4;
			break;
		default : 
			clip = null;
			break;
		}

		return clip;
	}

	void _ReadCommands()
	{
		// Start scared effect
		if (ToStartStress) 
		{
			m_StressController.OnPlayStress ();
			ToStartStress = false;
		}

		// Stop scared effect
		if (ToStopStress) 
		{
			m_StressController.OnStopStress ();
			ToStopStress = false;
		}

		// Go to next ambient clip
		if (ToNextAmbient) 
		{
			OnChangeAmbient ();
			ToNextAmbient = false;
		}

		// Play win sting
		if (ToWin) 
		{
			PlaySting (StingStates.Win);
			ToWin = false;
		}

		// Play fail sting
		if (ToFail) 
		{
			PlaySting (StingStates.Fail);
			ToFail = false;
		}

		// Play Action1 sting
		if (ToAction1) 
		{
			PlaySting (StingStates.Action1);
			ToAction1 = false;
		}

		// Play Action2 sting
		if (ToAction2) 
		{
			PlaySting (StingStates.Action2);
			ToAction2 = false;
		}

		// Play Action3 sting
		if (ToAction3) 
		{
			PlaySting (StingStates.Action3);
			ToAction3 = false;
		}

		// Play Action4 sting
		if (ToAction4) 
		{
			PlaySting (StingStates.Action4);
			ToAction4 = false;
		}
		
	}
}
