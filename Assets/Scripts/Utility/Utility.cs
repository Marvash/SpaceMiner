using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static float RemapRangeTo01(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}
