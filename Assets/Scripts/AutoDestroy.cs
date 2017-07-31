using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {
	public float Time;

	// Use this for initialization
	void Start () {
		StartCoroutine(DestroyAfter(Time));
	}

	// every 2 seconds perform the print()
	private IEnumerator DestroyAfter(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}
}
