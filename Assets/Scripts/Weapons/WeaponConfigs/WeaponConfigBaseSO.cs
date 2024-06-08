using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponConfigBaseSO : ScriptableObject, IWeaponConfig
{
    public WeaponDescriptorBaseSO WeaponDescriptor { get; set; }
    public int CurrentWeaponLevel { get; set; }

    public abstract void AcceptWeaponDetailUIBuilder();
}
