using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct WeightLevel {
    public float levelValuePercentage;
    public float movementMutliplier;
    public Color textColor;
}

[CreateAssetMenu(fileName = "WeightPlayerDataSO", menuName = "ScriptableObjects/WeightPlayerDataSO", order = 3)]
public class WeightPlayerDataSO : ScriptableObject
{
    [SerializeField]
    private List<WeightLevel> WeightLevels = new List<WeightLevel>();

    public float CurrentWeightMultiplier { get; private set;}
    public Color CurrentWeightColor { get; private set;}

    private float currentWeight = 0.0f;
    public float CurrentWeight { get { return currentWeight; } set {
        currentWeight = value;
        ComputeCurrentWeightMultiplier();
        ComputeCurrentWeightColor();
        OnCurrentWeightUpdate.Invoke(currentWeight);
    } }
    private float maxWeight = 100.0f;
    public float MaxWeight { get { return maxWeight; } set {
        maxWeight = value;
        ComputeCurrentWeightMultiplier();
        ComputeCurrentWeightColor();
        OnMaxWeightUpdate.Invoke(maxWeight);
    } }

    void OnEnable() {
        CurrentWeight = 0.0f;
    }

    public UnityEvent<float> OnCurrentWeightUpdate = new UnityEvent<float>();
    public UnityEvent<float> OnMaxWeightUpdate = new UnityEvent<float>();

    private void ComputeCurrentWeightMultiplier() {
        float multiplier = 1.0f;
        float currentWeightPercentage = CurrentWeight / MaxWeight;
        if(WeightLevels.Count == 0)
            CurrentWeightMultiplier = multiplier;
        for(int i = 0; i < WeightLevels.Count; i++) {
            WeightLevel level = WeightLevels[i];
            if((currentWeightPercentage < level.levelValuePercentage) || i == (WeightLevels.Count - 1)) {
                if(i == 0) {
                    multiplier = level.movementMutliplier;
                } else {
                    float cargoPercentage = CurrentWeight / MaxWeight;
                    float prevLevelValue = WeightLevels[i-1].levelValuePercentage;
                    float currLevelValue = WeightLevels[i].levelValuePercentage;
                    float lerp = (cargoPercentage - prevLevelValue) / (currLevelValue - prevLevelValue);
                    multiplier = Mathf.Lerp(WeightLevels[i-1].movementMutliplier, WeightLevels[i].movementMutliplier, lerp);
                }
                break;
            }
        }
        CurrentWeightMultiplier = multiplier;
    }

    private void ComputeCurrentWeightColor() {
        Color finalColor = Color.white;
        if(WeightLevels.Count == 0)
            CurrentWeightColor = finalColor;
        float weightPercentage = CurrentWeight / MaxWeight;
        for(int i = 0; i < WeightLevels.Count; i++) {
            WeightLevel level = WeightLevels[i];
            if((weightPercentage < level.levelValuePercentage) || i == (WeightLevels.Count - 1)) {
                if(i == 0) {
                    finalColor = WeightLevels[i].textColor;
                } else {
                    float prevLevelValue = WeightLevels[i-1].levelValuePercentage;
                    float currLevelValue = WeightLevels[i].levelValuePercentage;
                    float lerp = (weightPercentage - prevLevelValue) / (currLevelValue - prevLevelValue);
                    finalColor = Color.Lerp(WeightLevels[i-1].textColor, WeightLevels[i].textColor, lerp);
                }
                break;
            }
        }
        CurrentWeightColor = finalColor;
    }
}
