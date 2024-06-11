using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponConfigVisitor
{
    void VisitLaserCannonArrayConfig(LaserCannonArrayConfigSO laserCannonConfig);
}
