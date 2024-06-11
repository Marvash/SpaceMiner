using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoSlotUI : MonoBehaviour
{
    [SerializeField]
    private Image imageComponent;

    [SerializeField]
    private GameObject emptyTextChild;
    
    [SerializeField]
    private GameObject stackCountTextChild;

    public void SetSlotByPickup(PickupStack pickup)
    {
        imageComponent.sprite = pickup.pickupSO.sprite;
        imageComponent.enabled = true;
        stackCountTextChild.SetActive(true);
        stackCountTextChild.GetComponent<TextMeshProUGUI>().SetText("" + pickup.stackCount);
        emptyTextChild.SetActive(false);
    }
    public void ResetSlot()
    {
        imageComponent.sprite = null;
        imageComponent.enabled = false;
        stackCountTextChild.SetActive(false);
        emptyTextChild.SetActive(true);
    }
}
