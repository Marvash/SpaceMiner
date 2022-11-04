using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 movementVector;
    Vector2 desiredLookVector;
    Vector2 pointerWorldCoords;
    Rigidbody2D playerRigidBody;

    [SerializeField]
    LineRenderer lookLineRenderer;

    [SerializeField]
    LineRenderer forwardLineRenderer;
    
    [SerializeField]
    Camera mainCam;

    [SerializeField]
    float maxVelocity;

    [SerializeField]
    float accelerationFactor;

    [SerializeField]
    float dragFactor;

    void Start()
    {
        movementVector = new Vector2(0, 0);
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.drag = dragFactor;
    }

    void FixedUpdate()
    {
        playerRigidBody.AddForce(movementVector * Time.fixedDeltaTime * accelerationFactor);
        float angleLook = Mathf.Atan2(desiredLookVector.y, desiredLookVector.x) * Mathf.Rad2Deg;
        playerRigidBody.MoveRotation(angleLook - 90.0f);
    }

    private void Update()
    {
        pointerWorldCoords = mainCam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, mainCam.nearClipPlane));
        desiredLookVector = new Vector2(pointerWorldCoords.x, pointerWorldCoords.y) - new Vector2(transform.position.x, transform.position.y);
    }

    private void LateUpdate()
    {
        Vector2 playerVelocity = playerRigidBody.velocity;
        if (playerVelocity.magnitude > maxVelocity)
        {
            playerRigidBody.velocity = Vector2.ClampMagnitude(playerVelocity, maxVelocity);
        }

        UpdateLineRenderers();

    }

    public void MovementActionHandler(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
        if(movementVector.sqrMagnitude > 0)
        {
            playerRigidBody.drag = 0;
        } else
        {
            playerRigidBody.drag = dragFactor;
        }
    }

    private void UpdateLineRenderers()
    {
        Vector3 spaceShipPos = transform.position;
        Vector2 forwardDirScaled = (transform.up * 100) + spaceShipPos;
        forwardLineRenderer.SetPosition(0, new Vector3(spaceShipPos.x, spaceShipPos.y, 0));
        forwardLineRenderer.SetPosition(1, new Vector3(forwardDirScaled.x, forwardDirScaled.y, 0));
        lookLineRenderer.SetPosition(0, new Vector3(spaceShipPos.x, spaceShipPos.y, 0));
        Vector2 lookTargetPoint = desiredLookVector + new Vector2(spaceShipPos.x, spaceShipPos.y);
        lookLineRenderer.SetPosition(1, new Vector3(lookTargetPoint.x, lookTargetPoint.y, 0));
    }
}
