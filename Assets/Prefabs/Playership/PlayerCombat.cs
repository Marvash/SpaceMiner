using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    LaserGun laserGun1;

    [SerializeField]
    LaserGun laserGun2;

    [SerializeField]
    LaserGun laserGun3;

    [SerializeField]
    LaserGun laserGun4;

    [SerializeField]
    LaserGun laserGun5;

    [SerializeField]
    float shotInterval;

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

            laserGun1.ShootLaser();
            laserGun2.ShootLaser();
            laserGun3.ShootLaser();
            laserGun4.ShootLaser();
            laserGun5.ShootLaser();
            lastShot = currentTime;
        }
    }

    public void PlayerShoot(InputAction.CallbackContext context)
    {
        shooting = context.ReadValueAsButton();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("EnemyProjectile"))
        {
            LaserProjectileProperties laserProjectileProperties = other.GetComponent<LaserProjectileProperties>();
            Destroy(other);
        }
    }
}
