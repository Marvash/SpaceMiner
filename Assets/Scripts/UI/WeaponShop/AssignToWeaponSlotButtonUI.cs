using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AssignToWeaponSlotButtonUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI buttonText;
    int slotIndex = -1;

    public UnityEvent<int> OnAssignToSlot = new UnityEvent<int>();

    public void SetButtonText(int slotNumber) {
        this.slotIndex = slotNumber-1;
        buttonText.text = slotNumber.ToString();
    }

    public void HandleAssigntoSlotClick() {
        OnAssignToSlot.Invoke(slotIndex);
    }
}
