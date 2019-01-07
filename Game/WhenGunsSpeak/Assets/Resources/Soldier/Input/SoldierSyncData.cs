using MessagePack;
using System;
using UnityEngine;

namespace Soldier
{
    [MessagePackObject]
    public class SoldierSyncData
    {
        [Key(0)]
        public Vector2 LookingPosition { get; set; }

        [Key(1)]
        public Vector2 Position { get; set; }

        [Key(2)]
        public Guid PlayerGuid { get; set; }

        [Key(3)]
        public LegsAnimationType LegsAnimationType { get; set; }
    }
}
