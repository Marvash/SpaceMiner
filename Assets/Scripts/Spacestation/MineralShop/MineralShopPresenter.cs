using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralShopPresenter : MonoBehaviour
{
    [SerializeField]
    CargoPlayerDataSO cargoPlayerData;
    [SerializeField]
    MineralShopUI mineralShopUI;
    [SerializeField]
    MoneyPlayerDataSO moneyPlayerData;

    void Start() {
        mineralShopUI.OnActivateUI.AddListener(HandleUIActivation);
        mineralShopUI.OnDeactivateUI.AddListener(HandleUIDeactivation);
    }

    void HandleUIActivation(IGameUI ui) {
        mineralShopUI.UpdateMineralList(cargoPlayerData.GetCargo());
        mineralShopUI.OnItemSell.AddListener(HandleItemSold);
        mineralShopUI.OnSellAllItems.AddListener(HandleSellAllItems);
    }

    void HandleUIDeactivation(IGameUI ui) {
        mineralShopUI.OnItemSell.RemoveListener(HandleItemSold);
        mineralShopUI.OnSellAllItems.RemoveListener(HandleSellAllItems);
    }

    void HandleItemSold(int index) {
        PickupStack soldStack = cargoPlayerData.GetCargo()[index];
        moneyPlayerData.SellItem(soldStack);
        cargoPlayerData.RemoveCargoItemByIndex(index);
        mineralShopUI.UpdateMineralList(cargoPlayerData.GetCargo());
    }

    void HandleSellAllItems() {
        foreach(PickupStack stack in cargoPlayerData.GetCargo()) {
            if(stack != null) {
                moneyPlayerData.SellItem(stack);
            }
        }
            cargoPlayerData.ResetCargo();
            mineralShopUI.UpdateMineralList(cargoPlayerData.GetCargo());
    }
}
