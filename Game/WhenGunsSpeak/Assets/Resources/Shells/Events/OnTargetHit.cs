using System;

namespace Shells
{
    public class OnTargetHitEvent
    {
        public OnTargetHitEvent(Guid shellId, int targetInstanceId)
        {
            ShellId = shellId;
            TargetInstanceId = targetInstanceId;
        }

        public Guid ShellId { get; private set; }

        public int TargetInstanceId { get; private set; }
    }
}
