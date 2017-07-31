using UnityEngine;
using Weapons;

public abstract class WeaponHandlerBase : MonoBehaviour {
    protected Weapon ActiveWeapon;

    public void Select(Weapon weapon) {
        Deselect();
        if (weapon != null) {
            weapon.Init();
            ActiveWeapon = weapon;
        }
    }

    private void Deselect() {
        if (ActiveWeapon != null) {
            ActiveWeapon.Deinit();
            ActiveWeapon = null;
        }
    }
}
