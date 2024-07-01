using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class AWeaponLevelConfig {
    [field:SerializeField]
    public float WeaponDamage { get; set;}
    [field:SerializeField]
    public int WeaponPrice { get; set; }
}

public interface IWeaponConfig
{
    UnityEvent OnCurrentWeaponLevelChange { get; }
    int WeaponId {get;}
    string WeaponName {get; set;}
    WeaponType WeaponType {get; set;}
    List<AWeaponLevelConfig> WeaponLevels {get;}
    string WeaponDescription {get; set;}
    Sprite WeaponIcon {get; set;}
    int CurrentWeaponLevel {get; set;}
    int CurrentUnlockedWeaponLevel {get; set;}
    GameObject WeaponPrefab {get; set;}
    GameObject WeaponDetailPanelPrefab {get; set;}
    IWeaponDetailPanelFactory WeaponDetailPanelFactory { get; }
    IWeaponFactory WeaponFactory { get; }
}
