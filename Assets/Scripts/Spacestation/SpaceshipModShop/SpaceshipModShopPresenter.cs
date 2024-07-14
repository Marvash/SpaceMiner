using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipModShopPresenter : MonoBehaviour
{
    [SerializeField]
    SpaceshipModsPlayerDataSO spaceshipModsPlayerData;
    [SerializeField]
    SpaceshipModShopUI spaceshipModShopUI;
    [SerializeField]
    MoneyPlayerDataSO moneyPlayerData;

    void Start() {
        spaceshipModShopUI.OnActivateUI.AddListener(HandleUIActivation);
        spaceshipModShopUI.OnDeactivateUI.AddListener(HandleUIDeactivation);
    }

    void HandleUIActivation(IGameUI ui) {
        spaceshipModShopUI.InitSpaceshipModShopData(spaceshipModsPlayerData, moneyPlayerData.GetCurrentBalance());
    }

    void HandleUIDeactivation(IGameUI ui) {
    }
}
