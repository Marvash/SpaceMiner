using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWeapon : MonoBehaviour
{
    public abstract void ShootBegin();

    public abstract void ShootInterrupt();
    public abstract void ShootEnd();

    public abstract bool IsActive();
}
