using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    public class Throw7dot62Command
    {
        public Throw7dot62Command(Guid guid, Vector2 position, float rotation)
        {
            Guid = guid;
            Position = position;
            Rotation = rotation;
        }

        public Guid Guid { get; private set; }

        public Vector2 Position { get; private set; }

        public float Rotation { get; private set; }
    }
}