using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField]
    float maxTargetRange;

    [SerializeField]
    float scale;

    [SerializeField]
    Transform target;

    private float maxBackgroundRange;

    // Start is called before the first frame update
    void Start()
    {
        maxBackgroundRange = maxTargetRange * scale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetMaxRangePercentage = new Vector2(target.position.x / maxTargetRange, target.position.y / maxTargetRange);
        Vector3 updatedBackgroundPos = new Vector3(maxBackgroundRange * targetMaxRangePercentage.x, maxBackgroundRange * targetMaxRangePercentage.y, transform.position.z);
        transform.position = updatedBackgroundPos;
    }
}
