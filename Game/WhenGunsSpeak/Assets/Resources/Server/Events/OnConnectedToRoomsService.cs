using System;

namespace Server
{
    public class OnConnectedToRoomsServiceEvent
    {
        public OnConnectedToRoomsServiceEvent(Guid myId)
        {
            MyId = myId;
        }

        public Guid MyId { get; private set; }
    }
}
