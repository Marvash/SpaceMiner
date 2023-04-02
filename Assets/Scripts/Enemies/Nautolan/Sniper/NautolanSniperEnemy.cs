using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanSniperEnemy : ISimpleEnemy
{

    [SerializeField]
    private SimpleMovementBT SimpleMovementBT;

    [SerializeField]
    private NautolanSniperCombatBT NautolanSniperCombatBT;

    [SerializeField]
    private ChargedLaserCannonArray ChargedLaserCannonArray;

    [SerializeField]
    private ParticleSystem PsCharge1;

    [SerializeField]
    private ParticleSystem PsCharge2;

    [SerializeField]
    private ParticleSystem PsChargeLaser;

    protected override void Start()
    {
        base.Start();
        InterruptCharge();
    }

    private void Update()
    {
        SimpleMovementBT.ProjectileSpeed = ChargedLaserCannonArray.GetCurrentProjectileSpeed();
    }

    public void StartCharge()
    {
        PsCharge1.Play();
        PsCharge2.Play();
        PsChargeLaser.Play();
    }

    public void InterruptCharge()
    {
        PsCharge1.Stop();
        //PsCharge1.Clear();
        PsCharge2.Stop();
        //PsCharge2.Clear();
        PsChargeLaser.Stop();
        PsChargeLaser.Clear();
    }

    public void MaxCharge()
    {
        PsCharge1.Stop();
        PsCharge2.Stop();
    }

    private void _setTargetVariables()
    {
        SimpleMovementBT.Target = _target;
        NautolanSniperCombatBT.Target = _target;
    }

    public override void SetTarget(GameObject target)
    {
        _target = target;
        _setTargetVariables();
    }

    public override void ActivateEnemy()
    {
        SimpleMovementBT.enabled = true;
        NautolanSniperCombatBT.enabled = true;
    }

    public override void DeactivateEnemy()
    {
        SimpleMovementBT.enabled = false;
        NautolanSniperCombatBT.enabled = false;
    }
}
