using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject pickupTemplate;

    [SerializeField]
    PickupLibraryScriptableObject pickupLibrarySO;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject spawnStandardPickup(PickupScriptableObject.PickupId pickupId, int stackCount = 1)
    {
        PickupScriptableObject pickupSO = pickupLibrarySO.getPickupSO(pickupId);
        GameObject pickupGO = Instantiate(pickupTemplate, Vector3.zero, Quaternion.identity);
        pickupGO.name = pickupSO.name + "_pickup";
        SpriteRenderer spriteRenderer = pickupGO.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pickupSO.sprite;
        PickupStackScript valuablePickup = pickupGO.GetComponent<PickupStackScript>();
        valuablePickup.pickup = new PickupStack(pickupSO, stackCount);
        return pickupGO;
    }

    public GameObject spawnStandardPickup(PickupStack pickupStack)
    {
        PickupScriptableObject pickupSO = pickupStack.pickupSO;
        GameObject pickupGO = Instantiate(pickupTemplate, Vector3.zero, Quaternion.identity);
        pickupGO.name = pickupSO.name + "_pickup";
        SpriteRenderer spriteRenderer = pickupGO.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pickupSO.sprite;
        PickupStackScript valuablePickup = pickupGO.GetComponent<PickupStackScript>();
        valuablePickup.pickup = pickupStack;
        return pickupGO;
    }
}