using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatsUI : MonoBehaviour {

	public GameObject HeartImage;

	public bool IncreaseStress = false;

	private const int CALM_BPM = 65;
	private const int ANXIOUS_BPM = 85;
	private const int STRESS_BPM = 115;
	private const int PANIC_BPM = 155;
	private const int NEARDEATH_BPM = 185;

	private const float CALM_SCALE = 1.125f;
	private const float ANXIOUS_SCALE = 1.25f;
	private const float STRESS_SCALE = 1.50f;
	private const float PANIC_SCALE = 1.75f;
	private const float NEARDEATH_SCALE = 2.0f;

	private bool m_isActive = false;
	private uint m_rateIndex = 0;

	private float m_minScale;
	private float m_maxScale;
	private float m_currentTime;
	private float m_currentValue;
	private float m_duration; // In seconds

	private Fear.FearState m_ActualFearState;

	// Use this for initialization
	void Start () {
		m_minScale = 1.0f;
		m_maxScale = CALM_SCALE;
		m_isActive = true;
		m_currentTime = 0;
		m_currentValue = m_minScale;
		m_duration = 60.0f / CALM_BPM;
	}

	void Awake()
	{
		HeartImage.transform.GetComponent<RectTransform>().localScale = new Vector2 (m_minScale, m_minScale);
	}

	public void Pause() {
		m_isActive = false;
	}

	public void Resume() {
		m_isActive = true;
	}

	// Update is called once per frame
	void Update () {
		
		// Start scared effect
		if (IncreaseStress) 
		{
			++m_rateIndex;
			if (m_rateIndex > 4)
				m_rateIndex = 0;
			
			ChangeRate (m_rateIndex);
			IncreaseStress = false;
		}

		if (!m_isActive)
			return;

		m_currentTime += Time.deltaTime ;

		if (m_currentTime <= m_duration) {
			// Interpolate
			float t = m_currentTime * 2 / m_duration;

			if (m_currentTime * 2 < m_duration) {
				// Bounce In

				if(t < 0.5f)
				{
					t = (t == 0.0f) ? t : 0.5f * (float)Math.Pow(2, (20 * t) - 10);
				}
				else
				{
					t = (t == 1.0f) ? t : -0.5f * (float)Math.Pow(2, (-20 * t) + 10) + 1;
				}

				m_currentValue = (m_maxScale - m_minScale) * t + m_minScale;
			} else {
				// Bounce Out
				t = t - 1.0f;
				t = (t == 0.0f) ? t : (float)Math.Pow(2, 10 * (t - 1));

				m_currentValue = (m_minScale - m_maxScale) * t + m_maxScale;
			}
			HeartImage.transform.GetComponent<RectTransform>().localScale = new Vector2 (m_currentValue, m_currentValue);
		} else {
			HeartImage.transform.GetComponent<RectTransform>().localScale = new Vector2 (m_minScale, m_minScale);
			m_currentTime = 0;
		}
	}

	public void ChangeRate(Fear.FearState state)
	{
		float bpm, maxScale;

		switch (state) {
		case Fear.FearState.Anxious:
			bpm = ANXIOUS_BPM;
			maxScale = ANXIOUS_SCALE;
			break;
		case Fear.FearState.Stress:
			bpm = STRESS_BPM;
			maxScale = STRESS_SCALE;
			break;
		case Fear.FearState.Panic:
			bpm = PANIC_BPM;
			maxScale = PANIC_SCALE;
			break;
		case Fear.FearState.NearDeath:
			bpm = NEARDEATH_BPM;
			maxScale = NEARDEATH_SCALE;
			break;
		case Fear.FearState.Calm:
		default:
			bpm = CALM_BPM;
			maxScale = CALM_SCALE;
			break;
		}

		m_duration = 60.0f / bpm;
		m_maxScale = maxScale;
	}	

	// For testing
	public void ChangeRate(uint index)
	{
		float bpm, maxScale;

		switch (index) {
		case 1:
			bpm = ANXIOUS_BPM;
			maxScale = ANXIOUS_SCALE;
			break;
		case 2:
			bpm = STRESS_BPM;
			maxScale = STRESS_SCALE;
			break;
		case 3:
			bpm = PANIC_BPM;
			maxScale = PANIC_SCALE;
			break;
		case 4:
			bpm = NEARDEATH_BPM;
			maxScale = NEARDEATH_SCALE;
			break;
		case 0:
		default:
			bpm = CALM_BPM;
			maxScale = CALM_SCALE;
			break;
		}

		m_duration = 60.0f / bpm;
		m_maxScale = maxScale;
	}
}
