using UnityEngine;

namespace Messages
{
    public class SoldierPositionEvent
    {
        public string SoldierId { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 LookingPoint { get; set; }

        public bool IsRightLooking { get; set; }

        public bool IsLeftLooking { get; set; }
    }
}
