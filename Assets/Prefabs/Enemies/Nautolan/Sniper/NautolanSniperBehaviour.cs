using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanSniperBehaviour : IDamageable
{
    [SerializeField]
    private ChargedLaserCannonArray ChargedLaserCannonArray;

    [SerializeField]
    private SimpleMovementBT SimpleMovementBT;

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
        PsCharge1.Clear();
        PsCharge2.Stop();
        PsCharge2.Clear();
        PsChargeLaser.Stop();
        PsChargeLaser.Clear();
    }

    public void MaxCharge()
    {
        PsCharge1.Stop();
        PsCharge2.Stop();
    }
}
