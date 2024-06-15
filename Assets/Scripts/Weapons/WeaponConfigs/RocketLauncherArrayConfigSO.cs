using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RocketLauncherArrayLevelConfig : AWeaponLevelConfig {
    [field:SerializeField]
    public int MaxAmmo { get; set; }
    [field:SerializeField]
    public float ProjectileSpeed { get; set; }
    [field:SerializeField]
    public float RocketShotCooldown { get; set;}
}

[CreateAssetMenu(fileName = "RocketLauncherArrayConfigSO", menuName = "ScriptableObjects/WeaponConfigs/RocketLauncherArrayConfigSO", order = 1)]
public class RocketLauncherArrayConfigSO : WeaponConfigBaseSO
{
    public List<RocketLauncherArrayLevelConfig> RocketLauncherArrayLevelConfigs;

    public override List<AWeaponLevelConfig> WeaponLevels { get => RocketLauncherArrayLevelConfigs.Cast<AWeaponLevelConfig>().ToList(); }

    protected override void OnEnable()
    {
        base.OnEnable();
        WeaponDetailPanelFactory = new RocketLauncherArrayDetailPanelFactory(WeaponDetailPanelPrefab, this);
        WeaponFactory = new RocketLauncherArrayWeaponFactory(WeaponPrefab, this);
    }
}
