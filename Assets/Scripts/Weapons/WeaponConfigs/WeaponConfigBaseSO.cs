using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponConfigBaseSO : ScriptableObject, IWeaponConfig
{
    private static int weaponIdCounter;
    public int WeaponId { get; private set; }
    [field:SerializeField]
    public string WeaponName { get; set;}

    [field:SerializeField]
    public WeaponAmmoType WeaponAmmoType { get; set; }
    
    [field:SerializeField]
    public List<float> WeaponDamage { get; set; }
    
    [field:SerializeField]
    public string WeaponDescription { get; set; }

    [field:SerializeField]
    protected GameObject WeaponPrefab { get; set; }

    [field:SerializeField]
    public Sprite WeaponIcon { get; set; }
    public int CurrentWeaponLevel { get; set; }

    public abstract GameObject InstantiateWeapon();

    void OnEnable() {
        WeaponId = weaponIdCounter++;
    }
}
