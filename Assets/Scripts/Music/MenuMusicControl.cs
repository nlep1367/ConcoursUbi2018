using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuMusicControl : MonoBehaviour {

	public AudioMixerSnapshot InMenu;
	public AudioMixerSnapshot OutMenu;
	public AudioMixerSnapshot InMenuIntro;
	public AudioMixerSnapshot OutMenuIntro;

	public float TransitionIn;
	public float TransitionOut;

	private GameObject m_MenuPlayer;

	private AudioSource m_BackgroundAudioSource;
	private AudioSource m_ButtonsAudioSource;
	private AudioSource m_IntroAudioSource;

	public AudioClip HoverSting;
	public AudioClip ClickSting;

	private float m_currentTime;

	private bool m_playingMenu = false;
	private bool m_stoppingIntro = false;
	private bool m_stoppingMenu = false;

	// Use this for initialization
	public void Initialize () 
	{
		m_MenuPlayer = transform.Find ("MenuPlayer").gameObject;

		AudioSource[] sources = m_MenuPlayer.GetComponents<AudioSource> ();
		m_BackgroundAudioSource = sources [0];
		m_ButtonsAudioSource = sources [1];
		m_IntroAudioSource = sources [2];
	}

	// Tick is called by the AmbientMusicControl::Update if necessary
	public void Tick () 
	{
		if (!m_playingMenu && !m_stoppingIntro && !m_stoppingMenu)
			return;

		if (m_stoppingIntro) {
			m_currentTime += Time.deltaTime;

			if (m_currentTime > TransitionIn + 60F / 53F) {
				OnConcludeIntroMusic ();
			}
		}

		if (m_stoppingMenu) {
			m_currentTime += Time.deltaTime;

			if (m_currentTime > 60F / 53F) {
				OnConcludeMusic ();
			}
		}
	}

	public void OnPlayMenuMusic()
	{
		if (m_stoppingMenu)
			OnConcludeMusic ();
		
		if (!m_playingMenu && !m_stoppingIntro) {
			InMenu.TransitionTo (0);
			OutMenuIntro.TransitionTo (0);

			m_BackgroundAudioSource.PlayDelayed (3.95f);
			m_IntroAudioSource.Play ();

			m_stoppingIntro = true;
			m_currentTime = 0;
		}
	}

	public void OnStopMenuMusic()
	{
		if (m_playingMenu || m_stoppingIntro) {
			m_playingMenu = m_stoppingIntro = false;
			OutMenu.TransitionTo (60F/53F);

			m_stoppingMenu = true;
			m_currentTime = 0;
		}
	}

	void OnConcludeIntroMusic()
	{
		m_stoppingIntro = false;
		m_IntroAudioSource.Stop();
	}

	void OnConcludeMusic()
	{
		m_stoppingMenu = false;
		m_BackgroundAudioSource.Stop();
	}

	public void OnPlayClick()
	{
		if (!m_ButtonsAudioSource.isPlaying) 
		{
			m_ButtonsAudioSource.PlayOneShot(ClickSting);
		}
	}

	public void OnPlayHover()
	{
		if (!m_ButtonsAudioSource.isPlaying) 
		{
			m_ButtonsAudioSource.PlayOneShot(HoverSting);
		}
	}
}
