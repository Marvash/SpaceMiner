using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField]
    private LaserCannonArray LaserCannonArray;

    [SerializeField]
    private MissileLauncherArray MissileLauncherArray;

    private IWeapon _selectedWeapon;

    private bool _shooting;

    private void Start()
    {
        _selectedWeapon = LaserCannonArray;
    }

    public void PlayerShoot(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            _selectedWeapon.ShootBegin();
            _shooting = true;
        } else
        {
            _selectedWeapon.ShootEnd();
            _shooting = false;
        }

    }

    public void SelectWeapon1(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            SwapWeapon(LaserCannonArray);
        }
    }

    public void SelectWeapon2(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            SwapWeapon(MissileLauncherArray);
        }
    }

    private void SwapWeapon(IWeapon newWeapon)
    {
        if (_selectedWeapon != newWeapon)
        {
            _selectedWeapon.ShootInterrupt();
            _selectedWeapon = newWeapon;
            if (_shooting)
            {
                _selectedWeapon.ShootBegin();
            }
        }
    }

}
