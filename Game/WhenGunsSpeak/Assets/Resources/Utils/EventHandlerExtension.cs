using UnityEngine;
using System.Collections;
using System;

public static class EventHandlerExtension
{
    public static void SafeRaise(this EventHandler eventHandler, object sender, EventArgs args)
    {
        var handler = eventHandler;

        if (handler != null)
        {
            handler(sender, args);
        }
    }

    public static void SafeRaise<T>(this EventHandler<T> eventHandler, object sender, T args) where T : EventArgs
    {
        var handler = eventHandler;

        if (handler != null)
        {
            handler(sender, args);
        }
    }
}
