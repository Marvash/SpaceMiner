using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponConfig
{
    int WeaponId {get;}
    string WeaponName {get; set;}
    WeaponAmmoType WeaponAmmoType {get; set;}
    List<float> WeaponDamage {get; set;}
    string WeaponDescription {get; set;}
    Sprite WeaponIcon {get; set;}
    int CurrentWeaponLevel {get; set;}
    GameObject InstantiateWeapon();
}
