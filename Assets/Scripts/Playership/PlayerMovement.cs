using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _movementVector;
    private Vector2 _desiredLookVector;
    private Vector2 _pointerWorldCoords;
    Rigidbody2D playerRigidBody;

    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private LineRenderer LookLineRenderer;

    [SerializeField]
    private LineRenderer ForwardLineRenderer;

    private Camera _mainCam;

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

    [SerializeField]
    private FloatEventChannelSO WeightMultiplierSO;

    private bool _engineOn = false;

    private bool _shouldBoost = false;

    private float _boostCooldownTime;

    private int _currentBoostCycle = 0;

    private bool _isBoosting = false;

    private float currentWeightMultiplier = 1f;

    private void Awake()
    {
        InputDispatcherSO.Movement += movementActionHandler;
        InputDispatcherSO.MousePosition += mousePositionUpdateHandler;
        InputDispatcherSO.GamepadDirection += directionActionHandler;
        InputDispatcherSO.Boost += boostActionHandler;
        WeightMultiplierSO.OnFloatChanged.AddListener(HandleWeightMovementMultiplier);
    }

    void Start()
    {
        _movementVector = new Vector2(0, 0);
        playerRigidBody = GetComponent<Rigidbody2D>();
        _mainCam = Camera.main;
        playerRigidBody.drag = DragFactor;
        playerRigidBody.centerOfMass = Vector2.zero;
        _boostCooldownTime = -BoostCooldown;
    }

    void FixedUpdate()
    {
        if ((_movementVector.sqrMagnitude != 0.0f))
        {
            if (!_engineOn)
            {
                if (consumeMovementEnergy())
                {
                    InvokeRepeating("consumeMovementEnergy", EnergyConsumptionTickRate, EnergyConsumptionTickRate);
                    float boost = 1.0f;
                    playerRigidBody.AddForce(_movementVector * Time.fixedDeltaTime * AccelerationFactor * boost * currentWeightMultiplier);
                    Vector2 playerVelocity = playerRigidBody.velocity;
                    if (playerVelocity.magnitude > MaxVelocity)
                    {
                        playerRigidBody.velocity = Vector2.ClampMagnitude(playerVelocity, MaxVelocity);
                    }
                    _engineOn = true;
                } else
                {
                    _engineOn = false;
                    _isBoosting = false;
                }
            }
            else
            {
                float boost = 1.0f;
                if(_shouldBoost)
                {
                    if(!_isBoosting && (Time.time - _boostCooldownTime) > BoostCooldown)
                    {
                        if (consumeBoostEnergy())
                        {
                            _currentBoostCycle = BoostCycles;
                            _boostCooldownTime = Time.time;
                            _isBoosting = true;
                            InvulnerabilityTarget.SetInvulnerable(InvulnerabilityWindow);
                        } else
                        {
                            _shouldBoost = false;
                        }
                    } else
                    {
                        _shouldBoost = false;
                    }
                }

                if(_isBoosting)
                {
                    if (_currentBoostCycle > 0)
                    {
                        boost = BoostForceMultiplier * currentWeightMultiplier;
                        _currentBoostCycle--;
                    } else
                    {
                        _isBoosting = false;
                    }
                }

                playerRigidBody.AddForce(_movementVector * Time.fixedDeltaTime * AccelerationFactor * boost * currentWeightMultiplier);
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
            _isBoosting = false;
        }
        float angleLook = Mathf.Atan2(_desiredLookVector.y, _desiredLookVector.x) * Mathf.Rad2Deg;
        playerRigidBody.MoveRotation(angleLook - 90.0f);
    }

    private void LateUpdate()
    {
        UpdateLineRenderers();

    }

    private void HandleWeightMovementMultiplier(float multiplier) {
        currentWeightMultiplier = multiplier;
    }

    private void mousePositionUpdateHandler(Vector2 mousePos)
    {
        _pointerWorldCoords = _mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _mainCam.nearClipPlane));
        _desiredLookVector = new Vector2(_pointerWorldCoords.x, _pointerWorldCoords.y) - new Vector2(transform.position.x, transform.position.y);
    }

    private void directionActionHandler(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.0f)
        {
            _desiredLookVector = direction;
        }
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


    private void movementActionHandler(Vector2 movementVector)
    {
        _movementVector = movementVector;
        if (movementVector.sqrMagnitude > 0)
        {
            //playerRigidBody.drag = 0;
        } else
        {
            playerRigidBody.drag = DragFactor;
        }
    }

    private void boostActionHandler()
    {
        if(!_shouldBoost && !_isBoosting && _engineOn)
        {
            _shouldBoost = true;
        }
    }

    private void UpdateLineRenderers()
    {
        Vector3 spaceShipPos = transform.position;
        Vector2 forwardDirScaled = (transform.up * 100) + spaceShipPos;
        ForwardLineRenderer.SetPosition(0, new Vector3(spaceShipPos.x, spaceShipPos.y, 0));
        ForwardLineRenderer.SetPosition(1, new Vector3(forwardDirScaled.x, forwardDirScaled.y, 0));
        LookLineRenderer.SetPosition(0, new Vector3(spaceShipPos.x, spaceShipPos.y, 0));
        Vector2 lookTargetPoint = _desiredLookVector + new Vector2(spaceShipPos.x, spaceShipPos.y);
        LookLineRenderer.SetPosition(1, new Vector3(lookTargetPoint.x, lookTargetPoint.y, 0));
    }

    
}
