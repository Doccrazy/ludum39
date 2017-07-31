using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelWarning : MonoBehaviour {
	public Fuel Fuel;

	// Use this for initialization
	void Start () {
		InvokeRepeating("Blink", 0.5f, 0.5f);
	}

	// Update is called once per frame
	void Update () {
		if (Fuel.Value <= 0) {
			GetComponent<Text>().text = "Out of fuel";
		} else if (Fuel.Value < 0.2f) {
			GetComponent<Text>().text = "Low fuel";
		} else {
			GetComponent<Text>().text = "";
		}
	}

	private void Blink() {
		if (Fuel.Value <= 0) {
			GetComponent<Text>().enabled = !GetComponent<Text>().enabled;
		} else {
			GetComponent<Text>().enabled = true;
		}
	}
}
