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

    // Start is called before the first frame update
    void Start()
    {
        resetSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setSlotByPickup(PickupStack pickup)
    {
        imageComponent.sprite = pickup.pickupSO.sprite;
        imageComponent.enabled = true;
        stackCountTextChild.SetActive(true);
        stackCountTextChild.GetComponent<TextMeshProUGUI>().SetText("" + pickup.stackCount);
        emptyTextChild.SetActive(false);
    }
    public void resetSlot()
    {
        imageComponent.sprite = null;
        imageComponent.enabled = false;
        stackCountTextChild.SetActive(false);
        emptyTextChild.SetActive(true);
    }
}
