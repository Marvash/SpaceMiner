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

    private bool isEnergySystemShutdown = false;

    private float allocatedEnergy = 0.0f;

    private bool consumedEnergyThisTick = false;

    [SerializeField]
    private GameplayCanvasControllerSO gameplayCanvasControllerSO;

    private void TickConversion()
    {
        if ((CurrentEnergy < EnergyCapacity) && CurrentFuel > 0.0f && !consumedEnergyThisTick) {
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
        consumedEnergyThisTick = false;
    }

    public float ConsumeEnergy(float energy)
    {
        float energyConsumed = 0.0f;
        if(!isEnergySystemShutdown)
        {
            energyConsumed = energy;
            if (!InfiniteEnergy)
            {
                CurrentEnergy -= energy;
                consumedEnergyThisTick = true;
                if (CurrentEnergy < 0.0f)
                {
                    energyConsumed = energy + CurrentEnergy;
                    CurrentEnergy = 0.0f;
                    isEnergySystemShutdown = true;
                    gameplayCanvasControllerSO.EnableEnergyShutdown(SystemShutdownTime);
                    Invoke("EnergySystemRestart", SystemShutdownTime);
                }
                allocatedEnergy = Mathf.Min(allocatedEnergy, CurrentEnergy);
            }
            gameplayCanvasControllerSO.UpdateEnergy(CurrentEnergy / EnergyCapacity);
        }
        return energyConsumed;
    }

    public float AllocateEnergy(float energy)
    {
        float availableEnergy = CurrentEnergy - allocatedEnergy;
        if(CurrentEnergy > 0.0f && availableEnergy > 0.0f)
        {
            allocatedEnergy += Mathf.Max(availableEnergy, energy);
        }
        return allocatedEnergy;
    }

    public void ResetAllocatedEnergy()
    {
        allocatedEnergy = 0.0f;
    }

    public float ConsumeAllocatedEnergy()
    {
        float consumableEnergy = Mathf.Min(CurrentEnergy, allocatedEnergy);
        ConsumeEnergy(consumableEnergy);
        allocatedEnergy = 0.0f;
        return consumableEnergy;
    }

    public float GetAllocatedEnergy()
    {
        return allocatedEnergy;
    }

    private void EnergySystemRestart()
    {
        if (CurrentEnergy > 0.0f)
        {
            isEnergySystemShutdown = false;
            gameplayCanvasControllerSO.DisableEnergyShutdown();
        }
    }

    public float GetAvailableEnergy()
    {
        return CurrentEnergy;
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnergy = EnergyCapacity;
        CurrentFuel = FuelCapacity;
        gameplayCanvasControllerSO.UpdateEnergy(CurrentEnergy / EnergyCapacity);
        gameplayCanvasControllerSO.UpdateFuel(CurrentFuel / FuelCapacity);
        InvokeRepeating("TickConversion", 0.0f, EnergyConversionTickRate);
    }

}
