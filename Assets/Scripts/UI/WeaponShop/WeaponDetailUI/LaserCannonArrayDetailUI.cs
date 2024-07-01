using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaserCannonArrayDetailUI : AWeaponDetailUI
{
    [SerializeField]
    protected TextMeshProUGUI NumCannonsText;
    [SerializeField]
    protected TextMeshProUGUI EnergyCostText;
    [SerializeField]
    protected TextMeshProUGUI ProjectileSpeedText;
    [SerializeField]
    protected TextMeshProUGUI FireRateText;

    public void PopulateDetailPanel(LaserCannonArrayConfigSO config, int weaponLevel) {
        base.PopulateDetailPanel(config, weaponLevel);
        int levelIndex = 0;
        if(weaponLevel > 0) {
            levelIndex = weaponLevel-1;
        }
        LaserCannonArrayLevelConfig levelConfig = config.LaserCannonArrayLevelConfigs[levelIndex];
        NumCannonsText.text = levelConfig.NumCannons.ToString();
        EnergyCostText.text = Mathf.FloorToInt(levelConfig.EnergyCost).ToString();
        ProjectileSpeedText.text = Mathf.FloorToInt(levelConfig.ProjectileSpeed).ToString();
        FireRateText.text = Mathf.FloorToInt(1.0f / levelConfig.LaserShotCooldown * 60.0f).ToString();
    }
}
