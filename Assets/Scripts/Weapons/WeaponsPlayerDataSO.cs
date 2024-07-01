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
    [field:SerializeField]
    public List<WeaponConfigBaseSO> CompleteWeaponConfigList { get; set; }
    [field:SerializeField]
    public WeaponConfigBaseSO[] WeaponSlots { get; set;} = new WeaponConfigBaseSO[0];
    [field:SerializeField]
    public int MaxWeaponSlotsCount { get; set; } = 4;
    [field:SerializeField]
    public int WeaponSlotsCount { get; set; } = 1;
    [field:SerializeField]
    public List<int> WeaponSlotsPrices { get; set; }

    void OnEnable() {
        WeaponSlotsCount = Mathf.Max(WeaponSlots.Length, WeaponSlotsCount);
        MaxWeaponSlotsCount = Mathf.Max(MaxWeaponSlotsCount, WeaponSlotsCount);
        if(WeaponSlots.Length < WeaponSlotsCount) {
            WeaponConfigBaseSO[] tmp = WeaponSlots;
            WeaponSlots = new WeaponConfigBaseSO[WeaponSlotsCount];
            for(int i = 0; i < tmp.Length; i++) {
                WeaponSlots[i] = tmp[i];
            }
        }
        foreach(WeaponConfigBaseSO config in WeaponSlots) {
            if(config != null) {
                if(!CompleteWeaponConfigList.Contains(config))
                    CompleteWeaponConfigList.Add(config);
                if(config.CurrentUnlockedWeaponLevel == 0) {
                    config.CurrentUnlockedWeaponLevel = 1;
                    config.CurrentWeaponLevel = 1;
                }
            }
        }
    }

    public void UnlockWeapon(WeaponConfigBaseSO weaponConfig, int weaponLevel) {
        weaponConfig.CurrentUnlockedWeaponLevel = weaponLevel;
        int existingIndex = -1;
        for(int i = 0; i < WeaponSlots.Length; i++) {
            if(WeaponSlots[i] == weaponConfig) {
                existingIndex = i;
                break;
            }
        }
        if(existingIndex >= 0) {
            weaponConfig.CurrentWeaponLevel = weaponLevel;
        }
    }

    public void SetWeaponSlot(WeaponConfigBaseSO weapon, int weaponLevel, int slotIndex) {
        if(weapon.CurrentUnlockedWeaponLevel >= weaponLevel) {
            int existingIndex = -1;
            for(int i = 0; i < WeaponSlots.Length; i++) {
                if(WeaponSlots[i] == weapon) {
                    existingIndex = i;
                    break;
                }
            }
            if(existingIndex == slotIndex) {
                if(weapon.CurrentWeaponLevel != weaponLevel) {
                    weapon.CurrentWeaponLevel = weaponLevel;
                }
                return;
            } else if(existingIndex >= 0) {
                RemoveWeaponFromSlot(existingIndex);
            }
            RemoveWeaponFromSlot(slotIndex);
            weapon.CurrentWeaponLevel = weaponLevel;
            WeaponSlots[slotIndex] = weapon;
            OnWeaponSlotSet.Invoke(slotIndex, weapon);
        } else {
            Debug.LogWarning("Trying to set weapon slot for non owned weapon");
        }
    }

    public void RemoveWeaponFromSlot(int slotIndex) {
        if(WeaponSlots[slotIndex] != null) {
            WeaponSlots[slotIndex] = null;
            OnWeaponSlotSet.Invoke(slotIndex, null);
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
