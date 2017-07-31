using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Lives : MonoBehaviour {
	public int Value = 3;
	private bool _dead;

	public bool Dead {
		get { return _dead; }
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
	}

	public void LoseLife() {
		if (Value == 0) {
			_dead = true;
		}
		Value = Math.Max(0, Value - 1);
	}
}
