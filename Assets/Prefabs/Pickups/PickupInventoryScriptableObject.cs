using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PickupInventoryScriptableObject", menuName = "ScriptableObjects/PickupInventoryScriptableObject", order = 1)]
public class PickupInventoryScriptableObject : ScriptableObject
{
    [SerializeField]
    public int cargoSlotCount;

    private PickupStack[] pickupCargo;
    public UnityEvent<int, PickupStack> cargoSlotChangeEvent;
    public UnityEvent<bool> infoWindowOpenCloseEvent;
    private bool infoWindowIsOpen;

    private void OnEnable()
    {
        pickupCargo = new PickupStack[cargoSlotCount];
        for (int i = 0; i < cargoSlotCount; i++)
        {
            pickupCargo[i] = null;
        }
        cargoSlotChangeEvent = new UnityEvent<int, PickupStack>();
        infoWindowOpenCloseEvent = new UnityEvent<bool>();
        infoWindowIsOpen = false;
    }

    public void addPickupStackToInventory(PickupStack pickupStack)
    {
        int existingPickupIndex = pickupIndexInCargo(pickupStack.pickupSO.pickupId);
        if (existingPickupIndex != -1)
        {
            pickupCargo[existingPickupIndex].stackCount += pickupStack.stackCount;
            cargoSlotChangeEvent.Invoke(existingPickupIndex, pickupCargo[existingPickupIndex]);
        }
        else
        {
            int nextFreeCargoSlotIndex = getNextFreeCargoSlot();
            if (nextFreeCargoSlotIndex != -1)
            {
                pickupCargo[nextFreeCargoSlotIndex] = pickupStack;
                cargoSlotChangeEvent.Invoke(nextFreeCargoSlotIndex, pickupCargo[nextFreeCargoSlotIndex]);
            }
        }
    }

    private int pickupIndexInCargo(PickupScriptableObject.PickupId pickupId)
    {
        for(int i = 0; i < cargoSlotCount; i++)
        {
            PickupStack pickupStack = pickupCargo[i];
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
            PickupStack pickupStack = pickupCargo[i];
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

    public void infoWindowOpenCloseHandler(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            infoWindowIsOpen = !infoWindowIsOpen;
            infoWindowOpenCloseEvent.Invoke(infoWindowIsOpen);
        }
    }
}