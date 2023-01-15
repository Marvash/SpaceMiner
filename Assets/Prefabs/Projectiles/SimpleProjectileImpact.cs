using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectileImpact : MonoBehaviour
{
    [SerializeField]
    public float ProjectileDamage;

    [SerializeField]
    private List<int> ImpactLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (matchesImpactLayers(other.layer) && collision.isTrigger)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(ProjectileDamage);
            }
            Destroy(gameObject);
        }
    }

    private bool matchesImpactLayers(int layer)
    {
        foreach (int impactLayer in ImpactLayers)
        {
            if (layer == impactLayer)
            {
                return true;
            }
        }
        return false;
    }
}
