using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpointing : MonoBehaviour {
	public Bezier3D2 Track;
	public Game Game;
	private int _lastCheckpointIdx = -1;
	private GameObject _lastCheckpoint;
	private bool _afterLast;
	private int _lapCounter;
	private int _finishPosition = -1;

	public int LapCounter {
		get { return _lapCounter; }
	}

	public int LastCheckpointIdx {
		get { return _lastCheckpointIdx; }
	}

	public GameObject LastCheckpoint {
		get { return _lastCheckpoint; }
	}

	public int FinishPosition {
		get { return _finishPosition; }
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void FixedUpdate () {
	}

	public void TouchCp(Checkpoint cp) {
		if (cp.Index == _lastCheckpointIdx + 1 || _afterLast && cp.Index == 0) {
			if (_afterLast) {
				_lapCounter++;
				if (_lapCounter == Game.TotalLaps) {
					_finishPosition = Game.RegisterFinish(this);
				}
			}
			_lastCheckpointIdx = cp.Index;
			_lastCheckpoint = cp.gameObject;
			_afterLast = cp.Last;
		}
	}
}
