using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponDetailPanelFactory
{
    GameObject PanelPrefab { get; }
    AWeaponDetailUI InstantiateWeaponDetailPanel();
    AWeaponDetailUI InstantiateWeaponDetailPanel(int weaponLevel);
}
