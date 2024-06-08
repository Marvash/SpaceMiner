using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayershipMenuCanvasUIController : MonoBehaviour
{
    [SerializeField]
    private GameplayMenuControllerSO GameplayMenuControllerSO;

    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

    private Canvas _canvas;

    [SerializeField]
    private TextMeshProUGUI MoneyText;

    [SerializeField]
    private float ExtremeEncumbermentMultiplier = 0.2f;
    [SerializeField]
    private float OverEncumbermentMultiplier = 0.4f;
    [SerializeField]
    private float MaxEncumbermentMultiplier = 0.6f;

    void Awake()
    {
        _canvas = GetComponent<Canvas>();
        GameplayMenuControllerSO.OpenPlayershipMenuEvent.AddListener(handleOpenPlayershipMenu);
        GameplayMenuControllerSO.ClosePlayershipMenuEvent.AddListener(handleClosePlayershipMenu);
        PlayershipManagerSO.MoneyUpdateEvent.AddListener(handleMoneyUpdate);
    }

    private void handleOpenPlayershipMenu()
    {
        _canvas.enabled = true;
    }

    private void handleClosePlayershipMenu()
    {
        _canvas.enabled = false;
    }

    private void handleMoneyUpdate(int currentMoney)
    {
        MoneyText.text = currentMoney + " $";
    }
}
