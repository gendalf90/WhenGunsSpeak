using UnityEngine;

namespace Messages
{
    public class CreateAndThrowShellCommand
    {
        public string SoldierId { get; set; }

        public string ShellId { get; set; }

        public string ShellKey { get; set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }
    }
}
