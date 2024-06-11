using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameUIManagerSO", menuName = "ScriptableObjects/GameUIManagerSO", order = 1)]
public class GameUIManagerSO : ScriptableObject
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;
    List<IGameUI> ActiveUIs = new List<IGameUI>();

    public void ActivateUI(IGameUI toActivate) {
        if(ActiveUIs.Contains(toActivate)) {
            return;
        }
        ActiveUIs.Add(toActivate);
        if(ActiveUIs.Count == 1) {
            InputDispatcherSO.EnableControls(toActivate.InputControls);
        }
        toActivate.ActivateUI();
        toActivate.OnDeactivateUI.AddListener(HandleUIDeactivated);
    }

    public void DeactivateUI(IGameUI toDeactivate) {
        if(ActiveUIs.Contains(toDeactivate)) {
            toDeactivate.OnDeactivateUI.RemoveListener(HandleUIDeactivated);
            ActiveUIs.Remove(toDeactivate);
            toDeactivate.DeactivateUI();
            if(ActiveUIs.Count == 0) {
                InputDispatcherSO.EnableControls(GameInputControls.BaseGameplay);
            } else {
                InputDispatcherSO.EnableControls(ActiveUIs[^1].InputControls);
            }
        }
    }

    private void HandleUIDeactivated(IGameUI deactivatedUI) {
        deactivatedUI.OnDeactivateUI.RemoveListener(HandleUIDeactivated);
        ActiveUIs.Remove(deactivatedUI);
        if(ActiveUIs.Count == 0) {
            InputDispatcherSO.EnableControls(GameInputControls.BaseGameplay);
        } else {
            InputDispatcherSO.EnableControls(ActiveUIs[^1].InputControls);
        }
    }

    public void ActivateUIWithPriority(IGameUI toActivate) {
        for(int i = ActiveUIs.Count - 1; i >= 0; i--) {
            if(ActiveUIs[i].Priority <= toActivate.Priority) {
                DeactivateUI(ActiveUIs[i]);
            }
        }
        ActivateUI(toActivate);
    }
}
