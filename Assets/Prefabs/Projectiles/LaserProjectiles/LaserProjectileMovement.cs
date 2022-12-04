using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectileMovement : MonoBehaviour
{
    LaserProjectileProperties properties;

    // Start is called before the first frame update
    void Start()
    {
        properties = GetComponent<LaserProjectileProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = new Vector2(transform.up.x, transform.up.y) * properties.projectileSpeed * Time.deltaTime;
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + movementVector;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        if(((Vector2)transform.position - properties.shooterPosition).magnitude > 8)
        {
            Destroy(gameObject);
        }
    }
}
