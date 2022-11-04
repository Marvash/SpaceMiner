using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoPanelUI : MonoBehaviour
{
    [SerializeField]
    PickupInventoryScriptableObject pickupInventorySO;

    [SerializeField]
    GameObject cargoSlotGO;

    private GameObject[] cargoSlots;

    // Start is called before the first frame update
    void Start()
    {
        resizeCargo(pickupInventorySO.cargoSlotCount);
        resetCargo();
        pickupInventorySO.cargoSlotChangeEvent.AddListener(onCargoSlotChangeEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onCargoSlotChangeEvent(int index, PickupStack pickup)
    {
        if (pickup != null)
        {
            cargoSlots[index].GetComponent<CargoSlotUI>().setSlotByPickup(pickup);
        } else
        {
            cargoSlots[index].GetComponent<CargoSlotUI>().resetSlot();
        }
    }

    void resizeCargo(int slotCount)
    {
        cargoSlots = new GameObject[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            cargoSlots[i] = Instantiate(cargoSlotGO, transform);
        }
    }
    
    void resetCargo()
    {
        for (int i = 0; i < cargoSlots.Length; i++)
        {
            cargoSlots[i].GetComponent<CargoSlotUI>().resetSlot();
        }
    }
}
