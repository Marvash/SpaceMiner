using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameplayCanvasControllerSO", menuName = "ScriptableObjects/GameplayCanvasControllerSO", order = 1)]
public class GameplayCanvasControllerSO : ScriptableObject
{
    public UnityEvent<float> EnergyUpdateEvent = new UnityEvent<float>();
    public UnityEvent<float> FuelUpdateEvent = new UnityEvent<float>();
    public UnityEvent<float> HealthUpdateEvent = new UnityEvent<float>();
    public UnityEvent<float> EnergyShutdownEnableEvent = new UnityEvent<float>();
    public UnityEvent EnergyShutdownDisableEvent = new UnityEvent();

    public void UpdateEnergy(float energyPercentage)
    {
        EnergyUpdateEvent.Invoke(energyPercentage);
    }

    public void UpdateFuel(float fuelPercentage)
    {
        FuelUpdateEvent.Invoke(fuelPercentage);
    }

    public void EnableEnergyShutdown(float duration)
    {
        EnergyShutdownEnableEvent.Invoke(duration);
    }

    public void DisableEnergyShutdown()
    {
        EnergyShutdownDisableEvent.Invoke();
    }

    public void UpdateHealth(float healthPercentage)
    {
        HealthUpdateEvent.Invoke(healthPercentage);
    }
}
