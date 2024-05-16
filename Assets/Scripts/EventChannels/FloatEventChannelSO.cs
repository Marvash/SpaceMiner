using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "FloatEventChannelSO", menuName = "ScriptableObjects/FloatEventChannelSO", order = 1)]
public class FloatEventChannelSO : ScriptableObject
{
    [HideInInspector]
    public UnityEvent<float> OnFloatChanged = new UnityEvent<float>();

    public void RaiseEvent(float value) {
        OnFloatChanged.Invoke(value);
    }
}
