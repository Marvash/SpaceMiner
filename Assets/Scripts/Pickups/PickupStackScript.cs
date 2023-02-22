using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStackScript : MonoBehaviour
{
    public PickupStack pickup;

    private bool lerpingToPos;
    private Vector2 targetPos;
    private const float smoothDampTime = 0.3f;
    private Vector2 lerpVelocity = Vector2.zero;
    private Rigidbody2D pickupRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        pickupRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (lerpingToPos)
        {
            pickupRigidbody.MovePosition(Vector2.SmoothDamp(new Vector2(transform.position.x, transform.position.y), targetPos, ref lerpVelocity, smoothDampTime));
            Vector2 targetDiff = new Vector2(transform.position.x, transform.position.y) - targetPos;
            if(targetDiff.sqrMagnitude < 0.0001f)
            {
                lerpingToPos = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setLerpToTargetPosition(Vector2 pos)
    {
        targetPos = pos;
        lerpingToPos = true;
    }
}
