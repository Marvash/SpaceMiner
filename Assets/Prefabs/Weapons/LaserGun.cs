using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField]
    public float LaserSpeed;

    [SerializeField]
    public float LaserDamage;

    [SerializeField]
    public GameObject LaserProjectile;

    public void ShootLaser()
    {
        GameObject projectile = Instantiate(LaserProjectile, transform.position, transform.rotation);
        LaserProjectileProperties projectileProperties = projectile.GetComponent<LaserProjectileProperties>();
        projectileProperties.projectileSpeed = LaserSpeed;
        projectileProperties.shooterPosition = gameObject.transform.position;
        projectileProperties.damage = LaserDamage;
    }
}
