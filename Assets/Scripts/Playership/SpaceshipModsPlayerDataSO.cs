using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[CreateAssetMenu(fileName = "SpaceshipModsPlayerDataSO", menuName = "ScriptableObjects/SpaceshipModsPlayerDataSO", order = 3)]
public class SpaceshipModsPlayerDataSO : ScriptableObject
{
    [field:SerializeField]
    public List<SpaceshipModConfigBaseSO> CompleteModsConfigList { get; set; }
}
