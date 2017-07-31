using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class PlayerWeapon : WeaponHandlerBase {

    // Update is called once per frame
    void Update() {
        if (ActiveWeapon != null && !GetComponent<Lives>().Dead && GetComponent<Fuel>().Value > 0) {
            if (Input.GetButtonDown("Fire1")) {
                ActiveWeapon.Fire();
            }
            if (Input.GetButtonUp("Fire1")) {
                ActiveWeapon.Release();
            }
        }
    }
}
