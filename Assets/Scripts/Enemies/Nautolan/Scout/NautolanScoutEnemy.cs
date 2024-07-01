using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanScoutEnemy : ISimpleEnemy
{
    [SerializeField]
    private MoneyPlayerDataSO PlayershipManagerSO;

    [SerializeField]
    private SimpleMovementBT SimpleMovementBT;

    [SerializeField]
    private NautolanScoutCombatBT NautolanScoutCombatBT;

    [SerializeField]
    private TrailRenderer _trailRenderer1;

    [SerializeField]
    private TrailRenderer _trailRenderer2;

    protected override void Awake()
    {
        _trailRenderer1 = GetComponentInChildren<TrailRenderer>();
        _trailRenderer2 = GetComponentInChildren<TrailRenderer>();
        base.Awake();
    }

    private void _setTargetVariables()
    {
        SimpleMovementBT.Target = _target;
        NautolanScoutCombatBT.Target = _target;
    }

    public override void SetTarget(GameObject target)
    {
        _target = target;
        _setTargetVariables();
    }

    public override void ActivateEnemy()
    {
        SimpleMovementBT.enabled = true;
        NautolanScoutCombatBT.enabled = true;
        _trailRenderer1.enabled = true;
        _trailRenderer2.enabled = true;
    }

    public override void DeactivateEnemy()
    {
        SimpleMovementBT.enabled = false;
        NautolanScoutCombatBT.enabled = false;
        _trailRenderer1.enabled = false;
        _trailRenderer2.enabled = false;

    }
}
