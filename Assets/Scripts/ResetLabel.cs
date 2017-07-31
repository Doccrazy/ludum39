using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetLabel : MonoBehaviour {
	public GameObject Player;

	// Update is called once per frame
	void FixedUpdate () {
		var fuel = Player.GetComponent<Fuel>();
		var lives = Player.GetComponent<Lives>();
		var cp = Player.GetComponent<Checkpointing>();
		var resetter = Player.GetComponent<Resetter>();
		GetComponent<Text>().enabled = (resetter.NeedReset || lives.Dead) && cp.FinishPosition < 0;
		if (lives.Dead || lives.Value == 0 && fuel.Value <= 0) {
			GetComponent<Text>().text = "Game over!\nPress Return to try again";
		} else if (fuel.Value <= 0) {
			GetComponent<Text>().text = "Out of fuel!\nPress R to reset";
		}
		else {
			GetComponent<Text>().text = "Press R to reset";
		}
	}
}
