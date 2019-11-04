using UnityEngine;

namespace Messages
{
    public class CreateOfflineShellCommand
    {
        public string SoldierId { get; set; }

        public string ShellId { get; set; }

        public string ShellName { get; set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }
    }
}
