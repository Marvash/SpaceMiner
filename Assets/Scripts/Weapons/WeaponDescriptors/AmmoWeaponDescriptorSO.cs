using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AmmoWeaponDescriptorSO", menuName = "ScriptableObjects/WeaponDescriptors/AmmoWeaponDescriptorSO", order = 1)]
public class AmmoWeaponDescriptorSO : WeaponDescriptorBaseSO
{
    [field:SerializeField]
    public List<float> MaxAmmoCount {get; set;}

    public AmmoWeaponConfigSO GetDefaultAmmoWeaponConfig()
    {
        AmmoWeaponConfigSO ammoWeaponConfig = CreateInstance<AmmoWeaponConfigSO>();
        ammoWeaponConfig.WeaponDescriptor = this;
        ammoWeaponConfig.CurrentWeaponLevel = 0;
        return ammoWeaponConfig;
    }
}
