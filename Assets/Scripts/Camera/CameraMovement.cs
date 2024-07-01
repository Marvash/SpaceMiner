using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameManagerSO gameManager;

    private Transform _target;

    private Vector3 _zCameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        _zCameraOffset = new Vector3(0.0f, 0.0f, transform.position.z);
        _target = gameManager.Player.transform;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(_target.position.x, _target.position.y, _zCameraOffset.z);
        transform.position = targetPos;
    }
}
