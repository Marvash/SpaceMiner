using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct WeaponShopListItemData {
    public WeaponShopListItemData(WeaponDescriptorBaseSO weaponDescriptorBase, bool locked) {
        WeaponName = weaponDescriptorBase.WeaponName;
        WeaponImage = weaponDescriptorBase.WeaponIcon;
        WeaponLevel = 0;
        this.locked = locked;
    }

    public WeaponShopListItemData(WeaponDescriptorBaseSO weaponDescriptorBase, bool locked, int weaponLevel) {
        WeaponName = weaponDescriptorBase.WeaponName;
        WeaponImage = weaponDescriptorBase.WeaponIcon;
        WeaponLevel = weaponLevel;
        this.locked = locked;
    }
    public string WeaponName;
    public Sprite WeaponImage;
    public int WeaponLevel;
    public bool locked;
} 

public class WeaponShopListItemUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI WeaponNameText;
    [SerializeField]
    Image WeaponImage;
    [SerializeField]
    Toggle LockToggle;

    public void UpdateWeaponListItemUI(WeaponShopListItemData weaponShopListItemData) {
        if(!weaponShopListItemData.locked) {
            WeaponNameText.text = BuildWeaponName(weaponShopListItemData.WeaponName, weaponShopListItemData.WeaponLevel);
        } else {
            WeaponNameText.text = weaponShopListItemData.WeaponName;
        }
        WeaponImage.sprite = weaponShopListItemData.WeaponImage;
        LockToggle.SetIsOnWithoutNotify(weaponShopListItemData.locked);
    }

    private string BuildWeaponName(string weaponName, int weaponLevel) {
        return weaponName + " MK. " + weaponLevel.ToString();
    }
}
