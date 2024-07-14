using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class MiningLaserModLevelConfig : ASpaceshipModLevelConfig {
}

[CreateAssetMenu(fileName = "MiningLaserModConfigSO", menuName = "ScriptableObjects/SpaceshipModsConfig/MiningLaserModConfigSO", order = 1)]
public class MiningLaserModConfigSO : SpaceshipModConfigBaseSO
{
    public List<MiningLaserModLevelConfig> MiningLaserModLevelConfigs;

    public override List<ASpaceshipModLevelConfig> ModLevels { get => MiningLaserModLevelConfigs.Cast<ASpaceshipModLevelConfig>().ToList(); }
}
