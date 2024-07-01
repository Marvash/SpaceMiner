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
    MoneyPlayerDataSO moneyPlayerData;

    void Start() {
        weaponShopUI.OnActivateUI.AddListener(HandleUIActivation);
        weaponShopUI.OnDeactivateUI.AddListener(HandleUIDeactivation);
        weaponShopUI.OnWeaponPurchase.AddListener(HandleWeaponPurchase);
        weaponShopUI.OnWeaponSlotAssign.AddListener(HandleWeaponAssignToSlot);
        weaponShopUI.OnWeaponSlotPurchase.AddListener(HandleWeaponSlotPurchase);
    }

    void HandleWeaponPurchase(WeaponConfigBaseSO config, int level) {
        int weaponCost = config.WeaponLevels[level - 1].WeaponPrice;
        if(moneyPlayerData.TrySubtractMoney(weaponCost)) {
            weaponsPlayerData.UnlockWeapon(config, level);
            weaponShopUI.UpdateWeaponDetailPanel();
            weaponShopUI.UpdateWeaponListItem(config);
            weaponShopUI.UpdateBalance(moneyPlayerData.GetCurrentBalance());
        }
    }

    void HandleWeaponAssignToSlot(WeaponConfigBaseSO config, int selectedLevel, int selectedSlot) {
        weaponsPlayerData.SetWeaponSlot(config, selectedLevel, selectedSlot);
        weaponShopUI.UpdateWeaponSlots(weaponsPlayerData);
    }

    void HandleWeaponSlotPurchase() {
        int weaponSlotCost = weaponsPlayerData.WeaponSlotsPrices[weaponsPlayerData.WeaponSlotsCount];
        if(moneyPlayerData.TrySubtractMoney(weaponSlotCost)) {
            weaponsPlayerData.IncreaseWeaponSlots(1);
            weaponShopUI.UpdateWeaponSlots(weaponsPlayerData);
            weaponShopUI.UpdateBalance(moneyPlayerData.GetCurrentBalance());
        }
    }

    void HandleUIActivation(IGameUI ui) {
        weaponShopUI.InitWeaponShopData(weaponsPlayerData, moneyPlayerData.GetCurrentBalance());
    }

    void HandleUIDeactivation(IGameUI ui) {
    }
}
