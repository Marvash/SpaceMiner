using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDamageable : MonoBehaviour
{
    public float MaxHealth;

    protected float _currentHealth;

    private bool _invulnerability = false;
    public bool Invulnerable { get => _invulnerability; }

    private float _invulnerabilityTime;

    private float _InvulnerabilityTimeSpan;

    protected virtual void Start()
    {
        _currentHealth = MaxHealth;
    }

    private void Update()
    {
        if ((Time.time - _invulnerabilityTime) > _InvulnerabilityTimeSpan)
        {
            _invulnerability = false;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if(!_invulnerability) {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetInvulnerable(float time)
    {
        _InvulnerabilityTimeSpan = time;
        _invulnerabilityTime = Time.time;
        _invulnerability = true;
    }
}
