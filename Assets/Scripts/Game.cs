using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
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
	private bool _inProgress;
	private List<GameObject> _aiList = new List<GameObject>();

	public bool InProgress {
		get { return _inProgress; }
	}

	// Use this for initialization
	void Start () {
		float t = 0.98f;
		float offset = -3f;
		int color = 0;
		OffTrackDetector ot;
		for (int i = 0; i < 8; i++) {
			GameObject ai = Instantiate(AiPrefab);
			Spawn(ai, t, offset, color);
			_aiList.Add(ai);

			ot = ai.AddComponent<OffTrackDetector>();
			ot.Track = Track;
			ot.FalloffAction += DropFuel;
			var cp = ai.AddComponent<Checkpointing>();
			cp.Game = this;
			cp.Track = Track;
			ai.AddComponent<AiWeapon>();
			ai.AddComponent<Resetter>().Automatic = true;
			ai.GetComponent<CarAIControl>().enabled = false;
			ai.GetComponent<Resetter>().enabled = false;

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
		Player.GetComponent<CarUserControl>().enabled = false;
		Player.GetComponent<Resetter>().enabled = false;

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
		if (FindObjectsOfType<Pickup>().Length >= 15) {
			return;
		}
		var pickupPrefab = Pickups[Random.Range(0, Pickups.Length)];

		for (int attempt = 0; attempt < 10; attempt++) {
			float t = Random.Range(0, 3) / 3f;
			float offset = Random.Range(-2, 2) * 2;
			var spawnPoint = Track.GetPointAt(t, offset);
			if (!Physics.CheckSphere(spawnPoint, 0.5f, 1 << 11)) {
				var pickup = Instantiate(pickupPrefab);
				Track.UpdateTransformTo(pickup.transform, t, offset);
				return;
			}
		}
	}

	public int RegisterFinish(Checkpointing checkpointing) {
		return finishCounter++;
	}

	public void StartRace() {
		_inProgress = true;
		foreach (var ai in _aiList) {
			ai.GetComponent<CarAIControl>().enabled = true;
			ai.GetComponent<Resetter>().enabled = true;
		}
		Player.GetComponent<CarUserControl>().enabled = true;
		Player.GetComponent<Resetter>().enabled = true;
	}
}
