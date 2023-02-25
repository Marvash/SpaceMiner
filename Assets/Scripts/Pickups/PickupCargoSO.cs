using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PickupCargoSO", menuName = "ScriptableObjects/PickupCargoSO", order = 1)]
public class PickupCargoSO: ScriptableObject
{
    [SerializeField]
    public int cargoSlotCount;

    private PickupStack[] _pickupCargo;
    public UnityEvent<int, PickupStack> cargoSlotChangeEvent;
    public UnityEvent<bool> infoWindowOpenCloseEvent;

    private void OnEnable()
    {
        _pickupCargo = new PickupStack[cargoSlotCount];
        resetCargo();
        cargoSlotChangeEvent = new UnityEvent<int, PickupStack>();
        infoWindowOpenCloseEvent = new UnityEvent<bool>();
    }

    private void resetCargo()
    {
        for (int i = 0; i < cargoSlotCount; i++)
        {
            _pickupCargo[i] = null;
        }
    }

    public List<PickupStack> GetPickups()
    {
        List<PickupStack> pickups = new List<PickupStack>();
        foreach(PickupStack ps in _pickupCargo)
        {
            if (ps != null)
            {
                pickups.Add(ps);
            }
        }
        return pickups;
    }

    public void addPickupStackToInventory(PickupStack pickupStack)
    {
        int existingPickupIndex = pickupIndexInCargo(pickupStack.pickupSO.pickupId);
        if (existingPickupIndex != -1)
        {
            _pickupCargo[existingPickupIndex].stackCount += pickupStack.stackCount;
            cargoSlotChangeEvent.Invoke(existingPickupIndex, _pickupCargo[existingPickupIndex]);
        }
        else
        {
            int nextFreeCargoSlotIndex = getNextFreeCargoSlot();
            if (nextFreeCargoSlotIndex != -1)
            {
                _pickupCargo[nextFreeCargoSlotIndex] = pickupStack;
                cargoSlotChangeEvent.Invoke(nextFreeCargoSlotIndex, _pickupCargo[nextFreeCargoSlotIndex]);
            }
        }
    }

    private int pickupIndexInCargo(PickupSO.PickupId pickupId)
    {
        for(int i = 0; i < cargoSlotCount; i++)
        {
            PickupStack pickupStack = _pickupCargo[i];
            if (pickupStack != null && pickupStack.pickupSO.pickupId == pickupId)
            {
                return i;
            }
        } 
        return -1;
    }

    private int getNextFreeCargoSlot()
    {
        for (int i = 0; i < cargoSlotCount; i++)
        {
            PickupStack pickupStack = _pickupCargo[i];
            if (pickupStack == null)
            {
                return i;
            }
        }
        return -1;
    }

    public bool cargoHasFreeSlot()
    {
        return getNextFreeCargoSlot() != -1;
    }

    public bool canAddPickupToCargo(PickupStack pickupStack)
    {
        return cargoHasFreeSlot() || (pickupIndexInCargo(pickupStack.pickupSO.pickupId) != -1); 
    }

    public void SetPickups(List<PickupStack> pickupList)
    {
        resetCargo();
        for (int i = 0; i < cargoSlotCount; i++)
        {
            if(pickupList.Count > i)
            {
                addPickupStackToInventory(pickupList[i]);
            } else
            {
                cargoSlotChangeEvent.Invoke(i, null);
            }
        }
    }
}