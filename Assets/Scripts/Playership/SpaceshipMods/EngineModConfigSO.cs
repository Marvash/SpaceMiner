using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EngineModLevelConfig : ASpaceshipModLevelConfig {
}

[CreateAssetMenu(fileName = "EngineModConfigSO", menuName = "ScriptableObjects/SpaceshipModsConfig/EngineModConfigSO", order = 1)]
public class EngineModConfigSO : SpaceshipModConfigBaseSO
{
    public List<EngineModLevelConfig> EngineModLevelConfigs;

    public override List<ASpaceshipModLevelConfig> ModLevels { get => EngineModLevelConfigs.Cast<ASpaceshipModLevelConfig>().ToList(); }
}
