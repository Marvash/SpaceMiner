using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannonArrayWeaponFactory : IWeaponFactory
{
    public GameObject WeaponPrefab { get; private set; }
    public LaserCannonArrayConfigSO LaserCannonArrayConfig { get; private set; }

    public LaserCannonArrayWeaponFactory(GameObject weaponPrefab, LaserCannonArrayConfigSO config) {
        WeaponPrefab = weaponPrefab;
        LaserCannonArrayConfig = config;
    }

    public AWeapon InstantiateWeapon(GameObject spaceshipGO) {
        GameObject laserCannonGO = GameObject.Instantiate(WeaponPrefab);
        LaserCannonArray laserCannonArray = laserCannonGO.GetComponent<LaserCannonArray>();
        laserCannonArray.LaserCannonConfig = LaserCannonArrayConfig;
        laserCannonArray.PlayershipGO = spaceshipGO;
        return laserCannonGO.GetComponent<AWeapon>();
    }

}
