using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponsManager : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private LaserCannonArray LaserCannonArray;

    [SerializeField]
    private MissileLauncherArray MissileLauncherArray;

    [SerializeField]
    private List<IWeapon> Weapons = new List<IWeapon>();

    private int _weaponSlotsCount = 4;

    private int _selectedWeaponIndex;

    private bool _shooting;

    private void Awake()
    {
        _selectedWeaponIndex = 0;
        InputDispatcherSO.SelectWeaponSlot += selectWeaponHandler;
        InputDispatcherSO.FirePrimaryStart += shootStartHandler;
        InputDispatcherSO.FirePrimaryStop += shootStopHandler;
        InputDispatcherSO.IncDecWeaponSlot += incDecWeaponHandler;
        Weapons.Add(LaserCannonArray);
        Weapons.Add(MissileLauncherArray);
        Weapons.Add(null);
        Weapons.Add(null);
    }

    private void shootStartHandler()
    {
        Weapons[_selectedWeaponIndex]?.ShootBegin();
        _shooting = true;
    }

    private void shootStopHandler()
    {
        Weapons[_selectedWeaponIndex]?.ShootEnd();
        _shooting = false;
    }

    private void selectWeaponHandler(int slot)
    {
        SwapWeapon(slot - 1);
    }

    private void incDecWeaponHandler(float value)
    {
        if(value > 0.0f)
        {
            increaseWeaponIndex();
        } else if(value < 0.0f)
        {
            decreaseWeaponIndex();
        }
    }

    private void increaseWeaponIndex()
    {
        int newIndex = (_selectedWeaponIndex + 1) % _weaponSlotsCount;
        SwapWeapon(newIndex);
    }

    private void decreaseWeaponIndex()
    {
        int newIndex = (_selectedWeaponIndex - 1) % _weaponSlotsCount;
        if(newIndex < 0)
        {
            newIndex = _weaponSlotsCount - 1;
        }
        SwapWeapon(newIndex);
    }

    private void SwapWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex != _selectedWeaponIndex)
        {
            Weapons[_selectedWeaponIndex]?.ShootInterrupt();
            _selectedWeaponIndex = newWeaponIndex;
            if (_shooting)
            {
                Weapons[_selectedWeaponIndex]?.ShootBegin();
            }
        }
    }

}
