using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class StressMusicControl : MonoBehaviour {
	// TO DO : 
	//	-	Ajuster transitions

	public bool ToStart = false;
	public bool ToStop = false;

	public AudioMixerSnapshot outOfStress;
	public AudioMixerSnapshot inStress;
//	public AudioClip[] stings;
//	public AudioSource stingSource;
	public float bpm = 53;

	AudioSource m_StressAudioSource;

	private float m_TransitionIn;
	private float m_TransitionOut;
	private float m_QuarterNote;

	private float m_currentTime;

	private bool m_playingStress = false;
	private bool m_stopping = false;

	// Use this for initialization
	void Start () 
	{
		m_QuarterNote = 60 / bpm;
		m_TransitionIn = m_QuarterNote * 4;
		m_TransitionOut = m_QuarterNote * 16;

		m_StressAudioSource = GetComponents<AudioSource>()[1];

	}

	// Update is called once per frame
	void Update () 
	{
		// Start scared effect
		if (ToStart) {
			OnPlayStress ();
			ToStart = false;
		}

		// Stop scared effect
		if (ToStop) {
			OnStopStress ();
			ToStop = false;
		}

		if (!m_playingStress)
			return;

		if (m_stopping) {
			m_currentTime += Time.deltaTime;

			if (m_currentTime > m_TransitionOut) {
				OnConcludeStress ();
			} 
		}
	}

	void OnPlayStress()
	{
		if (!m_playingStress) {
			m_playingStress = true;
			m_StressAudioSource.Play();
			inStress.TransitionTo(m_TransitionIn);
			PlaySting();
		}
	}

	void OnStopStress()
	{
		if (m_playingStress && !m_stopping) {
			outOfStress.TransitionTo (m_TransitionOut);
			m_currentTime = 0;
		}
	}

	void OnConcludeStress()
	{
		m_playingStress = false;
		m_stopping = false;

		m_StressAudioSource.Stop();
	}

	void PlaySting()
	{
//		int randClip = Random.Range (0, stings.Length);
//		stingSource.clip = stings[randClip];
//		stingSource.Play();
	}
}
