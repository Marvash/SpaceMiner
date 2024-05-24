using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAmmoType {
    Energy,
    Ammo
}

public abstract class IWeapon : MonoBehaviour
{
    public WeaponAmmoType WeaponAmmoType { get; private set; }
    public GameObject PlayershipGO { get; set;}
    public abstract void ShootBegin();

    public abstract void ShootInterrupt();
    public abstract void ShootEnd();

    public abstract bool IsActive();
}
