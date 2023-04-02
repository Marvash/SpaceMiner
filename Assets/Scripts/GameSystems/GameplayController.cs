using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

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
        if(PlayershipManagerSO.Player.transform.position.magnitude > SafeZoneExitDistance && _isInSafeZone) {
            _isInSafeZone = false;
            GameEventsManager.ActivateGameEvents();
        } else if(PlayershipManagerSO.Player.transform.position.magnitude < SafeZoneEnterDistance && !_isInSafeZone) {
            GameEventsManager.DeactivateGameEvents();
        }
    }
}
