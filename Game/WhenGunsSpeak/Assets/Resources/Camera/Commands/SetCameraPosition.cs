using UnityEngine;

namespace Camera
{
    public class SetCameraPositionCommand
    {
        public SetCameraPositionCommand(Vector2 worldPosition)
        {
            WorldPositon = worldPosition;
        }

        public Vector2 WorldPositon { get; private set; }
    }
}