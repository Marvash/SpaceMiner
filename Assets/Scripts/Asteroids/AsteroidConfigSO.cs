using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidType
{
    NONE,
    COMMON,
    UNCOMMON,
    RARE
}

[CreateAssetMenu(fileName = "AsteroidConfigSO", menuName = "ScriptableObjects/AsteroidConfigSO", order = 3)]
public class AsteroidConfigSO : ScriptableObject
{
    [SerializeField]
    public AsteroidType Type;
    [SerializeField]
    public string DisplayName;
    [SerializeField]
    public List<GameObject> VariantPrefabs;
    [SerializeField]
    public int Health;
    [SerializeField]
    public string DropTableId;
    [SerializeField]
    public int MaxMiningDrops;
    [SerializeField]
    public int MinMiningDrops;
    [SerializeField]
    public int MaxBreakingDrops;
    [SerializeField]
    public int MinBreakingDrops;
}
