using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponDescriptor
{
    int WeaponId {get;}
    string WeaponName {get; set;}
    WeaponAmmoType WeaponAmmoType {get; set;}
    List<float> WeaponDamage {get; set;}
    string WeaponDescription {get; set;}
    GameObject WeaponPrefab {get; set;}
    Sprite WeaponIcon {get; set;}

}
