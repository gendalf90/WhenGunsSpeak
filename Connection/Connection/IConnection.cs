using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Connection
{
    public interface IConnection : IObservable<ConnectedUserData>, 
                                   IObservable<DisconnectedUserData>,
                                   IObservable<MyIPData>,
                                   IObservable<RoomExpellingData>,
                                   IObservable<RoomJoiningData>,
                                   IObservable<RoomJoiningRequestData>,
                                   IObservable<RoomLeavingData>,
                                   IObservable<RoomRejectingData>,
                                   IObservable<UserIPData>,
                                   IObservable<MessageData>, 
                                   IDisposable
    {
        Task CreateMyRoomAsync();

        Task DescribeMyRoomAsync(string description);

        Task DeleteMyRoomAsync();

        Task AskToJoinToHisRoomAsync(Guid ownerId);

        Task TellMyIpAsync(Guid userId, IPAddress address, int port);

        Task JoinTheUserToMyRoomAsync(Guid userId);

        Task DenyToUserToMyRoomJoiningAsync(Guid userId, string message);

        Task LeaveThisRoomAsync(Guid roomId, Guid ownerId);

        Task<IEnumerable<RoomData>> GetAllRoomsAsync();

        Task<RoomDescriptionData> GetRoomDescriptionAsync(Guid roomId);

        Task SendAsync(MessageData data);

        Task StopAsync(CancellationToken token);
    }
}
