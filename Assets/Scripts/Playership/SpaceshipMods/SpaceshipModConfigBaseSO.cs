using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SpaceshipModConfigBaseSO : ScriptableObject, ISpaceshipModConfig
{
    public UnityEvent OnCurrentModLevelChange { get; protected set; }
    private static int modIdCounter;
    public int ModId { get; private set; }
    [field:SerializeField]
    public string ModName { get; set; }

    public abstract List<ASpaceshipModLevelConfig> ModLevels { get; }
    [field:SerializeField]
    public string ModDescription { get; set; }
    [field:SerializeField]
    public Sprite ModIcon { get; set; }
    [SerializeField]
    private int currentModLevel;
    public int CurrentModLevel { get => currentModLevel; set {
        currentModLevel = value;
        OnCurrentModLevelChange.Invoke();
    } }
    [field:SerializeField]
    public int CurrentUnlockedModLevel { get; set; }
    public GameObject ModDetailPanelPrefab { get; set; }

    protected virtual void OnEnable() {
        ModId = modIdCounter++;
        if(CurrentModLevel > CurrentUnlockedModLevel) {
            CurrentUnlockedModLevel = CurrentModLevel;
        }
        if(CurrentUnlockedModLevel > 0 && CurrentModLevel == 0) {
            CurrentModLevel = CurrentUnlockedModLevel;
        }
        OnCurrentModLevelChange = new UnityEvent();
    }
}
