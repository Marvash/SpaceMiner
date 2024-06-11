using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CargoPlayerDataSO", menuName = "ScriptableObjects/CargoPlayerDataSO", order = 3)]
public class CargoPlayerDataSO : ScriptableObject
{
    [SerializeField]
    public int cargoSlotCount;

    private PickupStack[] pickupCargo;
    public UnityEvent<int, PickupStack> OnCargoSlotChanged = new UnityEvent<int, PickupStack>();
    public UnityEvent<PickupStack[]> OnCargoChanged = new UnityEvent<PickupStack[]>();

    private void OnEnable()
    {
        pickupCargo = new PickupStack[cargoSlotCount];
    }

    public void ResetCargo()
    {
        for (int i = 0; i < cargoSlotCount; i++)
        {
            pickupCargo[i] = null;
            OnCargoSlotChanged.Invoke(i, null);
        }
        OnCargoChanged.Invoke(pickupCargo);
    }

    public PickupStack[] GetCargo()
    {
        return pickupCargo;
    }

    public void AddPickupStackToCargo(PickupStack pickupStack)
    {
        int existingPickupIndex = FindPickupIndexInCargoByPickupId(pickupStack.pickupSO.pickupId);
        if (existingPickupIndex != -1)
        {
            pickupCargo[existingPickupIndex].stackCount += pickupStack.stackCount;
            OnCargoSlotChanged.Invoke(existingPickupIndex, pickupCargo[existingPickupIndex]);
            OnCargoChanged.Invoke(pickupCargo);
        }
        else
        {
            int nextFreeCargoSlotIndex = GetNextFreeCargoSlot();
            if (nextFreeCargoSlotIndex != -1)
            {
                pickupCargo[nextFreeCargoSlotIndex] = pickupStack;
                OnCargoSlotChanged.Invoke(nextFreeCargoSlotIndex, pickupCargo[nextFreeCargoSlotIndex]);
                OnCargoChanged.Invoke(pickupCargo);
            }
        }
    }

    public void DropSinglePickupFromStack(PickupStack pickupStack)
    {
        int existingPickupIndex = FindPickupIndexInCargoByPickupId(pickupStack.pickupSO.pickupId);
        if (existingPickupIndex != -1)
        {
            pickupCargo[existingPickupIndex].stackCount--;
            if(pickupCargo[existingPickupIndex].stackCount == 0) {
                pickupCargo[existingPickupIndex] = null;
            }
            OnCargoSlotChanged.Invoke(existingPickupIndex, pickupCargo[existingPickupIndex]);
            OnCargoChanged.Invoke(pickupCargo);
        }
    }

    private int FindPickupIndexInCargoByPickupId(PickupSO.PickupId pickupId)
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

    private int GetNextFreeCargoSlot()
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

    public bool CargoHasFreeSlot()
    {
        return GetNextFreeCargoSlot() != -1;
    }

    public bool CanAddPickupToCargo(PickupStack pickupStack)
    {
        return CargoHasFreeSlot() || (FindPickupIndexInCargoByPickupId(pickupStack.pickupSO.pickupId) != -1); 
    }
    
    public void RemoveCargoItemByIndex(int index) {
        if(pickupCargo[index] != null) {
            pickupCargo[index] = null;
            OnCargoSlotChanged.Invoke(index, null);
            OnCargoChanged.Invoke(pickupCargo);
        }
    }
}
