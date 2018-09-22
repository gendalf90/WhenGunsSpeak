using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Connection
{
    public interface IRoomConnection : IObservable<MyIdData>,
                                       IObservable<ConnectedUserData>,
                                       IObservable<DisconnectedUserData>,
                                       IObservable<RoomExpellingData>,
                                       IObservable<RoomJoinedData>,
                                       IObservable<RoomJoiningData>,
                                       IObservable<RoomLeavingData>,
                                       IObservable<RoomRejectedData>,
                                       IObservable<UserIPData>,
                                       IDisposable
    {
        Task CreateMyRoomAsync();

        Task DescribeMyRoomAsync(string description);

        Task DeleteMyRoomAsync();

        Task AskToJoinToHisRoomAsync(Guid ownerId);

        Task TellMyIpAsync(Guid userId, IPAddress address, int port);

        Task JoinTheUserToMyRoomAsync(Guid userId, byte[] securityKey);

        Task DenyToUserToMyRoomJoiningAsync(Guid userId, string message);

        Task LeaveThisRoomAsync(Guid roomId, Guid ownerId);

        Task KnowMyIdAsync();

        Task<IEnumerable<RoomData>> GetAllRoomsAsync();

        Task<RoomDescriptionData> GetRoomDescriptionAsync(Guid roomId);
    }
}
