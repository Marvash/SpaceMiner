using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponConfig
{
    WeaponDescriptorBaseSO WeaponDescriptor {get; set;}
    int CurrentWeaponLevel {get; set;}
    public void AcceptWeaponDetailUIBuilder();

}
