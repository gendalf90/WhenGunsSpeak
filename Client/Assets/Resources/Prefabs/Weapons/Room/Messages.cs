using UnityEngine;
using System.Collections;

namespace Rooms
{
    public class SetCameraPositionMessage
    {
        public SetCameraPositionMessage(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; private set; }
    }
}