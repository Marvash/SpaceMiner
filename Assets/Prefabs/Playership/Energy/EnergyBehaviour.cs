using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float FuelCapacity = 10;

    [SerializeField]
    private float EnergyConversionTickRate = 1.0f;

    [SerializeField]
    private float EnergyConvertedPerTick = 0.5f;

    [SerializeField]
    private float EnergyConversionFuelPerTick = 0.1f;

    [SerializeField]
    private float EnergyCapacity = 100;

    [SerializeField]
    private float CurrentEnergy;

    [SerializeField]
    private float CurrentFuel;

    [SerializeField]
    private float SystemShutdownTime = 2.0f;

    [SerializeField]
    private bool InfiniteFuel = false;

    [SerializeField]
    private bool InfiniteEnergy = false;

    private UnityEvent FuelDepleted = new UnityEvent();

    private bool _isEnergySystemShutdown = false;

    [SerializeField]
    private GameplayCanvasControllerSO gameplayCanvasControllerSO;

    private void tickConversion()
    {
        if ((CurrentEnergy < EnergyCapacity) && CurrentFuel > 0.0f) {
            CurrentEnergy += EnergyConvertedPerTick;
            if (!InfiniteFuel)
            {
                CurrentFuel -= EnergyConversionFuelPerTick;
            }
            if (CurrentEnergy > EnergyCapacity)
            {
                CurrentEnergy = EnergyCapacity;
            }
            if(CurrentFuel <= 0.0f)
            {
                CurrentFuel = 0.0f;
                FuelDepleted.Invoke();
            }
            gameplayCanvasControllerSO.UpdateEnergy(CurrentEnergy / EnergyCapacity);
            gameplayCanvasControllerSO.UpdateFuel(CurrentFuel / FuelCapacity);
        }
    }

    public float ConsumeEnergy(float energy)
    {
        float energyConsumed = 0.0f;
        if(!_isEnergySystemShutdown)
        {
            energyConsumed = energy;
            if (!InfiniteEnergy)
            {
                CurrentEnergy -= energy;
                if (CurrentEnergy < 0.0f)
                {
                    energyConsumed = energy + CurrentEnergy;
                    CurrentEnergy = 0.0f;
                    _isEnergySystemShutdown = true;
                    gameplayCanvasControllerSO.EnableEnergyShutdown(SystemShutdownTime);
                    Invoke("energySystemRestart", SystemShutdownTime);
                }
            }
            gameplayCanvasControllerSO.UpdateEnergy(CurrentEnergy / EnergyCapacity);
        }
        return energyConsumed;
    }

    private void energySystemRestart()
    {
        if (CurrentEnergy > 0.0f)
        {
            _isEnergySystemShutdown = false;
            gameplayCanvasControllerSO.DisableEnergyShutdown();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnergy = EnergyCapacity;
        CurrentFuel = FuelCapacity;
        gameplayCanvasControllerSO.UpdateEnergy(CurrentEnergy / EnergyCapacity);
        gameplayCanvasControllerSO.UpdateFuel(CurrentFuel / FuelCapacity);
        InvokeRepeating("tickConversion", 0.0f, EnergyConversionTickRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
