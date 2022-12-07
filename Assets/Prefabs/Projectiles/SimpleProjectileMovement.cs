using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectileMovement : MonoBehaviour
{
    [SerializeField]
    public float ProjectileSpeed;

    [SerializeField]
    public float MaxDistanceFromPlayer;

    private GameObject _player;

    private void Start()
    {
        _player = PlayerManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = new Vector2(transform.up.x, transform.up.y) * ProjectileSpeed * Time.deltaTime;
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + movementVector;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        if (((Vector2)transform.position - (Vector2)_player.transform.position).magnitude > MaxDistanceFromPlayer)
        {
            Destroy(gameObject);
        }
    }
}
