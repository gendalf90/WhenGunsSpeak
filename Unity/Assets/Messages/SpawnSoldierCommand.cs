using UnityEngine;

namespace Messages
{
    public class SpawnSoldierCommand
    {
        public string SoldierId { get; set; }

        public Vector2 Position { get; set; }
    }
}
