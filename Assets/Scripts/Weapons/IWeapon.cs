using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAmmoType {
    Energy,
    Ammo
}

public interface IWeapon
{
    public WeaponConfigBaseSO WeaponConfig { get; }
    public GameObject PlayershipGO { get; set;}
    public void ShootBegin();

    public void ShootInterrupt();
    public void ShootEnd();

    public bool IsActive();
    
    public void InitWeapon(WeaponInitializer initializer);
}
