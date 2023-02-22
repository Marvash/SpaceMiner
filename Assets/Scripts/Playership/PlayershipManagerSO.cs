using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


[CreateAssetMenu(fileName = "PlayershipManagerSO", menuName = "ScriptableObjects/PlayershipManagerSO", order = 1)]
public class PlayershipManagerSO : ScriptableObject
{
    public GameObject Player;

    public void RegisterPlayer(GameObject player)
    {
        Debug.Log("Registering Player");
        Player = player;
    }
}
