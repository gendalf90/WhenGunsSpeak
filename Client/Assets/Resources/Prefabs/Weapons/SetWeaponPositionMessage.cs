using UnityEngine;
using System.Collections;
using System;

namespace Weapons
{
    public class SetWeaponPositionMessage
    {
        public SetWeaponPositionMessage(Guid parentId, Vector2 position, float rotation, bool isFlip)
        {
            ParentId = parentId;
            Position = position;
            Rotation = rotation;
            IsFlip = isFlip;
        }

        public Guid ParentId { get; private set; }

        public Vector2 Position { get; private set; }

        public float Rotation { get; private set; }

        public bool IsFlip { get; private set; }
    }
}