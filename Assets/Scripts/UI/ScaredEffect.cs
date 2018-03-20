using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaredEffect : MonoBehaviour {
	// Public variables to test ScaredEffect in runtime
	// ToStart and ToStop can be seen as events to control the scared effect.
	public bool ToStart;
	public bool ToStop;
	public Color MaskColor;

	private bool m_afficherScaredEffect = false;
	private bool m_isAborting = false;

	private float m_transitionIn = 1f;	
	private float m_transitionOut = 3f;
	private float m_halfBounceTransition = 0.75f;

	private Tweener m_transitionnerIn;
	private Tweener m_transitionnerOut;
	private Tweener m_transitionnerHalfBounce;

	private GameObject m_centeredMask;

	// Use this for initialization
	void Start () {
		m_centeredMask = transform.Find  ("mask").gameObject;

		m_transitionnerIn = new Tweener (Tweener.EaseType.Smooth, 1f, 0f, m_transitionIn);
		m_transitionnerOut = new Tweener (Tweener.EaseType.Smooth, 0f, 1f, m_transitionOut);
		m_transitionnerHalfBounce = new Tweener (Tweener.EaseType.Smooth, 1f, 1.25f, m_halfBounceTransition);

		m_transitionnerHalfBounce.IsRepeating = true;

		m_centeredMask.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Start scared effect
		if (ToStart) {
			if (!m_afficherScaredEffect) {
				StartScaredEffect ();
			}
			ToStart = false;
		}

		// If the effect is not shown, no need to update it
		if (!m_afficherScaredEffect)
			return;

		// Stop scared effect
		if (ToStop) {
			if (!m_isAborting) {
				AbortScaredEffect ();
			}
			ToStop = false;
		}

		if (m_transitionnerIn.IsActive ()) {

			m_transitionnerIn.Update ();

			float currentValue = m_transitionnerIn.GetCurrentValue ();

			// Animate scale and color
			m_centeredMask.transform.GetComponent<RectTransform>().localScale = new Vector2 (currentValue + 1f, currentValue + 1f);
			m_centeredMask.transform.GetComponent<Image>().color = new Color(MaskColor.r, MaskColor.g, MaskColor.b, 1f - currentValue);

			if (!m_transitionnerIn.IsActive ()) {
				m_transitionnerHalfBounce.SetBeginValue (1f);
				m_transitionnerHalfBounce.SetEndValue (1.25f);
				m_transitionnerHalfBounce.Start ();
			}
		}

		if (m_transitionnerHalfBounce.IsActive ()) {

			m_transitionnerHalfBounce.Update ();

			float currentValue = m_transitionnerHalfBounce.GetCurrentValue ();

			// Animate scale and color
			m_centeredMask.transform.GetComponent<RectTransform>().localScale = new Vector2 (currentValue, currentValue);

			if (!m_transitionnerHalfBounce.IsActive ()) {
				m_transitionnerOut.Start ();
			}
		}


		if (m_transitionnerOut.IsActive ()) {

			m_transitionnerOut.Update ();

			float currentValue = m_transitionnerOut.GetCurrentValue ();

			m_centeredMask.transform.GetComponent<RectTransform>().localScale = new Vector2 (currentValue + 1f, currentValue + 1f);
			m_centeredMask.transform.GetComponent<Image> ().color = new Color (MaskColor.r, MaskColor.g, MaskColor.b, 1f - currentValue);

			if (!m_transitionnerOut.IsActive ()) {
				ConcludeScaredEffect ();
			}
		}
	}

	void StartScaredEffect() {
		if (!m_afficherScaredEffect) {
			m_afficherScaredEffect = true;
			m_centeredMask.SetActive(true);
			m_transitionnerIn.Start ();
		}
	}

	void AbortScaredEffect() {
		if (m_afficherScaredEffect) {
			m_isAborting = true;
			m_transitionnerHalfBounce.StopRepeating = true ;
		}
	}

	void ConcludeScaredEffect() {
		m_afficherScaredEffect = false;
		m_isAborting = false;
		m_centeredMask.SetActive(false);
	}
}
