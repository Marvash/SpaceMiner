using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class StopFireLaserCannonArrayNode : Node
{
    private LaserCannonArray _laserCannonArray;

    public StopFireLaserCannonArrayNode(LaserCannonArray laserCannonArray)
    {
        _laserCannonArray = laserCannonArray;
    }
    public override BTState Evaluate()
    {
        if (_laserCannonArray == null)
        {
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }

        _laserCannonArray.ShootEnd();
        CurrentState = BTState.SUCCESS;
        return CurrentState;
    }
}
