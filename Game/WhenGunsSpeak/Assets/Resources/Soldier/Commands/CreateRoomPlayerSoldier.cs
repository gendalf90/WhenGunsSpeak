using System;

namespace Soldier
{
    public class CreateRoomPlayerSoldierCommand
    {
        public CreateRoomPlayerSoldierCommand(Guid playerGuid)
        {
            PlayerGuid = playerGuid;
        }

        public Guid PlayerGuid { get; private set; }
    }
}
