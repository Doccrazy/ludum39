using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using Random = UnityEngine.Random;

public class FuelGauge : MonoBehaviour {
	public Fuel Source;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		float value = Source.Value;
		var control = Source.GetComponent<CarController>();
		float speedPerc = Math.Min(control.CurrentSpeed / control.MaxSpeed, 1f);
		float twitch = speedPerc;
		transform.rotation = Quaternion.AngleAxis(55f - 110f * value + Random.Range(-twitch * 1.5f, twitch * 1.5f), Vector3.forward);
	}
}
