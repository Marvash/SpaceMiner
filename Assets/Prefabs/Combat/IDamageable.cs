using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDamageable : MonoBehaviour
{
    [SerializeField]
    public float Health;

    public abstract void TakeDamage(float damage);
}
