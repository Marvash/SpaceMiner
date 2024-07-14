using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class ASpaceshipModLevelConfig {
    [field:SerializeField]
    public int ModPrice { get; set; }
}

public interface ISpaceshipModConfig
{
    UnityEvent OnCurrentModLevelChange { get; }
    int ModId {get;}
    string ModName {get; set;}
    List<ASpaceshipModLevelConfig> ModLevels {get;}
    string ModDescription {get; set;}
    Sprite ModIcon {get; set;}
    int CurrentModLevel {get; set;}
    int CurrentUnlockedModLevel {get; set;}
    GameObject ModDetailPanelPrefab {get; set;}
    //IWeaponDetailPanelFactory WeaponDetailPanelFactory { get; }
}
