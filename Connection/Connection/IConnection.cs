using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connection
{
    interface IConnection : IObservable<ConnectedUserData>, IObservable<MessageData>, IObservable<MyIPData>
    {
        Task CreateMyRoomAsync();

        Task<IEnumerable<RoomData>> GetAllRoomsAsync();

        Task SendAsync(MessageData message);
    }
}
