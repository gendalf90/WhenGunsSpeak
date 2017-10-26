using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shells
{
    class CreateShellCommand
    {
        public CreateShellCommand(string type, Guid guid, Vector2 position, float rotation)
        {
            Type = type;
            Guid = guid;
            Position = position;
            Rotation = rotation;
        }

        public string Type { get; private set; }

        public Guid Guid { get; private set; }

        public Vector2 Position { get; private set; }

        public float Rotation { get; private set; }
    }
}
