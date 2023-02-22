using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWeaponActiveNode : Node
{
    private IWeapon _weapon;

    public IsWeaponActiveNode(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public override BTState Evaluate()
    {
        if (_weapon == null)
        {
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }

        if (_weapon.IsActive())
        {
            CurrentState = BTState.SUCCESS;
        } else
        {
            CurrentState = BTState.FAILURE;
        }
        return CurrentState;
    }
}
