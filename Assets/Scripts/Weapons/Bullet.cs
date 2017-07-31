using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public GameObject DieEffectPrefab;

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.GetComponentInParent<Rigidbody>()) {
			Debug.Log(other);
			other.gameObject.GetComponentInParent<Rigidbody>().AddForceAtPosition(GetComponent<Rigidbody>().velocity.normalized * 5000f, other.contacts[0].point);
		}
		if (DieEffectPrefab != null) {
			Instantiate(DieEffectPrefab, transform.position, transform.rotation);
		}
		Destroy(gameObject);
	}

}
