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

    private void Start()
    {
        InterruptCharge();
    }

    private void Update()
    {
        SimpleMovementBT.ProjectileSpeed = ChargedLaserCannonArray.GetCurrentProjectileSpeed();
    }

    public override void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
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
