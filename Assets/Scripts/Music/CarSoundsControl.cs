using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSoundsControl : MonoBehaviour {
	public AudioMixerSnapshot RunSnap;
	public AudioMixerSnapshot IdleSnap;
	public AudioMixerSnapshot BrakeSnap;
	public AudioMixerSnapshot RestartSnap;

	public bool ToStartMoving = false;
	public bool ToBrake = false;

	private AudioSource[] m_AudioSources;

	private bool m_IsAccelerating = false;
	private bool m_IsBraking = false;
	private bool m_IsRunning = false;
	private bool m_IsIdle = false;

	private float m_BrakeFadeOutRunTransition = 0.5f;
	private float m_AccFadeOutIdleTransition = 1.5f;

	private float m_CurrentTime;

	// Use this for initialization
	void Start () {
		m_AudioSources = transform.Find  ("CarSounds").gameObject.GetComponents<AudioSource>();

		RunSnap.TransitionTo (0);
		m_IsRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (ToStartMoving) 
		{
			StartMoving ();
			ToStartMoving = false;
		}

		if (ToBrake) 
		{
			Brake ();
			ToBrake = false;
		}

		if (!(m_IsAccelerating
		    || m_IsBraking
		    || m_IsRunning
		    || m_IsIdle))
			return;


		if (m_IsBraking) 
		{
			m_CurrentTime += Time.deltaTime;

			if (m_CurrentTime > m_AudioSources[0].clip.length) 
			{
				ConcludeBraking ();
			} 
		}

		if (m_IsAccelerating) 
		{
			m_CurrentTime += Time.deltaTime;

			if (m_CurrentTime > m_AudioSources[2].clip.length) 
			{
				ConcludeStart ();
			} 
		}
	}

	public void StartMoving() {
		if (m_IsIdle || m_IsBraking) {
			m_AudioSources[2].Play();	// Acc
			RestartSnap.TransitionTo (m_AccFadeOutIdleTransition);

			m_IsIdle = m_IsBraking = false;
			m_IsAccelerating = true;
			m_CurrentTime = 0;
		}
	}

	void ConcludeStart() {
		// Start Run loop
		m_AudioSources[3].Play();
		m_AudioSources[1].Stop();
		RunSnap.TransitionTo (0);
		m_IsAccelerating = false;
		m_IsRunning = true;
	}

	public void Brake() {
		if (m_IsRunning || m_IsAccelerating) {
			m_AudioSources[0].Play();	// Brake
			BrakeSnap.TransitionTo (m_BrakeFadeOutRunTransition);

			m_IsRunning = m_IsAccelerating = false;
			m_IsBraking = true;
			m_CurrentTime = 0;
		}
	}

	void ConcludeBraking() {
		m_AudioSources[1].Play();	// Idle
		m_AudioSources[3].Stop();
		IdleSnap.TransitionTo (0);
		m_IsBraking = false;
		m_IsIdle = true;
	}
}
