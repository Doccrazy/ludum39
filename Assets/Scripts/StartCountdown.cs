using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountdown : MonoBehaviour {
	public Game Game;

	// Use this for initialization
	void Start () {
		StartCoroutine(SetText("3", Color.red, 2f));
		StartCoroutine(SetText("2", Color.red, 3f));
		StartCoroutine(SetText("1", Color.yellow, 4f));
		StartCoroutine(SetText("Go!", Color.green, 5f));
		StartCoroutine(StartRace(5f));
		StartCoroutine(SetText("", Color.green, 6f));
	}

	private IEnumerator SetText(string text, Color color, float delayTime) {
		yield return new WaitForSeconds(delayTime);
		GetComponent<Text>().text = text;
		GetComponent<Text>().color = color;
	}

	private IEnumerator StartRace(float delayTime) {
		yield return new WaitForSeconds(delayTime);
		Game.StartRace();
	}
}
