using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HullModLevelConfig : ASpaceshipModLevelConfig {
}

[CreateAssetMenu(fileName = "HullModConfigSO", menuName = "ScriptableObjects/SpaceshipModsConfig/HullModConfigSO", order = 1)]
public class HullModConfigSO : SpaceshipModConfigBaseSO
{
    public List<MiningLaserModLevelConfig> HullModLevelConfigs;

    public override List<ASpaceshipModLevelConfig> ModLevels { get => HullModLevelConfigs.Cast<ASpaceshipModLevelConfig>().ToList(); }
}
