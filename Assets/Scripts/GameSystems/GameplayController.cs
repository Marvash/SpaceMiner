using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private GameManagerSO gameManager;

    [SerializeField]
    private GameEventsManager GameEventsManager;

    public float SafeZoneEnterDistance = 12;
    public float SafeZoneExitDistance = 16;

    private bool _isInSafeZone = true;

    void Start() {
        GameEventsManager.DeactivateGameEvents();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.Player.transform.position.magnitude > SafeZoneExitDistance && _isInSafeZone) {
            _isInSafeZone = false;
            GameEventsManager.ActivateGameEvents();
        } else if(gameManager.Player.transform.position.magnitude < SafeZoneEnterDistance && !_isInSafeZone) {
            GameEventsManager.DeactivateGameEvents();
        }
    }
}
