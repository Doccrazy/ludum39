using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Weapons;

public class Pickup : MonoBehaviour {
	public AudioClip Sound;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter(Collider other) {
		var weapon = GetComponent<Weapon>();
		if (other.gameObject.GetComponentInParent<WeaponHandlerBase>()) {
			var root = other.gameObject.transform.root.gameObject;

			foreach (var w in root.GetComponents<Weapon>()) {
				Destroy(w);
			}

			var copy = root.AddComponent(weapon.GetType());
			System.Reflection.FieldInfo[] fields = weapon.GetType().GetFields();
			foreach (System.Reflection.FieldInfo field in fields)
			{
				field.SetValue(copy, field.GetValue(weapon));
			}
			root.GetComponent<WeaponHandlerBase>().Select(copy as Weapon);
			AudioSource.PlayClipAtPoint(Sound, transform.position);
			Destroy(gameObject);
		}
	}
}
