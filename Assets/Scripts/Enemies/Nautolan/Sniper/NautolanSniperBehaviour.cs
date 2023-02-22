using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanSniperBehaviour : IDamageable
{
    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

    [SerializeField]
    private SimpleMovementBT SimpleMovementBT;

    [SerializeField]
    private NautolanSniperCombatBT NautolanSniperCombatBT;

    private GameObject _target;

    [SerializeField]
    private ChargedLaserCannonArray ChargedLaserCannonArray;

    [SerializeField]
    private ParticleSystem PsCharge1;

    [SerializeField]
    private ParticleSystem PsCharge2;

    [SerializeField]
    private ParticleSystem PsChargeLaser;

    private void Awake()
    {
        SetTarget(PlayershipManagerSO.Player);
    }
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

    public void SetTarget(GameObject target)
    {
        _target = target;
        _setTargetVariables();
    }
}
