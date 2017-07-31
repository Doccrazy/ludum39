using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour {
	public Game Game;
	public Checkpointing Player;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = "Lap " + Math.Min(Player.LapCounter + 1, Game.TotalLaps) + " / " + Game.TotalLaps;
	}
}
