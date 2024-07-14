using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpaceshipModListItemUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI modNameText;
    [SerializeField]
    Image modImage;
    [SerializeField]
    Toggle lockToggle;
    [SerializeField]
    Toggle itemToggle;
    public SpaceshipModConfigBaseSO modConfig { get; private set; }
    public UnityEvent<SpaceshipModConfigBaseSO> OnItemSelect = new UnityEvent<SpaceshipModConfigBaseSO>();

    public void UpdateModListItemUI(SpaceshipModConfigBaseSO config) {
        modConfig = config;
        if(modConfig.CurrentUnlockedModLevel > 0) {
            modNameText.text = BuildWeaponName(modConfig.ModName, modConfig.CurrentUnlockedModLevel);
        } else {
            modNameText.text = modConfig.ModName;
        }
        modImage.sprite = modConfig.ModIcon;
        lockToggle.SetIsOnWithoutNotify(modConfig.CurrentUnlockedModLevel == 0);
    }

    private string BuildWeaponName(string weaponName, int weaponLevel) {
        return weaponName + " MK. " + weaponLevel.ToString();
    }

    public void HandleItemClick() {
        OnItemSelect.Invoke(modConfig);
    }

    public void SetSelectedState(bool isSelected) {
        itemToggle.SetIsOnWithoutNotify(isSelected);
    }

    public void SetToggleGroup(ToggleGroup group) {
        itemToggle.group = group;
    }
}
