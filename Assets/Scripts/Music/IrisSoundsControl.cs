using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisSoundsControl : CharacterSoundsControl {

	public AudioClip[] CanneClips;

	protected override void Step()
	{
		base.Step ();

		// Iris canne sound
		m_AudioSource [1].PlayOneShot (GetRandomClip (CanneClips));
	}
}
