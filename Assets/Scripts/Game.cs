using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour {
	public Bezier3D2 Track;
	public GameObject Player;
	public GameObject AiPrefab;
	public Material[] CarColors;
	public GameObject FuelPrefab;
	public GameObject[] Pickups;
	public int TotalLaps;
	private int finishCounter;

	// Use this for initialization
	void Start () {
		float t = 0.98f;
		float offset = -3f;
		int color = 0;
		OffTrackDetector ot;
		for (int i = 0; i < 8; i++) {
			GameObject ai = Instantiate(AiPrefab);
			Spawn(ai, t, offset, color);

			ot = ai.AddComponent<OffTrackDetector>();
			ot.Track = Track;
			ot.FalloffAction += DropFuel;
			var cp = ai.AddComponent<Checkpointing>();
			cp.Game = this;
			cp.Track = Track;
			ai.AddComponent<AiWeapon>();
			ai.AddComponent<Resetter>().Automatic = true;

			t -= 0.01f;
			offset += 2f;
			if (offset >= 4f) {
				offset = -3f;
			}
			color = (color + 1) % CarColors.Length;
		}
		Spawn(Player, t, offset, 0);
		ot = Player.AddComponent<OffTrackDetector>();
		ot.Track = Track;

		for (int i = 0; i < 12; i++) {
			SpawnRandomPickup();
		}
		InvokeRepeating("SpawnRandomPickup", 0f, 10f);
	}

	private void DropFuel(Vector3 pos, Quaternion rot) {
		Instantiate(FuelPrefab, pos, rot);
	}

	private void Spawn(GameObject car, float t, float offset, int color = -1) {
		if (car.GetComponent<AI>() != null) {
			car.GetComponent<AI>().Track = Track;
		}
		Track.UpdateTransformTo(car.transform, t, offset);
		car.transform.position = car.transform.position + Vector3.up * 1f;
		if (color >= 0) {
			car.transform.Find("SkyCar/car").GetComponent<MeshRenderer>().material = CarColors[color];
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
	}

	private void SpawnRandomPickup() {
		if (FindObjectsOfType<Pickup>().Length >= 12) {
			return;
		}
		var pickupPrefab = Pickups[Random.Range(0, Pickups.Length)];
		var pickup = Instantiate(pickupPrefab);
		float t = Random.Range(0, 3) / 3f;
		Track.UpdateTransformTo(pickup.transform, t, Random.Range(-4f, 4f));
	}

	public int RegisterFinish(Checkpointing checkpointing) {
		return finishCounter++;
	}
}
