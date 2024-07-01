using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    Energy,
    Kinetic
}

public abstract class AWeapon : MonoBehaviour
{
    public GameObject PlayershipGO { get; set;}
    public abstract void ShootBegin();

    public abstract void ShootInterrupt();
    public abstract void ShootEnd();

    public abstract bool IsActive();

    public abstract WeaponConfigBaseSO WeaponConfig { get; }

    public abstract void UpdateWeaponConfig();
}
