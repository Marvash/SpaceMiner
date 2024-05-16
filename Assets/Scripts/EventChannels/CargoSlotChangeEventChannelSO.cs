using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CargoSlotChangeEventChannelSO", menuName = "ScriptableObjects/CargoSlotChangeEventChannelSO", order = 1)]
public class CargoSlotChangeEventChannelSO : ScriptableObject
{
    [HideInInspector]
    public UnityEvent<int, PickupStack> OnCargoSlotChanged = new UnityEvent<int, PickupStack>();

    public void RaiseEvent(int slotIndex, PickupStack cargo) {
        OnCargoSlotChanged.Invoke(slotIndex, cargo);
    }
}
