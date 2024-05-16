using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoPanelUIController : MonoBehaviour
{
    [SerializeField] 
    CargoChangeEventChannelSO cargoChangeEvent;
    [SerializeField]
    private GameObject CargoSlotGO;

    private List<GameObject> _cargoSlots = new List<GameObject>();


    void Awake()
    { 
        cargoChangeEvent.OnCargoChanged.AddListener(OnCargoSlotChangeEvent);
    }

    void OnCargoSlotChangeEvent(PickupStack[] pickups)
    {
        int currentUISlotsCount = _cargoSlots.Count;
        for(int i = 0; i < pickups.Length; i++) {
            if(i >= currentUISlotsCount) {
                _cargoSlots.Add(Instantiate(CargoSlotGO, transform));
            }
            CargoSlotUIController cargoSlotUIController = _cargoSlots[i].GetComponent<CargoSlotUIController>();
            if(pickups[i] != null) {
                cargoSlotUIController.setSlotByPickup(pickups[i]);
            } else {
                cargoSlotUIController.resetSlot();
            }
        }
        while(pickups.Length < _cargoSlots.Count) {
            _cargoSlots.RemoveAt(_cargoSlots.Count - 1);
        }
    }
}
