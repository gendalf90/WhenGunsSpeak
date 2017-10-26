using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shells.Factory
{
    class ShellData
    {
        public ShellData(string type, Guid guid, Vector2 position, float rotation)
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

    class ShellsEvent
    {
        public ShellsEvent(IEnumerable<ShellData> shells)
        {
            Shells = shells;
        }

        public IEnumerable<ShellData> Shells { get; private set; }
    }
}
