using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanFighterBehaviour : IDamageable
{
    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

    [SerializeField]
    private SimpleMovementBT SimpleMovementBT;

    [SerializeField]
    private NautolanFighterCombatBT NautolanFighterCombatBT;

    private GameObject _target;

    private void Awake()
    {
        SetTarget(PlayershipManagerSO.Player);
    }

    private void _setTargetVariables()
    {
        SimpleMovementBT.Target = _target;
        NautolanFighterCombatBT.Target = _target;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _setTargetVariables();
        Debug.Log("Set target " + _target);
    }
}
