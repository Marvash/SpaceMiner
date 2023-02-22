using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectileDamager : IDamager
{
    [SerializeField]
    private GameObject ExplosionVFXGO;

    [SerializeField]
    public float ProjectileAreaDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (matchesImpactLayers(other.layer))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (collision.isTrigger)
                {
                    DealDamage(damageable, ProjectileAreaDamage);
                    if (!damageable.Invulnerable)
                    {
                        GameObject missileExplosionGOInstance = Instantiate(ExplosionVFXGO, transform.position, Quaternion.identity);
                        missileExplosionGOInstance.GetComponent<ParticleSystem>().Play();
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                GameObject missileExplosionGOInstance = Instantiate(ExplosionVFXGO, transform.position, Quaternion.identity);
                missileExplosionGOInstance.GetComponent<ParticleSystem>().Play();
                Destroy(gameObject);
            }
        }
    }
}
