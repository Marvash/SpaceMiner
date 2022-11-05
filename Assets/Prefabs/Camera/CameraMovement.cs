using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    Transform target;

    private Vector3 zCameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        zCameraOffset = new Vector3(0.0f, 0.0f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, zCameraOffset.z);
        transform.position = targetPos;
    }
}
