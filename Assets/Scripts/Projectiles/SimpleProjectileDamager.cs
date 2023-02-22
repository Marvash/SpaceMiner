using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectileDamager : IDamager
{
    [SerializeField]
    public float ProjectileDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (matchesImpactLayers(other.layer) )
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if(damageable != null)
            {
                if (collision.isTrigger)
                {
                    DealDamage(damageable, ProjectileDamage);
                    if (!damageable.Invulnerable)
                    {
                        Destroy(gameObject);
                    }
                }
            } else
            {
                Destroy(gameObject);
            }
        }
    }
}
