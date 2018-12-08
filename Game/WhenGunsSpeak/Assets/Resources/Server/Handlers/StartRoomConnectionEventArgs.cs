using Connection;
using System;

namespace Server
{
    class StartRoomConnectionEventArgs : EventArgs
    {
        public StartRoomConnectionEventArgs(IRoomConnection roomConnection, Guid myId)
        {
            MyId = myId;
            RoomConnection = roomConnection;
        }

        public Guid MyId { get; private set; }

        public IRoomConnection RoomConnection { get; private set; }
    }
}
