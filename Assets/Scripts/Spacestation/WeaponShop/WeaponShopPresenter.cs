using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopPresenter : MonoBehaviour
{
    [SerializeField]
    WeaponsPlayerDataSO weaponsPlayerData;
    [SerializeField]
    WeaponShopUI weaponShopUI;
    [SerializeField]
    MoneyPlayerDataSO playershipData;

    void Start() {
        weaponShopUI.OnActivateUI.AddListener(HandleUIActivation);
        weaponShopUI.OnDeactivateUI.AddListener(HandleUIDeactivation);
    }

    void HandleUIActivation(IGameUI ui) {
        weaponShopUI.UpdateWeaponShopData(weaponsPlayerData.GetCompleteWeaponConfigList(), weaponsPlayerData.GetWeaponSlots(), playershipData.GetMoney());
    }

    void HandleUIDeactivation(IGameUI ui) {
    }
}
