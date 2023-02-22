using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupSpawnerSO", menuName = "ScriptableObjects/PickupSpawnerSO", order = 1)]
public class PickupSpawnerSO : ScriptableObject
{
    [SerializeField]
    private  GameObject PickupTemplate;

    [SerializeField]
    private PickupLibrarySO PickupLibrarySO;

    public GameObject spawnStandardPickup(PickupSO.PickupId pickupId, int stackCount = 1)
    {
        PickupSO pickupSO = PickupLibrarySO.getPickupSO(pickupId);
        GameObject pickupGO = Instantiate(PickupTemplate, Vector3.zero, Quaternion.identity);
        pickupGO.name = pickupSO.name + "_pickup";
        SpriteRenderer spriteRenderer = pickupGO.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pickupSO.sprite;
        PickupStackScript valuablePickup = pickupGO.GetComponent<PickupStackScript>();
        valuablePickup.pickup = new PickupStack(pickupSO, stackCount);
        return pickupGO;
    }

    public GameObject spawnStandardPickup(PickupStack pickupStack)
    {
        PickupSO pickupSO = pickupStack.pickupSO;
        GameObject pickupGO = Instantiate(PickupTemplate, Vector3.zero, Quaternion.identity);
        pickupGO.name = pickupSO.name + "_pickup";
        SpriteRenderer spriteRenderer = pickupGO.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pickupSO.sprite;
        PickupStackScript valuablePickup = pickupGO.GetComponent<PickupStackScript>();
        valuablePickup.pickup = pickupStack;
        return pickupGO;
    }
}