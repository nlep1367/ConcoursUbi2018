using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour {

	public float inValue;

	private int _maxWidth;
	private float _oldValue;

	private Tweener _tweener;
	private float valueVariationDuration = 1F;

	// Use this for initialization
	void Start () {
		_maxWidth = (int)this.GetComponent<RectTransform> ().sizeDelta [0];
		_oldValue = 1F;

		_tweener = new Tweener ();
		_tweener.SetEaseType (Tweener.EaseType.Smooth);
	}

	void Update() {
		if (_oldValue != inValue) {
			_tweener.SetBeginValue (_oldValue);
			_tweener.SetEndValue (inValue);
			_tweener.SetDuration (valueVariationDuration);

			_tweener.Start ();

			_oldValue = inValue;
		}

		if (_tweener.IsActive ()) {

			_tweener.Update ();

			RectTransform mask = transform.Find("mask").GetComponent<RectTransform>();
			mask.sizeDelta = new Vector2 (_maxWidth * _tweener.GetCurrentValue (), 100);
		}
	}
}
