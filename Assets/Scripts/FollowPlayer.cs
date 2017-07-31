using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	public GameObject Player;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	private void LateUpdate() {
		transform.position = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
	}
}
