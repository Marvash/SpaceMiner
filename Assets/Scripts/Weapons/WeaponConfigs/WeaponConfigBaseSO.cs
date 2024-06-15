using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponConfigBaseSO : ScriptableObject, IWeaponConfig
{
    private static int weaponIdCounter;
    public int WeaponId { get; private set; }
    [field:SerializeField]
    public string WeaponName { get; set; }

    [field:SerializeField]
    public WeaponType WeaponType { get; set; }
    
    public abstract List<AWeaponLevelConfig> WeaponLevels { get; }
    
    [field:SerializeField]
    public string WeaponDescription { get; set; }

    [field:SerializeField]
    public GameObject WeaponPrefab { get; set; }
    [field:SerializeField]
    public GameObject WeaponDetailPanelPrefab { get; set; }

    [field:SerializeField]
    public Sprite WeaponIcon { get; set; }
    [SerializeField]
    private int currentWeaponLevel;
    
    public int CurrentWeaponLevel { get => currentWeaponLevel;  set {
        currentWeaponLevel = value;
        OnCurrentWeaponLevelChange.Invoke();
    } }
    [field:SerializeField]
    public int CurrentUnlockedWeaponLevel {get; set;}

    public IWeaponDetailPanelFactory WeaponDetailPanelFactory { get; protected set; }
    public IWeaponFactory WeaponFactory { get; protected set; }
    public UnityEvent OnCurrentWeaponLevelChange { get; protected set; }

    protected virtual void OnEnable() {
        WeaponId = weaponIdCounter++;
        if(CurrentWeaponLevel > CurrentUnlockedWeaponLevel) {
            CurrentUnlockedWeaponLevel = CurrentWeaponLevel;
        }
        if(CurrentUnlockedWeaponLevel > 0 && CurrentWeaponLevel == 0) {
            CurrentWeaponLevel = CurrentUnlockedWeaponLevel;
        }
        OnCurrentWeaponLevelChange = new UnityEvent();
    }
}
