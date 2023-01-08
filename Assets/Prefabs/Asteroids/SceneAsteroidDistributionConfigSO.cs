using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneAsteroidDistributionConfigSO", menuName = "ScriptableObjects/SceneAsteroidDistributionConfigSO", order = 3)]
public class SceneAsteroidDistributionConfigSO : ScriptableObject
{
    [System.Serializable]
    public class AsteroidLayer
    {
        public float MinDistance;
        public float MaxDistance;
        public float MinSelectionWeight;
        public float MaxSelectionWeight;
        public float MinSpawnChance;
        public float MaxSpawnChance;
        public AsteroidConfigSO.AsteroidType AsteroidType;
    }

    [SerializeField]
    public string DistributionId;

    [SerializeField]
    public List<AsteroidLayer> AsteroidDistributionLayers;
}
