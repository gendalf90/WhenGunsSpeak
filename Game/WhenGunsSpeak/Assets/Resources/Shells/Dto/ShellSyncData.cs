using MessagePack;
using System;
using UnityEngine;

namespace Shells
{
    [MessagePackObject]
    public class ShellSyncData
    {
        [Key(0)]
        public Guid Guid { get; set; }

        [Key(1)]
        public Vector2 Position { get; set; }

        [Key(2)]
        public float Rotation { get; set; }
    }
}