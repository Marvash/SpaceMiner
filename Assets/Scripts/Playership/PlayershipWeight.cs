using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayershipWeight : MonoBehaviour
{
    [SerializeField]
    private float MaxWeight = 120;
    [SerializeField]
    CargoChangeEventChannelSO cargoChangeEventChannelSO;
    [SerializeField]
    private FloatEventChannelSO WeightMultiplierSO;
    [SerializeField]
    private FloatEventChannelSO WeightSO;
    [SerializeField]
    private FloatEventChannelSO MaxWeightSO;
    [SerializeField]
    private PlayershipWeightConfigSO PlayershipWeightConfigSO;

    private float weightMovementMultiplier = 1.0f; 
    public float WeightMovementMultiplier { get => weightMovementMultiplier; }

    private float weight = 0.0f;
    public float Weight { get => weight; }

    void Awake()
    {
        cargoChangeEventChannelSO.OnCargoChanged.AddListener(HandleCargoChange);
    }

    void Start() {
        MaxWeightSO.RaiseEvent(MaxWeight);
        WeightSO.RaiseEvent(Weight);
        float cargoPercentage = Weight / MaxWeight;
        ComputeWeightMultiplier(cargoPercentage);
        WeightSO.RaiseEvent(weight);
        MaxWeightSO.RaiseEvent(MaxWeight);
        WeightMultiplierSO.RaiseEvent(WeightMovementMultiplier);
    }

    private void HandleCargoChange(PickupStack[] cargo) {
        float currentWeight = ComputeCargoTotalWeight(cargo);
        Debug.Log("Total weight " + currentWeight + " max " + MaxWeight);    
        SetCurrentWeight(currentWeight);
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

    private void SetCurrentWeightMultiplier(float newMultiplier) {
        weightMovementMultiplier = newMultiplier;
        WeightMultiplierSO.RaiseEvent(weightMovementMultiplier);
    }

    private void SetCurrentWeight(float newWeight) {
        if(newWeight < 0)
            return;
        weight = newWeight;
        UpdateWeight();
        WeightSO.RaiseEvent(weight);
    }

    public void SetMaxWeight(float maxWeight) {
        if(maxWeight < 0)
            return;
        MaxWeight = maxWeight;
        UpdateWeight();
        MaxWeightSO.RaiseEvent(MaxWeight);
    }

    private void UpdateWeight() {
        float cargoPercentage = weight / MaxWeight;
        float newWeightMovementMultiplier = ComputeWeightMultiplier(cargoPercentage);
        SetCurrentWeightMultiplier(newWeightMovementMultiplier);
    }

    private float ComputeWeightMultiplier(float weightPercentage) {
        float multiplier = 1.0f;
        if(PlayershipWeightConfigSO.WeightLevels.Count == 0)
            return multiplier;
        for(int i = 0; i < PlayershipWeightConfigSO.WeightLevels.Count; i++) {
            WeightLevel level = PlayershipWeightConfigSO.WeightLevels[i];
            if((weightPercentage < level.levelValuePercentage) || i == (PlayershipWeightConfigSO.WeightLevels.Count - 1)) {
                if(i == 0) {
                    multiplier = level.movementMutliplier;
                } else {
                    float cargoPercentage = Weight / MaxWeight;
                    float prevLevelValue = PlayershipWeightConfigSO.WeightLevels[i-1].levelValuePercentage;
                    float currLevelValue = PlayershipWeightConfigSO.WeightLevels[i].levelValuePercentage;
                    float lerp = (cargoPercentage - prevLevelValue) / (currLevelValue - prevLevelValue);
                    multiplier = Mathf.Lerp(PlayershipWeightConfigSO.WeightLevels[i-1].movementMutliplier, PlayershipWeightConfigSO.WeightLevels[i].movementMutliplier, lerp);
                }
                break;
            }
        }
        return multiplier;
    }
}
