using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanScoutBehaviour : IDamageable
{
    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

    [SerializeField]
    private SimpleMovementBT SimpleMovementBT;

    [SerializeField]
    private NautolanScoutCombatBT NautolanScoutCombatBT;

    private GameObject _target;

    private void Awake()
    {
        SetTarget(PlayershipManagerSO.Player);
    }

    private void _setTargetVariables()
    {
        SimpleMovementBT.Target = _target;
        NautolanScoutCombatBT.Target = _target;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _setTargetVariables();
    }
}
