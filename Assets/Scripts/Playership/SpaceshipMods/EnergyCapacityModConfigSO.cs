using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnergyCapacityModLevelConfig : ASpaceshipModLevelConfig {
    [field:SerializeField]
    public float EnergyCapacity { get; set; }
}

[CreateAssetMenu(fileName = "EnergyCapacityModConfigSO", menuName = "ScriptableObjects/SpaceshipModsConfig/EnergyCapacityModConfigSO", order = 1)]
public class EnergyCapacityModConfigSO : SpaceshipModConfigBaseSO
{
    public List<EnergyCapacityModLevelConfig> EnergyCapacityModLevelConfigs;

    public override List<ASpaceshipModLevelConfig> ModLevels { get => EnergyCapacityModLevelConfigs.Cast<ASpaceshipModLevelConfig>().ToList(); }
}
