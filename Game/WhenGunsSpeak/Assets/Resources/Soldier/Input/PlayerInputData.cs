using MessagePack;
using System;
using UnityEngine;

namespace Soldier
{
    [MessagePackObject]
    public class PlayerInputData
    {
        [Key(0)]
        public Vector2 LookingPosition { get; set; }

        [Key(1)]
        public bool IsRightMoving { get; set; }

        [Key(2)]
        public bool IsLeftMoving { get; set; }

        [Key(3)]
        public bool IsJumping { get; set; }

        [Key(4)]
        public Guid PlayerGuid { get; set; }
    }
}
