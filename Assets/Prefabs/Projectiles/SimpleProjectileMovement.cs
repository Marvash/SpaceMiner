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
    private Rigidbody2D _rb;

    private Vector3 _currentPlayerPosition = Vector3.zero;

    private void Start()
    {
        _player = PlayerManager.Instance.Player;
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 movementVector = new Vector2(transform.up.x, transform.up.y) * ProjectileSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + movementVector;
        _rb.MovePosition(new Vector3(newPosition.x, newPosition.y, transform.position.z));

    }

    private void Update()
    {
        if(_player != null)
        {
            _currentPlayerPosition = _player.transform.position;

        }
        if (((Vector2)transform.position - (Vector2)_currentPlayerPosition).magnitude > MaxDistanceFromPlayer)
        {
            Destroy(gameObject);
        }
    }
}
