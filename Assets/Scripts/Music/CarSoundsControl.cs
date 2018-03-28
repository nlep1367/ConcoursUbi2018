using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundsControl : MonoBehaviour {
	public AudioClip AccNoise;
	public AudioClip IdleNoise;
	public AudioClip LightStopNoise;
	public AudioClip RunningNoise;
	public AudioClip SuddenBrakeNoise;

	public AudioMixerSnapshot InGame;
	public AudioMixerSnapshot[] clipTransitions;

	public bool ToPlayInitSpooked = false;
	public bool ToPlaySpooked = false;

	private AudioSource m_AudioSource;
	private m_AudioSource m_CarSounds;

	// Use this for initialization
	void Start () {
		
	}

	void Awake() {
		m_AudioSource = transform.Find  ("CarSounds").gameObject.GetComponents<AudioSource>()[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartMoving() {
	}

	public void StopMoving() {
	}
}
