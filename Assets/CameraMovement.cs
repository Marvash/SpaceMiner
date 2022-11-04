using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    Transform targetTransform;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float smoothDampTime;

    private Vector3 cameraVelocity = Vector3.zero;
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
        Vector3 targetPos = targetTransform.position + zCameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref cameraVelocity, smoothDampTime);
    }
}
