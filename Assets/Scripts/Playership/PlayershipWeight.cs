using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayershipWeight : MonoBehaviour
{
    [SerializeField]
    private CargoPlayerDataSO cargoPlayerData;
    [SerializeField]
    private WeightPlayerDataSO weightPlayerData;

    void Awake()
    {
        cargoPlayerData.OnCargoChanged.AddListener(HandleCargoChange);
    }

    private void HandleCargoChange(PickupStack[] cargo) {
        float currentWeight = ComputeCargoTotalWeight(cargo); 
        weightPlayerData.CurrentWeight = currentWeight;
    }


    private float ComputeCargoTotalWeight(PickupStack[] cargo) {
        float weight = 0f;
        foreach(PickupStack ps in cargo)
        {
            if (ps != null)
            {
                weight += ps.GetStackWeight();
            }
        }
        return weight;
    }
}
