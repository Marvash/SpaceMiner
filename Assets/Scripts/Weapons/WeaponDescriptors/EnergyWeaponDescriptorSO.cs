using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnergyWeaponDescriptorSO", menuName = "ScriptableObjects/WeaponDescriptors/EnergyWeaponDescriptorSO", order = 1)]
public class EnergyWeaponDescriptorSO : WeaponDescriptorBaseSO
{
    [field:SerializeField]
    public List<float> EnergyConsumption {get; set;}

    public EnergyWeaponConfigSO GetDefaultEnergyWeaponConfig()
    {
        EnergyWeaponConfigSO energyWeaponConfig = CreateInstance<EnergyWeaponConfigSO>();
        energyWeaponConfig.WeaponDescriptor = this;
        energyWeaponConfig.CurrentWeaponLevel = 0;
        return energyWeaponConfig;
    }
}
