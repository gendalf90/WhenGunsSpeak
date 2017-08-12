using UnityEngine;
using System.Collections;

public static class Vector2Extensions
{
    public static float GetAngle(this Vector2 from, Vector2 to)
    {
        return Mathf.Atan2(from.y - to.y, from.x - to.x) * Mathf.Rad2Deg + 180;
    }
}
