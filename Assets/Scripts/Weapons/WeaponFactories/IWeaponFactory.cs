using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponFactory
{
    AWeapon InstantiateWeapon(GameObject spaceshipGO);
}
