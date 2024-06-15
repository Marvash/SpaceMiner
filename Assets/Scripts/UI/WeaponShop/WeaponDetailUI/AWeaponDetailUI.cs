using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class AWeaponDetailUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI WeaponNameText;
    [SerializeField]
    protected TextMeshProUGUI WeaponTypeText;
    [SerializeField]
    protected TextMeshProUGUI WeaponDescriptionText;
    [SerializeField]
    protected TextMeshProUGUI WeaponDamageText;
    [SerializeField]
    protected TextMeshProUGUI WeaponPriceText;

    public void PopulateDetailPanel(WeaponConfigBaseSO config, int weaponLevel) {
        WeaponNameText.text = config.WeaponName + (weaponLevel > 0 ? " MK." + weaponLevel : "");
        WeaponTypeText.text = WeaponTypeToString(config.WeaponType);
        WeaponDescriptionText.text = config.WeaponDescription;
        int levelIndex = 0;
        if(weaponLevel > 0) {
            levelIndex = weaponLevel-1;
        }
        AWeaponLevelConfig levelConfig = config.WeaponLevels[levelIndex];
        if(config.CurrentUnlockedWeaponLevel < weaponLevel) {
            WeaponPriceText.gameObject.SetActive(true);
            WeaponPriceText.text = levelConfig.WeaponPrice.ToString() + " $";
        } else {
            WeaponPriceText.gameObject.SetActive(false);
        }
        WeaponDamageText.text = Mathf.FloorToInt(levelConfig.WeaponDamage).ToString();
    }

    protected string WeaponTypeToString(WeaponType weaponType) {
        switch(weaponType) {
            case WeaponType.Energy:
                return "Energy";
            case WeaponType.Kinetic:
                return "Kinetic";
            default:
                return "Unknown";
        }
    }
}
