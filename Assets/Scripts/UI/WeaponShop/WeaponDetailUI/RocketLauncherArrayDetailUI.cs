using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RocketLauncherArrayDetailUI : AWeaponDetailUI
{
    [SerializeField]
    protected TextMeshProUGUI MaxAmmoText;

    public void PopulateDetailPanel(RocketLauncherArrayConfigSO config, int weaponLevel) {
        base.PopulateDetailPanel(config, weaponLevel);
        int levelIndex = 0;
        if(weaponLevel > 0) {
            levelIndex = weaponLevel-1;
        }
        RocketLauncherArrayLevelConfig levelConfig = config.RocketLauncherArrayLevelConfigs[levelIndex];
        MaxAmmoText.text = levelConfig.MaxAmmo.ToString();
    }
}
