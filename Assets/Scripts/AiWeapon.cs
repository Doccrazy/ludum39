using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class AiWeapon : WeaponHandlerBase {
    private float _startTime;

    // Update is called once per frame
    void FixedUpdate() {
        if (ActiveWeapon != null) {
            if (Random.value < 0.01f) {
                ActiveWeapon.Fire();
                _startTime = Time.time;
            } else if (Random.value < 0.25f && Time.time - _startTime > 0.5f) {
                ActiveWeapon.Release();
            }
        }
    }
}
