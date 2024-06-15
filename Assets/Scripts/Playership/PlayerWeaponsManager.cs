using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponsManager : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private GameObject WeaponsGO;

    public AWeapon[] weapons {get; private set;}

    private int selectedWeaponIndex;

    private bool shooting;

    [SerializeField]
    WeaponsPlayerDataSO weaponPlayerData;

    private void Start()
    {
        selectedWeaponIndex = 0;
        InputDispatcherSO.SelectWeaponSlot += SelectWeaponHandler;
        InputDispatcherSO.FirePrimaryStart += ShootStartHandler;
        InputDispatcherSO.FirePrimaryStop += ShootStopHandler;
        InputDispatcherSO.IncDecWeaponSlot += IncDecWeaponHandler;
        weaponPlayerData.OnWeaponSlotSet.AddListener(HandleWeaponSlotSet);
        weaponPlayerData.OnWeaponSlotsCountChange.AddListener(ResizeWeaponsArray);
        int weaponCount = WeaponsGO.transform.childCount;
        while(WeaponsGO.transform.childCount > 0) {
            Destroy(WeaponsGO.transform.GetChild(0).gameObject);
        }
        InitializeWeaponSystem();
    }

    void OnDestroy() {
        InputDispatcherSO.SelectWeaponSlot -= SelectWeaponHandler;
        InputDispatcherSO.FirePrimaryStart -= ShootStartHandler;
        InputDispatcherSO.FirePrimaryStop -= ShootStopHandler;
        InputDispatcherSO.IncDecWeaponSlot -= IncDecWeaponHandler;
        weaponPlayerData.OnWeaponSlotSet.RemoveListener(HandleWeaponSlotSet);
    }

    private void InitializeWeaponSystem() {
        WeaponConfigBaseSO[] weaponSlots = weaponPlayerData.WeaponSlots;
        weapons = new AWeapon[weaponPlayerData.WeaponSlotsCount];
        for(int i = 0; i < weaponSlots.Length; i++) {
            SetWeaponBySlot(i, weaponSlots[i]);
        }
    }

    public bool SetWeaponBySlot(int weaponSlot, WeaponConfigBaseSO weaponConfig) {
        if(weaponConfig == null) {
            RemoveWeaponBySlot(weaponSlot);
            return true;
        }
        IWeaponFactory weaponFactory = weaponConfig.WeaponFactory;
        AWeapon weapon = weaponFactory.InstantiateWeapon(gameObject);
        if(weapon.transform.parent != WeaponsGO) {
            weapon.transform.SetParent(WeaponsGO.transform, false);
        }
        weapons[weaponSlot] = weapon;
        return true;
    }

    private void RemoveWeaponBySlot(int weaponSlot) {
        if(weapons[weaponSlot] != null) {
            GameObject toDestroy = weapons[weaponSlot].gameObject;
            Destroy(toDestroy);
            weapons[weaponSlot] = null;
        }
    }

    private void HandleWeaponSlotSet(int slotIndex, WeaponConfigBaseSO weapon) {
        SetWeaponBySlot(slotIndex, weapon);
    }

    private void ShootStartHandler()
    {
        weapons[selectedWeaponIndex]?.ShootBegin();
        shooting = true;
    }

    private void ShootStopHandler()
    {
        weapons[selectedWeaponIndex]?.ShootEnd();
        shooting = false;
    }

    private void SelectWeaponHandler(int slot)
    {
        SwapWeapon(slot - 1);
    }

    private void IncDecWeaponHandler(float value)
    {
        if(value > 0.0f)
        {
            IncreaseWeaponIndex();
        } else if(value < 0.0f)
        {
            DecreaseWeaponIndex();
        }
    }

    private void IncreaseWeaponIndex()
    {
        int newIndex = (selectedWeaponIndex + 1) % weapons.Length;
        SwapWeapon(newIndex);
    }

    private void DecreaseWeaponIndex()
    {
        int newIndex = (selectedWeaponIndex - 1) % weapons.Length;
        if(newIndex < 0)
        {
            newIndex = weapons.Length - 1;
        }
        SwapWeapon(newIndex);
    }

    private void SwapWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex % weapons.Length != selectedWeaponIndex)
        {
            weapons[selectedWeaponIndex]?.ShootInterrupt();
            selectedWeaponIndex = newWeaponIndex;
            if (shooting)
            {
                weapons[selectedWeaponIndex]?.ShootBegin();
            }
        }
    }

    private void ResizeWeaponsArray(int weaponSlotsCount) {
        int currentLength = weapons.Length;
        if(currentLength < weaponSlotsCount) {
            AWeapon[] weaponsTmp = weapons;
            weapons = new AWeapon[weaponSlotsCount];
            for(int i = 0; i < weaponsTmp.Length; i++) {
                weapons[i] = weaponsTmp[i];
            }
        } else if(currentLength > weaponSlotsCount) {
            int slotsDelta = currentLength - weaponSlotsCount;
            for(int i = 0; i < slotsDelta; i++) {
                RemoveWeaponBySlot(currentLength - 1 - i);
            }
            AWeapon[] weaponsTmp = weapons;
            weapons = new AWeapon[weaponSlotsCount];
            for(int i = 0; i < weaponSlotsCount; i++) {
                weapons[i] = weaponsTmp[i];
            }
        }
    }

}
