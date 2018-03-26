using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class StressMusicControl : MonoBehaviour {

	public AudioMixerSnapshot outOfStress;
	public AudioMixerSnapshot inStress;

	public float TransitionIn;
	public float TransitionOut;

	private GameObject m_StressPlayer;

	private AudioSource m_StressAudioSource;

	private float m_currentTime;

	private bool m_playingStress = false;
	private bool m_stopping = false;

	// Use this for initialization
	public void Initialize () 
	{
		m_StressPlayer = transform.Find  ("StressPlayer").gameObject;
		m_StressAudioSource = m_StressPlayer.GetComponent<AudioSource>();
	}

	// Tick is called by the AmbientMusicControl::Update if necessary
	public void Tick () 
	{
		if (!m_playingStress)
			return;

		if (m_stopping) {
			m_currentTime += Time.deltaTime;

			if (m_currentTime > TransitionOut) {
				OnConcludeStress ();
			} 
		}
	}

	public void OnPlayStress()
	{
		if (!m_playingStress) {
			m_playingStress = true;
			m_StressAudioSource.Play();
			inStress.TransitionTo(TransitionIn);
		}
	}

	public void OnStopStress()
	{
		if (m_playingStress && !m_stopping) {
			m_stopping = true;
			outOfStress.TransitionTo (TransitionOut);
			m_currentTime = 0;
		}
	}

	void OnConcludeStress()
	{
		m_playingStress = false;
		m_stopping = false;

		m_StressAudioSource.Stop();
	}
}
