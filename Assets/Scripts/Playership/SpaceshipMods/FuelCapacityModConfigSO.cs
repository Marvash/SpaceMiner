using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FuelCapacityModLevelConfig : ASpaceshipModLevelConfig {
    [field:SerializeField]
    public float FuelCapacity { get; set; }
}

[CreateAssetMenu(fileName = "FuelCapacityModConfigSO", menuName = "ScriptableObjects/SpaceshipModsConfig/FuelCapacityModConfigSO", order = 1)]
public class FuelCapacityModConfigSO : SpaceshipModConfigBaseSO
{
    public List<FuelCapacityModLevelConfig> FuelCapacityModLevelConfigs;

    public override List<ASpaceshipModLevelConfig> ModLevels { get => FuelCapacityModLevelConfigs.Cast<ASpaceshipModLevelConfig>().ToList(); }
}
