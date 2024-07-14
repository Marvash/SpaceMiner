using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CargoCapacityModLevelConfig : ASpaceshipModLevelConfig {
    [field:SerializeField]
    public float CargoCapacity { get; set; }
}

[CreateAssetMenu(fileName = "CargoCapacityModConfigSO", menuName = "ScriptableObjects/SpaceshipModsConfig/CargoCapacityModConfigSO", order = 1)]
public class CargoCapacityModConfigSO : SpaceshipModConfigBaseSO
{
    public List<CargoCapacityModLevelConfig> CargoCapacityModLevelConfigs;

    public override List<ASpaceshipModLevelConfig> ModLevels { get => CargoCapacityModLevelConfigs.Cast<ASpaceshipModLevelConfig>().ToList(); }
}
