using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RocketLauncherArrayConfigSO", menuName = "ScriptableObjects/WeaponConfigs/RocketLauncherArrayConfigSO", order = 1)]
public class RocketLauncherArrayConfigSO : WeaponConfigBaseSO
{
    public override GameObject InstantiateWeapon()
    {
        GameObject rocketLauncherGO = Instantiate(WeaponPrefab);
        RocketLauncherArray rocketLauncherArray = rocketLauncherGO.GetComponent<RocketLauncherArray>();
        rocketLauncherArray.WeaponConfig = this;
        return rocketLauncherGO;
    }
}
