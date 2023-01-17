using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 movementVector;
    Vector2 desiredLookVector;
    Vector2 pointerWorldCoords;
    Rigidbody2D playerRigidBody;

    [SerializeField]
    private LineRenderer LookLineRenderer;

    [SerializeField]
    private LineRenderer ForwardLineRenderer;

    [SerializeField]
    private Camera MainCam;

    [SerializeField]
    private float MaxVelocity;

    [SerializeField]
    private float AccelerationFactor;

    [SerializeField]
    private float DragFactor;

    [SerializeField]
    private float EnergyConsumptionTickRate;

    [SerializeField]
    private float EnergyConsumptionPerTickAmount;

    [SerializeField]
    private EnergyBehaviour EnergyBehaviour;

    [SerializeField]
    private float BoostForceMultiplier;

    [SerializeField]
    private int BoostCycles;

    [SerializeField]
    private float BoostCooldown;

    [SerializeField]
    private float BoostEnergyConsumption;

    [SerializeField]
    private float InvulnerabilityWindow;

    [SerializeField]
    private IDamageable InvulnerabilityTarget;

    private bool _engineOn = false;

    private bool _boost = false;

    private float _boostCooldownTime;

    private int _currentBoostCycle = 0;

    private bool _boostReset = true;

    void Start()
    {
        movementVector = new Vector2(0, 0);
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.drag = DragFactor;
        playerRigidBody.centerOfMass = Vector2.zero;
        _boostCooldownTime = -BoostCooldown;
    }

    void FixedUpdate()
    {
        if ((movementVector.sqrMagnitude != 0.0f))
        {
            if (!_engineOn)
            {
                if (consumeMovementEnergy())
                {
                    InvokeRepeating("consumeMovementEnergy", EnergyConsumptionTickRate, EnergyConsumptionTickRate);
                    float boost = 1.0f;
                    if (_boost)
                    {
                        if (_boostReset && (_currentBoostCycle == 0) && (Time.time - _boostCooldownTime) > BoostCooldown)
                        {
                            if (consumeBoostEnergy())
                            {
                                _currentBoostCycle = BoostCycles;
                                _boostCooldownTime = Time.time;
                                _boostReset = false;
                                InvulnerabilityTarget.SetInvulnerable(InvulnerabilityWindow);
                            }
                        }
                        if (_currentBoostCycle > 0)
                        {
                            boost = BoostForceMultiplier;
                            _currentBoostCycle--;
                        }
                    }
                    playerRigidBody.AddForce(movementVector * Time.fixedDeltaTime * AccelerationFactor * boost);
                    Vector2 playerVelocity = playerRigidBody.velocity;
                    if (playerVelocity.magnitude > MaxVelocity)
                    {
                        playerRigidBody.velocity = Vector2.ClampMagnitude(playerVelocity, MaxVelocity);
                    }
                    _engineOn = true;
                } else
                {
                    _engineOn = false;
                }
            }
            else
            {
                float boost = 1.0f;
                if (_boost || _currentBoostCycle > 0)
                {
                    if(_boostReset && (_currentBoostCycle == 0) && (Time.time - _boostCooldownTime) > BoostCooldown)
                    {
                        if (consumeBoostEnergy())
                        {
                            _currentBoostCycle = BoostCycles;
                            _boostCooldownTime = Time.time;
                            _boostReset = false;
                            InvulnerabilityTarget.SetInvulnerable(InvulnerabilityWindow);
                        }
                    }
                    if (_currentBoostCycle > 0)
                    {
                        boost = BoostForceMultiplier;
                        _currentBoostCycle--;
                    }
                }
                playerRigidBody.AddForce(movementVector * Time.fixedDeltaTime * AccelerationFactor * boost);
                Vector2 playerVelocity = playerRigidBody.velocity;
                if (playerVelocity.magnitude > MaxVelocity)
                {
                    playerRigidBody.velocity = Vector2.ClampMagnitude(playerVelocity, MaxVelocity);
                }
            }
        } else if(_engineOn)
        {
            CancelInvoke();
            _engineOn = false;
        }
        float angleLook = Mathf.Atan2(desiredLookVector.y, desiredLookVector.x) * Mathf.Rad2Deg;
        playerRigidBody.MoveRotation(angleLook - 90.0f);
    }

    private void Update()
    {
        pointerWorldCoords = MainCam.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, MainCam.nearClipPlane));
        desiredLookVector = new Vector2(pointerWorldCoords.x, pointerWorldCoords.y) - new Vector2(transform.position.x, transform.position.y);
    }

    private void LateUpdate()
    {
        UpdateLineRenderers();

    }

    private bool consumeMovementEnergy()
    {
        if(EnergyBehaviour.ConsumeEnergy(EnergyConsumptionPerTickAmount) == 0.0f)
        {
            CancelInvoke();
            _engineOn = false;
            return false;
        }
        return true;
    }

    private bool consumeBoostEnergy()
    {
        if (EnergyBehaviour.GetAvailableEnergy() < BoostEnergyConsumption)
        {
            return false;
        } else if(EnergyBehaviour.ConsumeEnergy(BoostEnergyConsumption) == BoostEnergyConsumption)
        {
            return true;
        }
        return false;
    }


    public void MovementActionHandler(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
        if(movementVector.sqrMagnitude > 0)
        {
            //playerRigidBody.drag = 0;
        } else
        {
            playerRigidBody.drag = DragFactor;
        }
    }

    public void BoostActionHandler(InputAction.CallbackContext context)
    {
        bool boost = context.ReadValueAsButton();
        if(_boost && !boost)
        {
            _boost = false;
        } else if(!_boost && boost)
        {
            _boost = true;
            _boostReset = true;
        }
    }

    private void UpdateLineRenderers()
    {
        Vector3 spaceShipPos = transform.position;
        Vector2 forwardDirScaled = (transform.up * 100) + spaceShipPos;
        ForwardLineRenderer.SetPosition(0, new Vector3(spaceShipPos.x, spaceShipPos.y, 0));
        ForwardLineRenderer.SetPosition(1, new Vector3(forwardDirScaled.x, forwardDirScaled.y, 0));
        LookLineRenderer.SetPosition(0, new Vector3(spaceShipPos.x, spaceShipPos.y, 0));
        Vector2 lookTargetPoint = desiredLookVector + new Vector2(spaceShipPos.x, spaceShipPos.y);
        LookLineRenderer.SetPosition(1, new Vector3(lookTargetPoint.x, lookTargetPoint.y, 0));
    }
}
