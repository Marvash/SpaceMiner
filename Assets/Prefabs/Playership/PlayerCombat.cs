using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    GameObject laserProjectile;

    [SerializeField]
    GameObject laserGun1;

    [SerializeField]
    GameObject laserGun2;

    [SerializeField]
    GameObject laserGun3;

    [SerializeField]
    GameObject laserGun4;

    [SerializeField]
    GameObject laserGun5;

    [SerializeField]
    float shotInterval;

    [SerializeField]
    float projectileSpeed;

    private bool shooting;

    private float lastShot;

    // Start is called before the first frame update
    void Start()
    {
        lastShot = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(shooting)
        {
            shootProjectile();
        }
    }

    private void shootProjectile()
    {
        float currentTime = Time.time;
        if ((currentTime - lastShot) >= shotInterval)
        {
            
            GameObject projectile = Instantiate(laserProjectile, laserGun1.transform.position, transform.rotation);
            LaserProjectileProperties projectileProperties = projectile.GetComponent<LaserProjectileProperties>();
            projectileProperties.projectileSpeed = projectileSpeed;
            projectileProperties.shooter = gameObject;
            projectileProperties.damage = 10.0f;
            projectile = Instantiate(laserProjectile, laserGun2.transform.position, transform.rotation);
            projectileProperties = projectile.GetComponent<LaserProjectileProperties>();
            projectileProperties.projectileSpeed = projectileSpeed;
            projectileProperties.shooter = gameObject;
            projectileProperties.damage = 10.0f;
            projectile = Instantiate(laserProjectile, laserGun3.transform.position, transform.rotation);
            projectileProperties = projectile.GetComponent<LaserProjectileProperties>();
            projectileProperties.projectileSpeed = projectileSpeed;
            projectileProperties.shooter = gameObject;
            projectileProperties.damage = 10.0f;
            projectile = Instantiate(laserProjectile, laserGun4.transform.position, transform.rotation);
            projectileProperties = projectile.GetComponent<LaserProjectileProperties>();
            projectileProperties.projectileSpeed = projectileSpeed;
            projectileProperties.shooter = gameObject;
            projectileProperties.damage = 10.0f;
            projectile = Instantiate(laserProjectile, laserGun5.transform.position, transform.rotation);
            projectileProperties = projectile.GetComponent<LaserProjectileProperties>();
            projectileProperties.projectileSpeed = projectileSpeed;
            projectileProperties.shooter = gameObject;
            projectileProperties.damage = 10.0f;
            //GameObject projectile = Instantiate(laserProjectile, laserGun5.transform.position, transform.rotation);
            //LaserProjectileProperties projectileProperties = projectile.GetComponent<LaserProjectileProperties>();
            //projectileProperties.projectileSpeed = projectileSpeed;
            //projectileProperties.shooter = gameObject;
            //projectileProperties.damage = 10.0f;
            lastShot = currentTime;
        }
    }

    public void PlayerShoot(InputAction.CallbackContext context)
    {
        shooting = context.ReadValueAsButton();
    }
}
