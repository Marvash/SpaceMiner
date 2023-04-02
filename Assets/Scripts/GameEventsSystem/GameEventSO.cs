using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameEvent {
    NONE,
    ENEMY_SPAWN
}

[CreateAssetMenu(fileName = "GameEventSO", menuName = "ScriptableObjects/GameEventSO", order = 3)]
public class GameEventSO : ScriptableObject
{
    public GameEvent GameEvent;
    public float StartMinWaitTime;
    public float StartMaxWaitTime; 
    public float EndMinWaitTime; 
    public float EndMaxWaitTime; 
}
