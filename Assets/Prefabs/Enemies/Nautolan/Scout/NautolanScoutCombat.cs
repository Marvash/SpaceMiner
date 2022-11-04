using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanScoutCombat : MonoBehaviour
{
    [SerializeField]
    float health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("PlayerLaserProjectile"))
        {
            LaserProjectileProperties laserProjectileProperties = other.GetComponent<LaserProjectileProperties>();
            health -= laserProjectileProperties.damage;
            if(health <= 0.0f)
            {
                Destroy(gameObject);
            }
            Destroy(other);
        }
    }
}
