using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayershipPresenter : MonoBehaviour
{
    [SerializeField]
    CargoPlayerDataSO cargoPlayerData;
    [SerializeField]
    WeightPlayerDataSO weightPlayerData;
    [SerializeField]
    MoneyPlayerDataSO moneyPlayerData;
    [SerializeField]
    PlayershipUI playershipUI;

    void Start() {
        playershipUI.OnActivateUI.AddListener(HandleUIActivation);
        playershipUI.OnDeactivateUI.AddListener(HandleUIDeactivation);
    }

    void HandleUIActivation(IGameUI ui) {
        playershipUI.UpdateCargo(cargoPlayerData.GetCargo());
        playershipUI.UpdateWeight(weightPlayerData.CurrentWeight, weightPlayerData.MaxWeight, weightPlayerData.CurrentWeightColor);
        playershipUI.UpdateMoney(moneyPlayerData.GetCurrentBalance());
    }

    void HandleUIDeactivation(IGameUI ui) {
        playershipUI.ResetCargoUI();
    }
}
