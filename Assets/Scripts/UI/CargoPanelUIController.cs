using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoPanelUIController : MonoBehaviour
{
    [SerializeField]
    private PickupCargoSO PickupCargoSO;

    [SerializeField]
    private GameObject CargoSlotGO;

    private GameObject[] _cargoSlots;

    // Start is called before the first frame update
    void Start()
    {
        resizeCargo(PickupCargoSO.cargoSlotCount);
        resetCargo();
        PickupCargoSO.cargoSlotChangeEvent.AddListener(onCargoSlotChangeEvent);
    }

    void onCargoSlotChangeEvent(int index, PickupStack pickup)
    {
        if (pickup != null)
        {
            _cargoSlots[index].GetComponent<CargoSlotUIController>().setSlotByPickup(pickup);
        } else
        {
            _cargoSlots[index].GetComponent<CargoSlotUIController>().resetSlot();
        }
    }

    void resizeCargo(int slotCount)
    {
        _cargoSlots = new GameObject[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            _cargoSlots[i] = Instantiate(CargoSlotGO, transform);
        }
    }
    
    void resetCargo()
    {
        for (int i = 0; i < _cargoSlots.Length; i++)
        {
            _cargoSlots[i].GetComponent<CargoSlotUIController>().resetSlot();
        }
    }
}
