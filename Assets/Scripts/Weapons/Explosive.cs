using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour {
	public GameObject DieEffectPrefab;
	public float Force = 50000f;
	public float Radius = 5f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnCollisionEnter(Collision other) {
		var hits = Physics.OverlapSphere(transform.position, Radius, 1 << 8 | 1 << 10);
		foreach (var hit in hits) {
			if (hit.GetComponentInParent<Rigidbody>()) {
				hit.GetComponentInParent<Rigidbody>().AddExplosionForce(Force, transform.position, Radius, 0.5f);
			}
		}
		Instantiate(DieEffectPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
