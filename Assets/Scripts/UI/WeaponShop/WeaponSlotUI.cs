using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField]
    private Image weaponIcon;

    private WeaponConfigBaseSO weaponConfig;

    public UnityEvent<WeaponConfigBaseSO> OnWeaponSlotSelected = new UnityEvent<WeaponConfigBaseSO>();

    void Awake() {
        weaponIcon.enabled = false;
    }

    public void UpdateSlot(WeaponConfigBaseSO config) {
        weaponConfig = config;
        weaponIcon.sprite = weaponConfig.WeaponIcon;
        weaponIcon.enabled = true;
    }

    public void HandleWeaponSlotClick() {
        OnWeaponSlotSelected.Invoke(weaponConfig);
    }
}
