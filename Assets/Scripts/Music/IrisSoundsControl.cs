using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisSoundsControl : CharacterSoundsControl {

	public AudioClip[] CanneClips;
	public AudioClip PushClip;

	private bool m_IsPushPlaying = false;

	protected override void Step()
	{
		base.Step ();

		if (!m_IsPushPlaying) {
			// Iris canne sound
			m_AudioSource [1].PlayOneShot (GetRandomClip (CanneClips));
		}
	}

	public void StartPush()
	{
		if (m_AudioSource [1].clip != PushClip || !m_IsPushPlaying) {
			m_IsPushPlaying = true;
			m_AudioSource [1].loop = true;
			m_AudioSource [1].clip = PushClip;
			m_AudioSource [1].Play ();
		}
	}

	public void StopPush()
	{
		if (m_IsPushPlaying) {
			m_IsPushPlaying = false;
			m_AudioSource [1].Stop ();
			m_AudioSource [1].loop = false;
		}
	}
}
