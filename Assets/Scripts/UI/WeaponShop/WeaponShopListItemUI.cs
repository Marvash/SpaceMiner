using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WeaponShopListItemUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI weaponNameText;
    [SerializeField]
    Image weaponImage;
    [SerializeField]
    Toggle lockToggle;
    [SerializeField]
    Toggle itemToggle;
    public WeaponConfigBaseSO weaponConfig { get; private set; }
    public UnityEvent<WeaponConfigBaseSO> OnItemSelect = new UnityEvent<WeaponConfigBaseSO>();

    public void UpdateWeaponListItemUI(WeaponConfigBaseSO config) {
        weaponConfig = config;
        if(weaponConfig.CurrentUnlockedWeaponLevel > 0) {
            weaponNameText.text = BuildWeaponName(weaponConfig.WeaponName, weaponConfig.CurrentUnlockedWeaponLevel);
        } else {
            weaponNameText.text = weaponConfig.WeaponName;
        }
        weaponImage.sprite = weaponConfig.WeaponIcon;
        lockToggle.SetIsOnWithoutNotify(weaponConfig.CurrentUnlockedWeaponLevel == 0);
    }

    private string BuildWeaponName(string weaponName, int weaponLevel) {
        return weaponName + " MK. " + weaponLevel.ToString();
    }

    public void HandleItemClick() {
        OnItemSelect.Invoke(weaponConfig);
    }

    public void SetSelectedState(bool isSelected) {
        itemToggle.SetIsOnWithoutNotify(isSelected);
    }

    public void SetToggleGroup(ToggleGroup group) {
        itemToggle.group = group;
    }
}
