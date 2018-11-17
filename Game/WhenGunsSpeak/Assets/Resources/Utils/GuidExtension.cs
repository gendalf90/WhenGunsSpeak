using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuidExtension
{
    public static string ToMinString(this Guid guid)
    {
        return guid.ToString("N");
    }
}
