using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupLibraryScriptableObject", menuName = "ScriptableObjects/PickupLibraryScriptableObject", order = 2)]
public class PickupLibraryScriptableObject : ScriptableObject
{
    private Dictionary<PickupScriptableObject.PickupId, PickupScriptableObject> pickupLibrary;

    public void init()
    {
        pickupLibrary = new Dictionary<PickupScriptableObject.PickupId, PickupScriptableObject>();

        PickupScriptableObject[] SOList = Resources.LoadAll<PickupScriptableObject>("PickupSO");

        foreach (PickupScriptableObject pickupSO in SOList)
        {
            pickupLibrary.Add(pickupSO.pickupId, pickupSO);
        }
    }

    public PickupScriptableObject getPickupSO(PickupScriptableObject.PickupId pickupId)
    {
        return pickupLibrary[pickupId];
    }
}
