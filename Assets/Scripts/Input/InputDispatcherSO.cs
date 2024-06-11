using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum GameInputControls {
    BaseGameplay,
    ShopMenu,
    PlayershipMenu
}

[CreateAssetMenu(fileName = "InputDispatcherSO", menuName = "ScriptableObjects/InputDispatcherSO", order = 1)]
public class InputDispatcherSO : ScriptableObject, GameInput.IBaseGameplayActions, GameInput.IPlayershipMenuActions, GameInput.IShopMenuActions
{
    [SerializeField]
    private GameInput GameInput;

    public UnityAction<Vector2> Movement;
    public UnityAction<Vector2> GamepadDirection;
    public UnityAction<Vector2> MousePosition;
    public UnityAction FirePrimaryStart;
    public UnityAction FirePrimaryStop;
    public UnityAction FireSecondaryStart;
    public UnityAction FireSecondaryStop;
    public UnityAction OpenPlayershipMenu;
    public UnityAction<int> SelectWeaponSlot;
    public UnityAction CycleWeaponSlotForward;
    public UnityAction CycleWeaponSlotBackward;
    public UnityAction Boost;
    public UnityAction<float> IncDecWeaponSlot;
    public UnityAction ClosePlayershipMenu;
    public UnityAction CloseShopMenu;

    public void OnEnable()
    {
        if (GameInput == null)
        {
            GameInput = new GameInput();
            GameInput.BaseGameplay.SetCallbacks(this);
            GameInput.PlayershipMenu.SetCallbacks(this);
            GameInput.ShopMenu.SetCallbacks(this);
        }
        GameInput.BaseGameplay.Enable();
    }

    public void DisableAllControls()
    {
        GameInput.BaseGameplay.Disable();
        GameInput.PlayershipMenu.Disable();
        GameInput.ShopMenu.Disable();
    }

    public void EnableControls(GameInputControls controls, bool exclusiveInput = true) {
        if(exclusiveInput) {
            DisableAllControls();
        }
        switch(controls) {
            case GameInputControls.BaseGameplay:
                GameInput.BaseGameplay.Enable();
            break;
            case GameInputControls.PlayershipMenu:
                GameInput.PlayershipMenu.Enable();
            break;
            case GameInputControls.ShopMenu:
                GameInput.ShopMenu.Enable();
            break;
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if(Movement != null && (context.performed || context.canceled))
        {
            //Debug.Log(context.ReadValue<Vector2>() + " " + context.ReadValue<Vector2>().magnitude);
            Movement.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if (MousePosition != null && context.performed)
        {
            //Debug.Log(context.ReadValue<Vector2>());
            MousePosition.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnGamepadDirection(InputAction.CallbackContext context)
    {
        if (GamepadDirection != null && context.performed)
        {
            //Debug.Log(context);
            GamepadDirection.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnFirePrimary(InputAction.CallbackContext context)
    {
        if (FirePrimaryStart != null && context.performed)
        {
            FirePrimaryStart.Invoke();
        }
        if (FirePrimaryStop != null && context.canceled)
        {
            FirePrimaryStop.Invoke();
        }
    }

    public void OnFireSecondary(InputAction.CallbackContext context)
    {


        if (FireSecondaryStart != null && context.performed)
        {
            FireSecondaryStart.Invoke();
        }
        if (FireSecondaryStop != null && context.canceled)
        {
            FireSecondaryStop.Invoke();
        }
    }

    public void OnOpenShipMenu(InputAction.CallbackContext context)
    {

        if (OpenPlayershipMenu != null && context.performed)
        {
            OpenPlayershipMenu.Invoke();
        }
    }

    public void OnCloseShipMenu(InputAction.CallbackContext context)
    {
        if (OpenPlayershipMenu != null && context.performed)
        {
            OpenPlayershipMenu.Invoke();
        }
    }

    public void OnSelectWeaponSlot1(InputAction.CallbackContext context)
    {
        if (SelectWeaponSlot != null && context.performed)
        {
            SelectWeaponSlot.Invoke(1);
        }
    }

    public void OnSelectWeaponSlot2(InputAction.CallbackContext context)
    {
        if (SelectWeaponSlot != null && context.performed)
        {
            SelectWeaponSlot.Invoke(2);
        }
    }

    public void OnSelectWeaponSlot3(InputAction.CallbackContext context)
    {
        if (SelectWeaponSlot != null && context.performed)
        {
            SelectWeaponSlot.Invoke(3);
        }
    }

    public void OnSelectWeaponSlot4(InputAction.CallbackContext context)
    {
        if (SelectWeaponSlot != null && context.performed)
        {
            SelectWeaponSlot.Invoke(4);
        }
    }

    public void OnCycleWeaponSlotForward(InputAction.CallbackContext context)
    {
        if (CycleWeaponSlotForward != null && context.performed)
        {
            CycleWeaponSlotForward.Invoke();
        }
    }

    public void OnCycleWeaponSlotBackward(InputAction.CallbackContext context)
    {
        if (CycleWeaponSlotBackward != null && context.performed)
        {
            CycleWeaponSlotBackward.Invoke();
        }
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (Boost != null && context.performed)
        {
            Boost.Invoke();
        }
    }

    public void OnIncDecWeaponSlot(InputAction.CallbackContext context)
    {
        if(IncDecWeaponSlot != null && context.performed)
        {
            IncDecWeaponSlot.Invoke(context.ReadValue<float>());
        }
    }

    public void OnClosePlayershipMenu(InputAction.CallbackContext context)
    {
        if(ClosePlayershipMenu != null && context.performed)
        {
            ClosePlayershipMenu.Invoke();
        }
    }

    public void OnCloseShopMenu(InputAction.CallbackContext context)
    {
        if(CloseShopMenu != null && context.performed)
        {
            CloseShopMenu.Invoke();
        }
    }
}
