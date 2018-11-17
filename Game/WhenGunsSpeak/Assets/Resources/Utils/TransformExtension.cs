using UnityEngine;
using System;

public static class TransformExtension
{
    public static void SetFlipX(this Transform transform, bool value)
    {
        Vector3 theScale = transform.localScale;

        if (value)
        {
            if (!transform.IsFlipX())
            {
                theScale.x *= -1;
            }
        }
        else
        {
            theScale.x = Mathf.Abs(theScale.x);
        }

        transform.localScale = theScale;
    }

    public static void SetFlipY(this Transform transform, bool value)
    {
        Vector3 theScale = transform.localScale;

        if (value)
        {
            if (!transform.IsFlipY())
            {
                theScale.y *= -1;
            }
        }
        else
        {
            theScale.y = Mathf.Abs(theScale.y);
        }

        transform.localScale = theScale;
    }

    public static bool IsFlipX(this Transform transform)
    {
        return Math.Sign(transform.localScale.x) == -1;
    }

    public static bool IsFlipY(this Transform transform)
    {
        return Math.Sign(transform.localScale.y) == -1;
    }
}
