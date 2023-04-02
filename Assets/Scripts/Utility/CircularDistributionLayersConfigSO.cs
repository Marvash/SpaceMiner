using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Circular distribution centered on a point
public abstract class CircularDistributionLayersConfigSO<T> : ScriptableObject
{
    [System.Serializable]
    public class Layer
    {
        public float MinDistance;
        public float MaxDistance;
        public float MinSelectionWeight;
        public float MaxSelectionWeight;
        public float MinSpawnChance;
        public float MaxSpawnChance;
        public T LayerObject;
    }
    public string Id;
    public List<Layer> Layers;
}
