using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserCannonArrayConfigSO", menuName = "ScriptableObjects/WeaponConfigs/LaserCannonArrayConfigSO", order = 1)]
public class LaserCannonArrayConfigSO : WeaponConfigBaseSO
{
    public override GameObject InstantiateWeapon()
    {
        GameObject laserCannonGO = Instantiate(WeaponPrefab);
        LaserCannonArray laserCannonArray = laserCannonGO.GetComponent<LaserCannonArray>();
        laserCannonArray.WeaponConfig = this;
        return laserCannonGO;
    }
}
