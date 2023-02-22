using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDamager : MonoBehaviour
{
    [SerializeField]
    private List<int> DamageableLayers;

    public void DealDamage(IDamageable damageable, float damage)
    {
        if(damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }

    protected bool matchesImpactLayers(int layer)
    {
        foreach (int impactLayer in DamageableLayers)
        {
            if (layer == impactLayer)
            {
                return true;
            }
        }
        return false;
    }
}
