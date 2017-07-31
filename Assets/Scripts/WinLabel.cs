using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLabel : MonoBehaviour {
	public Checkpointing Player;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text>().enabled = Player.FinishPosition >= 0;
		string pos;
		switch (Player.FinishPosition) {
			case 0:
				pos = "1st";
				break;
			case 1:
				pos = "2nd";
				break;
			case 2:
				pos = "3rd";
				break;
			default:
				pos = (Player.FinishPosition + 1) + "th";
				break;
		}
		GetComponent<Text>().text = "Congratulations!\nYou finished the race " + pos + "!";
	}
}
