using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class AI : MonoBehaviour {
	private CarAIControl _carAiControl;
	public Bezier3D2 Track;
	public GameObject TargetTemplate;
	public float LookAhead = 0.025f;
	private GameObject _target;

	// Use this for initialization
	void Start () {
		_carAiControl = GetComponent<CarAIControl>();
		_target = Instantiate(TargetTemplate);
	}

	// Update is called once per frame
	void FixedUpdate () {
//		t = (t + Time.fixedDeltaTime * 0.02f) % 1f;
//		Target.transform.position = Track.GetPointAt(t);

		float progress = Track.GetProgress(transform.position);
		Track.UpdateTransformTo(_target.transform, progress + LookAhead);
		_carAiControl.SetTarget(_target.transform);

//		if ((Track.Waypoints[nextWaypoint].transform.position - transform.position).sqrMagnitude < 10) {
//			nextWaypoint = (nextWaypoint + 1) % Track.Waypoints.Count;
//		}
//
//		_carAiControl.SetTarget(Track.Waypoints[nextWaypoint].transform);
//		Debug.Log();
	}
}
