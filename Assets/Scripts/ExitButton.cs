using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour {
	public Checkpointing Player;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetComponent<Button>().enabled = Player.FinishPosition >= 0;
		GetComponent<Image>().enabled = Player.FinishPosition >= 0;
		GetComponentInChildren<Text>().enabled = Player.FinishPosition >= 0;
		Cursor.visible = Player.FinishPosition >= 0;
	}

	public void Quit() {
		Application.Quit();
	}
}
