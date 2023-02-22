using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayershipMenuCanvasUI : MonoBehaviour
{
    [SerializeField]
    private PlayershipMenuUIControllerSO PlayershipMenuUIControllerSO;

    private Canvas _canvas;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        PlayershipMenuUIControllerSO.OpenPlayerShipMenuEvent.AddListener(handleOpenPlayershipMenu);
        PlayershipMenuUIControllerSO.ClosePlayerShipMenuEvent.AddListener(handleClosePlayershipMenu);
    }

    void handleOpenPlayershipMenu()
    {
        _canvas.enabled = true;
    }

    void handleClosePlayershipMenu()
    {
        _canvas.enabled = false;
    }
}
