using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnergyGeneratorModLevelConfig : ASpaceshipModLevelConfig {
    [field:SerializeField]
    public float EnergyConversionRate { get; set; }
}

[CreateAssetMenu(fileName = "EnergyGeneratorModConfigSO", menuName = "ScriptableObjects/SpaceshipModsConfig/EnergyGeneratorModConfigSO", order = 1)]
public class EnergyGeneratorModConfigSO : SpaceshipModConfigBaseSO
{
    public List<EnergyGeneratorModLevelConfig> EnergyGeneratorModLevelConfigs;

    public override List<ASpaceshipModLevelConfig> ModLevels { get => EnergyGeneratorModLevelConfigs.Cast<ASpaceshipModLevelConfig>().ToList(); }
}
