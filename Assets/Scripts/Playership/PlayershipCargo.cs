using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayershipCargo : MonoBehaviour
{
    [SerializeField]
    public int cargoSlotCount;

    private PickupStack[] _pickupCargo;
    
    [SerializeField]
    private CargoSlotChangeEventChannelSO cargoSlotChangeEventChannelSO;
    [SerializeField]
    private CargoChangeEventChannelSO cargoChangeEventChannelSO;

    private void Start()
    {
        _pickupCargo = new PickupStack[cargoSlotCount];
        ResetCargo();
    }

    private void ResetCargo()
    {
        for (int i = 0; i < cargoSlotCount; i++)
        {
            _pickupCargo[i] = null;
            if(cargoSlotChangeEventChannelSO != null) {
                cargoSlotChangeEventChannelSO.RaiseEvent(i, null);
            }
        }
        cargoChangeEventChannelSO.RaiseEvent(_pickupCargo);
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

    public void AddPickupStackToInventory(PickupStack pickupStack)
    {
        int existingPickupIndex = PickupIndexInCargo(pickupStack.pickupSO.pickupId);
        if (existingPickupIndex != -1)
        {
            _pickupCargo[existingPickupIndex].stackCount += pickupStack.stackCount;
            if(cargoSlotChangeEventChannelSO != null) {
                cargoSlotChangeEventChannelSO.RaiseEvent(existingPickupIndex, _pickupCargo[existingPickupIndex]);
                cargoChangeEventChannelSO.RaiseEvent(_pickupCargo);
            }
        }
        else
        {
            int nextFreeCargoSlotIndex = GetNextFreeCargoSlot();
            if (nextFreeCargoSlotIndex != -1)
            {
                _pickupCargo[nextFreeCargoSlotIndex] = pickupStack;
                if(cargoSlotChangeEventChannelSO != null) {
                    cargoSlotChangeEventChannelSO.RaiseEvent(nextFreeCargoSlotIndex, _pickupCargo[nextFreeCargoSlotIndex]);
                    cargoChangeEventChannelSO.RaiseEvent(_pickupCargo);
                }
            }
        }
    }

    public void DropSinglePickupFromStack(PickupStack pickupStack)
    {
        int existingPickupIndex = PickupIndexInCargo(pickupStack.pickupSO.pickupId);
        if (existingPickupIndex != -1)
        {
            _pickupCargo[existingPickupIndex].stackCount--;
            if(_pickupCargo[existingPickupIndex].stackCount == 0) {
                _pickupCargo[existingPickupIndex] = null;
            }
            if(cargoSlotChangeEventChannelSO != null) {
                cargoSlotChangeEventChannelSO.RaiseEvent(existingPickupIndex, _pickupCargo[existingPickupIndex]);
            }
        }
    }

    private int PickupIndexInCargo(PickupSO.PickupId pickupId)
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

    private int GetNextFreeCargoSlot()
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

    public bool CargoHasFreeSlot()
    {
        return GetNextFreeCargoSlot() != -1;
    }

    public bool CanAddPickupToCargo(PickupStack pickupStack)
    {
        return CargoHasFreeSlot() || (PickupIndexInCargo(pickupStack.pickupSO.pickupId) != -1); 
    }

    public void SetPickups(List<PickupStack> pickupList)
    {
        ResetCargo();
        for (int i = 0; i < cargoSlotCount; i++)
        {
            if(pickupList.Count > i)
            {
                AddPickupStackToInventory(pickupList[i]);
            } else
            {
                if(cargoSlotChangeEventChannelSO != null) {
                    cargoSlotChangeEventChannelSO.RaiseEvent(i, null);
                    cargoChangeEventChannelSO.RaiseEvent(_pickupCargo);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == 10)
        {
            PickupStackScript pickupStackScript = other.GetComponent<PickupStackScript>();
            if (pickupStackScript != null && CanAddPickupToCargo(pickupStackScript.pickup))
            {
                AddPickupStackToInventory(pickupStackScript.pickup);
                Destroy(other);
            } else
            {
                Debug.Log("CARGO FULL");
            }
        }
    }
}
