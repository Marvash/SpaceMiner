using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class StartFireChargedLaserCannonNode : Node
{
    private ChargedLaserCannonArray _chargedLaserCannonArray;

    public StartFireChargedLaserCannonNode(ChargedLaserCannonArray laserCannonArray)
    {
        _chargedLaserCannonArray = laserCannonArray;
    }
    public override BTState Evaluate()
    {
        if (_chargedLaserCannonArray == null)
        {
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }

        _chargedLaserCannonArray.ShootBegin();
        CurrentState = BTState.SUCCESS;
        return CurrentState;
    }
}
