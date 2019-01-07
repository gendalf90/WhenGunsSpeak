using System;

namespace Soldier
{
    public class RemoveSoldierCommand
    {
        public RemoveSoldierCommand(Guid playerGuid)
        {
            PlayerGuid = playerGuid;
        }

        public Guid PlayerGuid { get; private set; }
    }
}