using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MineralItemUI : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<GameObject> SoldSignal = new UnityEvent<GameObject>();

    [SerializeField]
    private Image PickupImage;

    [SerializeField]
    private TextMeshProUGUI PickupNameText;

    [SerializeField]
    private TextMeshProUGUI PickupQtyText;

    [SerializeField]
    private TextMeshProUGUI PickupValueText;

    [SerializeField]
    private TextMeshProUGUI PickupTotalText;

    private PickupStack _currentPickupStack = null;

    public void SetPickupStack(PickupStack ps)
    {
        PickupImage.sprite = ps.pickupSO.sprite;
        PickupNameText.text = ps.pickupSO.pickupName;
        PickupQtyText.text = ""+ps.stackCount;
        PickupValueText.text = ps.stackCount + " x " + ps.pickupSO.value + " $";
        PickupTotalText.text = "" + (ps.stackCount * ps.pickupSO.value) + " $";
        _currentPickupStack = ps;
    }

    public void SellItemHandler()
    {
        SoldSignal.Invoke(gameObject);
    }

    public PickupStack GetCurrentPickupStack()
    {
        return _currentPickupStack;
    }
}
