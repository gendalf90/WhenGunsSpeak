using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connection
{
    interface IConnection : IObservable<ConnectedUserData>
    {
        Task CreateMyRoomAsync();

        Task<IEnumerable<RoomData>> GetAllRoomsAsync();
    }
}
