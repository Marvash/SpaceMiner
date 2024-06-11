using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WeaponsPlayerDataSO", menuName = "ScriptableObjects/WeaponsPlayerDataSO", order = 1)]
public class WeaponsPlayerDataSO : ScriptableObject
{
    public UnityEvent<int, WeaponConfigBaseSO> OnWeaponSlotSet = new UnityEvent<int, WeaponConfigBaseSO>();
    public UnityEvent<int> OnWeaponSlotsCountChange = new UnityEvent<int>();
    [SerializeField]
    List<WeaponConfigBaseSO> CompleteWeaponConfigList = new List<WeaponConfigBaseSO>();
    [SerializeField]
    List<WeaponConfigBaseSO> OwnedWeaponsList = new List<WeaponConfigBaseSO>();
    [SerializeField]
    WeaponConfigBaseSO[] WeaponSlots;
    [SerializeField]
    private int MaxWeaponSlotsCount = 4;
    [SerializeField]
    private int WeaponSlotsCount = 1;

    public List<WeaponConfigBaseSO> GetCompleteWeaponConfigList() {
        return CompleteWeaponConfigList;
    }

    public List<WeaponConfigBaseSO> GetOwnedWeaponList() {
        return OwnedWeaponsList;
    }

    public WeaponConfigBaseSO[] GetWeaponSlots() {
        return WeaponSlots;
    }

    void OnEnable() {
        int weaponSlotsCount = WeaponSlots.Length;
        WeaponSlotsCount = weaponSlotsCount;
        MaxWeaponSlotsCount = Mathf.Max(MaxWeaponSlotsCount, WeaponSlotsCount);
        foreach(WeaponConfigBaseSO config in WeaponSlots) {
            if(!OwnedWeaponsList.Contains(config)) {
                OwnedWeaponsList.Add(config);
            }
        }
        foreach(WeaponConfigBaseSO config in OwnedWeaponsList) {
            if(!CompleteWeaponConfigList.Contains(config)) {
                CompleteWeaponConfigList.Add(config);
            }
        }
    }

    public int GetWeaponSlotsCount() {
        return WeaponSlotsCount;
    }

    public void SetWeaponSlot(int slotIndex, WeaponConfigBaseSO weapon) {
        if(OwnedWeaponsList.Contains(weapon)) {
            WeaponSlots[slotIndex] = weapon;
            OnWeaponSlotSet.Invoke(slotIndex, weapon);
        } else {
            Debug.LogWarning("Trying to set weapon slot for non owned weapon");
        }
    }

    public void IncreaseWeaponSlots(int amount) {
        WeaponSlotsCount = Mathf.Min(WeaponSlotsCount + amount, MaxWeaponSlotsCount);
        WeaponConfigBaseSO[] WeaponSlotsTmp = WeaponSlots;
        WeaponSlots = new WeaponConfigBaseSO[WeaponSlotsCount];
        for(int i = 0; i < WeaponSlotsTmp.Length; i++) {
            WeaponSlots[i] = WeaponSlotsTmp[i];
        }
        OnWeaponSlotsCountChange.Invoke(WeaponSlotsCount);
    }
}
