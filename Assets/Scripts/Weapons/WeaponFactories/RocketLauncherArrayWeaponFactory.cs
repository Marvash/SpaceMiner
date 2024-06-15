using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherArrayWeaponFactory : IWeaponFactory
{
    public GameObject WeaponPrefab { get; private set; }
    public RocketLauncherArrayConfigSO RocketLauncherArrayConfig { get; private set; }

    public RocketLauncherArrayWeaponFactory(GameObject weaponPrefab, RocketLauncherArrayConfigSO config) {
        WeaponPrefab = weaponPrefab;
        RocketLauncherArrayConfig = config;
    }

    public AWeapon InstantiateWeapon(GameObject spaceshipGO) {
        GameObject rocketLauncherGO = GameObject.Instantiate(WeaponPrefab);
        RocketLauncherArray rockerLauncherArray = rocketLauncherGO.GetComponent<RocketLauncherArray>();
        rockerLauncherArray.RocketLauncherConfig = RocketLauncherArrayConfig;
        rockerLauncherArray.PlayershipGO = spaceshipGO;
        return rockerLauncherArray.GetComponent<AWeapon>();
    }
}
