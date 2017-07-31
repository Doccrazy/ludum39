using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Fuel : MonoBehaviour {
	private float _value = 1f;
	public float LossPerSec = 0.5f;

	public float Value {
		get { return _value; }
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		var control = GetComponent<CarController>();
		float speedPerc = Math.Min(control.CurrentSpeed / control.MaxSpeed, 1f);
		_value = Math.Max(0, Value - Time.fixedDeltaTime * LossPerSec * speedPerc);
	}

	public void Refuel() {
		_value = Math.Min(Value + 0.2f, 1f);
	}

	public void RefuelToHalf() {
		_value = Math.Max(Value, 0.5f);
	}

	public void Use(float amount) {
		_value = Math.Max(Value - amount, 0f);
	}
}
