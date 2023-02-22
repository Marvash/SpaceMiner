using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoPanelUI : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void onCargoSlotChangeEvent(int index, PickupStack pickup)
    {
        if (pickup != null)
        {
            _cargoSlots[index].GetComponent<CargoSlotUI>().setSlotByPickup(pickup);
        } else
        {
            _cargoSlots[index].GetComponent<CargoSlotUI>().resetSlot();
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
            _cargoSlots[i].GetComponent<CargoSlotUI>().resetSlot();
        }
    }
}
