using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayershipWeightUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI WeightText;
    [SerializeField]
    private FloatEventChannelSO WeightSO;
    [SerializeField]
    private FloatEventChannelSO MaxWeightSO;
    [SerializeField]
    private PlayershipWeightConfigSO PlayershipWeightConfigSO;

    private float weight;
    private float maxWeight;


    // Start is called before the first frame update
    void Awake()
    {
        if(WeightSO != null)
            WeightSO.OnFloatChanged.AddListener(HandleWeightUpdate);
        if(MaxWeightSO != null)
            MaxWeightSO.OnFloatChanged.AddListener(HandleMaxWeightUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Color ComputeUIWeightTextColor() {
        Color finalColor = Color.white;
        if(PlayershipWeightConfigSO.WeightLevels.Count == 0)
            return finalColor;
        float weightPercentage = weight/maxWeight;
        for(int i = 0; i < PlayershipWeightConfigSO.WeightLevels.Count; i++) {
            WeightLevel level = PlayershipWeightConfigSO.WeightLevels[i];
            if((weightPercentage < level.levelValuePercentage) || i == (PlayershipWeightConfigSO.WeightLevels.Count - 1)) {
                if(i == 0) {
                    finalColor = PlayershipWeightConfigSO.WeightLevels[i].textColor;
                } else {
                    float prevLevelValue = PlayershipWeightConfigSO.WeightLevels[i-1].levelValuePercentage;
                    float currLevelValue = PlayershipWeightConfigSO.WeightLevels[i].levelValuePercentage;
                    float lerp = (weightPercentage - prevLevelValue) / (currLevelValue - prevLevelValue);
                    finalColor = Color.Lerp(PlayershipWeightConfigSO.WeightLevels[i-1].textColor, PlayershipWeightConfigSO.WeightLevels[i].textColor, lerp);
                }
                break;
            }
        }
        return finalColor;
    }

    private void HandleWeightUpdate(float weight) {
        this.weight = weight;
        WeightUIUpdate();
    }

    private void HandleMaxWeightUpdate(float maxWeight) {
        this.maxWeight = maxWeight;
        WeightUIUpdate();
    }
    
    private void WeightUIUpdate() {
        WeightText.text = (int)weight + " / " + (int)maxWeight;
        WeightText.color = ComputeUIWeightTextColor();
    }

    private void OnDestroy() {
        if(WeightSO != null)
            WeightSO.OnFloatChanged.RemoveListener(HandleWeightUpdate);
        if(MaxWeightSO != null)
            MaxWeightSO.OnFloatChanged.RemoveListener(HandleMaxWeightUpdate);
    }
}
