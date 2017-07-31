using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTrackDetector : MonoBehaviour {
	public Bezier3D2 Track;
	public Action<Vector3, Quaternion> FalloffAction;
	private Vector3 lastGoodPos;
	private Quaternion lastGoodRot;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 closestPoint = Track.GetClosestPoint(transform.position);
		float dist = (transform.position - closestPoint).magnitude;
		float distY = transform.position.y - closestPoint.y;
		//Debug.Log(dist + ", y = " + distY);
		if (dist < 3.5f && distY > -0.5f) {
			lastGoodPos = transform.position;
			lastGoodRot = transform.rotation;
		} else if (dist > 6f || distY < -0.5f) {
			if (FalloffAction != null) {
				FalloffAction(lastGoodPos, lastGoodRot);
			}
			RespawnAtCheckpoint(true);
		}
	}

	public void RespawnAtCheckpoint(bool killed) {
		if (GetComponent<Lives>() && GetComponent<Lives>().Dead) {
			return;
		}
		if (killed) {
			if (GetComponent<Lives>()) {
				GetComponent<Lives>().LoseLife();
				if (GetComponent<Lives>().Dead) {
					return;
				}
			}
			if (GetComponent<Fuel>()) {
				GetComponent<Fuel>().RefuelToHalf();
			}
		}

		var cp = GetComponent<Checkpointing>();
		float t;
		if (cp && cp.LastCheckpoint) {
			t = Track.GetProgress(cp.LastCheckpoint.transform.position);
		}
		else {
			t = Track.GetProgress(lastGoodPos);
		}
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		Track.UpdateTransformTo(transform, t, UnityEngine.Random.Range(-3f, 3f));
		transform.position = transform.position + Vector3.up * 1f;
	}
}
