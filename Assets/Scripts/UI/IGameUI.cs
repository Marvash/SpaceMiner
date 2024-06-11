using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IGameUI
{
    GameInputControls InputControls { get; }
    UnityEvent<IGameUI> OnActivateUI { get; }
    UnityEvent<IGameUI> OnDeactivateUI { get; }
    bool IsActive { get; }
    int Priority { get; set;}
    void ActivateUI();
    void DeactivateUI();
}
