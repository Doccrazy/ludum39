using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour {
	public AudioClip[] Sounds;

	// Use this for initialization
	void Start () {
		int s = Random.Range(0, Sounds.Length);
		AudioSource.PlayClipAtPoint(Sounds[s], transform.position);
	}

	// Update is called once per frame
	void Update () {

	}
}
