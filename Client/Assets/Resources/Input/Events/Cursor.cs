using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class CursorEvent
    {
        public CursorEvent(Vector2 worldPosition, Vector2 screenPosition)
        {
            WorldPosition = worldPosition;
            ScreenPosition = screenPosition;
        }

        public Vector2 WorldPosition { get; private set; }

        public Vector2 ScreenPosition { get; private set; }
    }
}