using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanFighterEnemy : ISimpleEnemy
{
    [SerializeField]
    private SimpleMovementBT SimpleMovementBT;

    [SerializeField]
    private NautolanFighterCombatBT NautolanFighterCombatBT;

    private void _setTargetVariables()
    {
        SimpleMovementBT.Target = _target;
        NautolanFighterCombatBT.Target = _target;
    }

    public override void SetTarget(GameObject target)
    {
        _target = target;
        _setTargetVariables();
    }

    public override void ActivateEnemy()
    {
        SimpleMovementBT.enabled = true;
        NautolanFighterCombatBT.enabled = true;
    }

    public override void DeactivateEnemy()
    {
        SimpleMovementBT.enabled = false;
        NautolanFighterCombatBT.enabled = false;
    }
}
