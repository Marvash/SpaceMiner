using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayershipMenuUIControllerSO", menuName = "ScriptableObjects/PlayershipMenuUIControllerSO", order = 1)]
public class PlayershipMenuUIControllerSO : ScriptableObject
{
    [HideInInspector]
    public UnityEvent OpenPlayerShipMenuEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent ClosePlayerShipMenuEvent = new UnityEvent();

    public void OpenPlayerShipMenu()
    {
        OpenPlayerShipMenuEvent.Invoke();
    }

    public void ClosePlayerShipMenu()
    {
        ClosePlayerShipMenuEvent.Invoke();
    }
}
