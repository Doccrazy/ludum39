using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	public int Index;
	public bool Last;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponentInParent<Checkpointing>()) {
			other.GetComponentInParent<Checkpointing>().TouchCp(this);
			if (other.GetComponentInParent<Lives>()) {
				GetComponent<AudioSource>().Play();
			}
		}
	}
}
