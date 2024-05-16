using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CargoChangeEventChannelSO", menuName = "ScriptableObjects/CargoChangeEventChannelSO", order = 1)]
public class CargoChangeEventChannelSO : ScriptableObject
{
    [HideInInspector]
    public UnityEvent<PickupStack[]> OnCargoChanged = new UnityEvent<PickupStack[]>();

    public void RaiseEvent(PickupStack[] cargo) {
        OnCargoChanged.Invoke(cargo);
    }
}