using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesLabel : MonoBehaviour {
	public Lives Lives;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Text>().text = "x" + Lives.Value;
	}
}
