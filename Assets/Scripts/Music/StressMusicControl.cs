using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class StressMusicControl : MonoBehaviour {

	public AudioMixerSnapshot outOfStress;
	public AudioMixerSnapshot inAnxious;
	public AudioMixerSnapshot inStress;
	public AudioMixerSnapshot inPanic;
	public AudioMixerSnapshot inNearDeath;

	public float TransitionInBetweenStates;

	private GameObject m_StressPlayer;

	private AudioSource m_StressAudioSource;

	private float m_currentTime;

	private bool m_playingStress = false;
	private bool m_stopping = false;
	private bool m_isInitialized = false;

	private Fear m_IrisFear;
	private Fear.FearState m_ActualFearState;

	// Use this for initialization
	public void Initialize () 
	{
		m_StressPlayer = transform.Find  ("StressPlayer").gameObject;
		m_StressAudioSource = m_StressPlayer.GetComponent<AudioSource>();
		m_isInitialized = true;
	}

	void Awake()
	{
		if (m_isInitialized) {
			m_IrisFear = GetComponentInParent<InGameUI>().fille.GetComponent<Fear>();
			m_ActualFearState = m_IrisFear.fearState;
			PlayStress(m_ActualFearState);
		}
	}

	// Tick is called by the AmbientMusicControl::Update if necessary
	public void Tick () 
	{	
		if (!m_isInitialized)
			return;
		
		if(m_ActualFearState != m_IrisFear.fearState)
		{
			m_ActualFearState = m_IrisFear.fearState;
			PlayStress(m_ActualFearState);
		}

		if (!m_playingStress)
			return;

		if (m_stopping) {
			m_currentTime += Time.deltaTime;

			if (m_currentTime > TransitionInBetweenStates) {
				OnConcludeStress ();
			} 
		}
	}

	public void OnPlayStress()
	{
		if (!m_playingStress) {
			m_playingStress = true;
			m_StressAudioSource.Play();
		}
	}

	public void PlayStress(Fear.FearState state)
	{
		AudioMixerSnapshot snapToUse;
		switch (state) {
		case Fear.FearState.Anxious:
			snapToUse = inAnxious;
			break;
		case Fear.FearState.Stress:
			snapToUse = inStress;
			break;
		case Fear.FearState.Panic:
			snapToUse = inPanic;
			break;
		case Fear.FearState.NearDeath:
			snapToUse = inNearDeath;
			break;
		default:
		case Fear.FearState.Calm:
			OnStopStress ();
			return;
		}
		OnPlayStress ();
		snapToUse.TransitionTo(TransitionInBetweenStates);
	}

	public void OnStopStress()
	{
		if (m_playingStress && !m_stopping) {
			m_stopping = true;
			outOfStress.TransitionTo (TransitionInBetweenStates);
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
