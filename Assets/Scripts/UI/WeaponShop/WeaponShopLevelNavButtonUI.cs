using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponShopLevelNavButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonText;

    public void SetButtonLevelText(int level) {
        buttonText.text = "Mk." + level.ToString();
    }
}
