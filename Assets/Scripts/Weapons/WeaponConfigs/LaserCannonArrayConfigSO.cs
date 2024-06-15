using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LaserCannonArrayLevelConfig : AWeaponLevelConfig {
    [field:SerializeField]
    public int NumCannons { get; set; }
    [field:SerializeField]
    public int EnergyCost { get; set;}
    [field:SerializeField]
    public float ProjectileSpeed { get; set; }
    [field:SerializeField]
    public float LaserShotCooldown { get; set;}
}

[CreateAssetMenu(fileName = "LaserCannonArrayConfigSO", menuName = "ScriptableObjects/WeaponConfigs/LaserCannonArrayConfigSO", order = 1)]
public class LaserCannonArrayConfigSO : WeaponConfigBaseSO
{
    public List<LaserCannonArrayLevelConfig> LaserCannonArrayLevelConfigs;

    public override List<AWeaponLevelConfig> WeaponLevels { get => LaserCannonArrayLevelConfigs.Cast<AWeaponLevelConfig>().ToList(); }

    protected override void OnEnable()
    {
        base.OnEnable();
        WeaponDetailPanelFactory = new LaserCannonArrayDetailPanelFactory(WeaponDetailPanelPrefab, this);
        WeaponFactory = new LaserCannonArrayWeaponFactory(WeaponPrefab, this);
    }
}
