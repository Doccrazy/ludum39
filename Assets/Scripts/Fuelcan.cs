using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelcan : MonoBehaviour {
	public AudioClip Sound;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponentInParent<Fuel>()) {
			other.gameObject.GetComponentInParent<Fuel>().Refuel();
			AudioSource.PlayClipAtPoint(Sound, transform.position);
			Destroy(gameObject);
		}
	}
}
