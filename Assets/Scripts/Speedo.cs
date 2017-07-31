using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using Random = UnityEngine.Random;

public class Speedo : MonoBehaviour {
	public CarController Source;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		var control = Source.GetComponent<CarController>();
		float value = Math.Min(control.CurrentSpeed / control.MaxSpeed, 1f);
		float twitch = value;
		transform.rotation = Quaternion.AngleAxis(130f - 260f * value + Random.Range(-twitch * 1.5f, twitch * 1.5f), Vector3.forward);
	}
}
