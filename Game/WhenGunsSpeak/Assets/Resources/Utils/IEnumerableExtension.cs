using System.Collections.Generic;
using System;

public static class IEnumerableExtension
{
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach(var item in collection)
        {
            action(item);
        }
    }
}
