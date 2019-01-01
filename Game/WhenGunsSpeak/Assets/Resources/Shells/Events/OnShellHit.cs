using System;

namespace Shells
{
    //public enum Type
    //{
    //    Standard,
    //    Expansive,
    //    AntiArmor
    //}

    public class OnShellHitEvent
    {
        public OnShellHitEvent(Guid shellId, int targetInstanceId)
        {
            ShellId = shellId;
            TargetInstanceId = targetInstanceId;
        }

        public Guid ShellId { get; private set; }

        public int TargetInstanceId { get; private set; }

        //public float Energy { get; private set; }
    }
}