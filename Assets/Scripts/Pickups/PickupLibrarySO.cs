using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupLibrarySO", menuName = "ScriptableObjects/PickupLibrarySO", order = 2)]
public class PickupLibrarySO : ScriptableObject
{
    private Dictionary<PickupSO.PickupId, PickupSO> pickupLibrary;

    public void init()
    {
        pickupLibrary = new Dictionary<PickupSO.PickupId, PickupSO>();

        PickupSO[] SOList = Resources.LoadAll<PickupSO>("PickupSO");

        foreach (PickupSO pickupSO in SOList)
        {
            pickupLibrary.Add(pickupSO.pickupId, pickupSO);
        }
    }

    public PickupSO getPickupSO(PickupSO.PickupId pickupId)
    {
        return pickupLibrary[pickupId];
    }
}
