using System;
using UnityEngine;

namespace Shells
{
    public class Throw7dot62Command
    {
        public Throw7dot62Command(Guid id, Vector2 position, float rotation)
        {
            Id = id;
            Position = position;
            Rotation = rotation;
        }

        public Guid Id { get; private set; }

        public Vector2 Position { get; private set; }

        public float Rotation { get; private set; }
    }
}