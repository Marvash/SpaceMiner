using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayershipUI : MonoBehaviour, IGameUI
{
    private Canvas canvas;

    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private TextMeshProUGUI weightText;

    [SerializeField]
    private Transform cargoPanelTransform;

    [SerializeField]
    private GameObject cargoSlotGO;

    public UnityEvent<IGameUI> OnActivateUI { get; private set;}

    public UnityEvent<IGameUI> OnDeactivateUI { get; private set;}

    public GameInputControls InputControls { get; private set;}

    public bool IsActive { get; private set;}
    [field:SerializeField]
    public int Priority { get; set; }

    private List<CargoSlotUI> UICargoSlots = new List<CargoSlotUI>();
    
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        OnActivateUI = new UnityEvent<IGameUI>();
        OnDeactivateUI = new UnityEvent<IGameUI>();
        InputControls = GameInputControls.PlayershipMenu;
        IsActive = false;
    }

    public void UpdateCargo(PickupStack[] cargo) {
        for(int i = 0; i < cargo.Length; i++) {
            GameObject newCargoSlotGO = Instantiate(cargoSlotGO, cargoPanelTransform);
            CargoSlotUI cargoSlotUI = newCargoSlotGO.GetComponent<CargoSlotUI>();
            UICargoSlots.Add(cargoSlotUI);
            if(cargo[i] != null) {
                cargoSlotUI.SetSlotByPickup(cargo[i]);
            } else {
                cargoSlotUI.ResetSlot();
            }
        }
    }

    public void UpdateMoney(int money) {
        moneyText.text = money + " $";
    }

    public void UpdateWeight(float currentWeight, float maxWeight, Color textColor) {
        weightText.text = (int)currentWeight + " / " + (int)maxWeight;
        weightText.color = textColor;
    }

    public void ResetCargoUI() {
        foreach(CargoSlotUI cargoSlot in UICargoSlots) {
            Destroy(cargoSlot.gameObject);
        }
        UICargoSlots.Clear();
    }

    public void ActivateUI()
    {
        canvas.enabled = true;
        IsActive = true;
        OnActivateUI.Invoke(this);
    }

    public void DeactivateUI()
    {
        OnDeactivateUI.Invoke(this);
        canvas.enabled = false;
        IsActive = false;
    }
}
