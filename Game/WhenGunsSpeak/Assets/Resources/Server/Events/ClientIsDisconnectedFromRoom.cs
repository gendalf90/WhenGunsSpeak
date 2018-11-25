using System;

namespace Server
{
    public class ClientIsDisconnectedFromRoomEvent
    {
        public ClientIsDisconnectedFromRoomEvent(Guid clientId)
        {
            ClientId = clientId;
        }

        public Guid ClientId { get; private set; }
    }
}
