using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponDescriptorBaseSO : ScriptableObject, IWeaponDescriptor
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
    public GameObject WeaponPrefab { get; set; }

    [field:SerializeField]
    public Sprite WeaponIcon { get; set; }

    void OnEnable() {
        WeaponId = weaponIdCounter++;
    }

}
