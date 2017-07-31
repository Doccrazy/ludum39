using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resetter : MonoBehaviour {
	[SerializeField] private float m_WaitTime = 3f;           // time to wait before self righting
	[SerializeField] private float m_VelocityThreshold = 1f;  // the velocity below which the car is considered stationary for self-righting
	public bool Automatic;

	private float m_LastOkTime; // the last time that the car was in an OK state
	private Rigidbody m_Rigidbody;
	private bool m_needReset;

	public bool NeedReset {
		get { return m_needReset; }
	}

	// Update is called once per frame
	void Update () {
		if ((!Automatic && Input.GetButtonDown("Fire3")) || (Automatic && m_needReset)) {
			m_LastOkTime = Time.time;
			GetComponent<OffTrackDetector>().RespawnAtCheckpoint(GetComponent<Fuel>() && GetComponent<Fuel>().Value <= 0);
		}
		if (Input.GetButtonDown("Submit")) {
			SceneManager.LoadScene("Main");
		}
	}

	// Use this for initialization
	private void Start() {
		m_Rigidbody = GetComponent<Rigidbody>();
	}


	// Update is called once per frame
	void FixedUpdate () {
		// is the car is the right way up
		if (m_Rigidbody.velocity.magnitude > m_VelocityThreshold)
		{
			m_LastOkTime = Time.time;
		}

		m_needReset = Time.time > m_LastOkTime + m_WaitTime || (GetComponent<Fuel>() && GetComponent<Fuel>().Value <= 0);
	}
}
