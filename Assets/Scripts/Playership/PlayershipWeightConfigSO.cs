using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightLevel {
    public float levelValuePercentage;
    public float movementMutliplier;
    public Color textColor;
}

[CreateAssetMenu(fileName = "PlayershipWeightConfigSO", menuName = "ScriptableObjects/PlayershipWeightConfigSO", order = 3)]
public class PlayershipWeightConfigSO : ScriptableObject
{
    public List<WeightLevel> WeightLevels = new List<WeightLevel>();
}
