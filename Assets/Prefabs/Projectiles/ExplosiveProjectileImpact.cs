using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectileImpact : MonoBehaviour
{
    [SerializeField]
    private GameObject ExplosionVFXGO;

    [SerializeField]
    public float ProjectileAreaDamage;

    [SerializeField]
    private List<int> ImpactLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (matchesImpactLayers(other.layer))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(ProjectileAreaDamage);
            }
            GameObject missileExplosionGOInstance = Instantiate(ExplosionVFXGO, transform.position, Quaternion.identity);
            missileExplosionGOInstance.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }

    private bool matchesImpactLayers(int layer)
    {
        foreach(int impactLayer in ImpactLayers)
        {
            if(layer == impactLayer)
            {
                return true;
            }
        }
        return false;
    }
}
