using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EnergyUIController : MonoBehaviour
{
    [SerializeField]
    private Image CurrentEnergyImage;

    [SerializeField]
    private Image CurrentFuelImage;

    [SerializeField]
    private Text EnergyShutdownText;

    [SerializeField]
    private Text FuelLevelText;

    [SerializeField]
    private GameplayCanvasControllerSO gameplayCanvasControllerSO;

    private float _targetEnergyPercentage;

    private float _targetFuelPercentage;

    [SerializeField]
    private float EnergyBarSmoothness = 0.3f;

    [SerializeField]
    private float FuelBarSmoothness = 0.3f;

    [SerializeField]
    private float EnergyShutdownFlashingInterval = 0.3f;

    [SerializeField]
    private float FuelLowThreshold = 0.25f;

    [SerializeField]
    private float FuelCriticalThreshold = 0.15f;

    private Tween _energyTextColorTween;
    private Tween _fuelTextLowColorTween;
    private Tween _fuelTextCriticalColorTween;

    private Color _energyBarInitialColor;

    public void SetEnergyPercentage(float percentage)
    {
        _targetEnergyPercentage = percentage;
        CurrentEnergyImage.DOFillAmount(_targetEnergyPercentage, EnergyBarSmoothness);
    }

    public void SetFuelPercentage(float percentage)
    {
        _targetFuelPercentage = percentage;
        CurrentFuelImage.DOFillAmount(_targetFuelPercentage, FuelBarSmoothness);
        if (_targetFuelPercentage <= 0.0f)
        {
            FuelLevelText.enabled = true;
            FuelLevelText.text = "NO FUEL";
            if (_fuelTextCriticalColorTween.IsPlaying())
                _fuelTextCriticalColorTween.Pause();
            if (_fuelTextLowColorTween.IsPlaying())
                _fuelTextLowColorTween.Pause();
            FuelLevelText.color = Color.red;
        }
        else if(_targetFuelPercentage < FuelCriticalThreshold)
        {
            FuelLevelText.enabled = true;
            FuelLevelText.text = "FUEL CRITICAL";
            if(_fuelTextLowColorTween.IsPlaying())
                _fuelTextLowColorTween.Pause();
            if (!_fuelTextCriticalColorTween.IsPlaying())
                _fuelTextCriticalColorTween.Restart();
        } else if(_targetFuelPercentage < FuelLowThreshold)
        {
            FuelLevelText.enabled = true;
            FuelLevelText.text = "FUEL LOW";
            if (_fuelTextCriticalColorTween.IsPlaying())
                _fuelTextCriticalColorTween.Pause();
            if (!_fuelTextLowColorTween.IsPlaying())
                _fuelTextLowColorTween.Restart();
        } else
        {
            FuelLevelText.color = Color.white;
            FuelLevelText.enabled = false;
            if (_fuelTextCriticalColorTween.IsPlaying())
                _fuelTextCriticalColorTween.Pause();
            if (_fuelTextLowColorTween.IsPlaying())
                _fuelTextLowColorTween.Pause();
        }
    }

    public void EnableEnergyShutdown(float duration)
    {
        EnergyShutdownText.enabled = true;
        CurrentEnergyImage.color = Color.red;
        CurrentEnergyImage.DOColor(_energyBarInitialColor, duration).SetEase(Ease.InCubic);
        _energyTextColorTween.Restart();
    }

    public void DisableEnergyShutdown()
    {
        EnergyShutdownText.enabled = false;
        _energyTextColorTween.Pause();
    }

    private void Awake()
    {
        gameplayCanvasControllerSO.EnergyUpdateEvent.AddListener(SetEnergyPercentage);
        gameplayCanvasControllerSO.FuelUpdateEvent.AddListener(SetFuelPercentage);
        gameplayCanvasControllerSO.EnergyShutdownEnableEvent.AddListener(EnableEnergyShutdown);
        gameplayCanvasControllerSO.EnergyShutdownDisableEvent.AddListener(DisableEnergyShutdown);
        _energyBarInitialColor = CurrentEnergyImage.color;
        _energyTextColorTween = EnergyShutdownText.DOColor(Color.red, EnergyShutdownFlashingInterval).SetEase(Ease.OutFlash).SetLoops(-1, LoopType.Restart);
        _fuelTextCriticalColorTween = FuelLevelText.DOColor(Color.red, 0.6f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Restart);
        _fuelTextLowColorTween = FuelLevelText.DOColor(Color.red, 0.9f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Restart);
        _fuelTextCriticalColorTween.Pause();
        _fuelTextLowColorTween.Pause();
        _energyTextColorTween.Pause();
    }
}
