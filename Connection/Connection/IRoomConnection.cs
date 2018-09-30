using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Connection
{
    public interface IRoomConnection : IObservable<MyData>,
                                       IObservable<ConnectedUserData>,
                                       IObservable<DisconnectedUserData>,
                                       IObservable<RoomExpellingData>,
                                       IObservable<RoomJoinedData>,
                                       IObservable<RoomJoiningData>,
                                       IObservable<RoomLeavingData>,
                                       IObservable<RoomRejectedData>,
                                       IObservable<MessagingStartingData>,
                                       IObservable<MessagingStartedData>,
                                       IObservable<UserIPData>,
                                       IDisposable
    {
        Task CreateMyRoomAsync(string header);

        Task DescribeMyRoomAsync(string description);

        Task DeleteMyRoomAsync();

        Task AskToJoinToHisRoomAsync(Guid ownerId);

        Task TellMyIpAsync(Guid userId, IPAddress address, int port);

        Task AskToStartMessagingAsync(Guid userId);

        Task StartMessagingAsync(Guid userId, byte[] securityKey);

        Task JoinTheUserToMyRoomAsync(Guid userId);

        Task DenyToUserToMyRoomJoiningAsync(Guid userId, string message);

        Task LeaveThisOwnerRoomAsync(Guid ownerId);

        Task KnowAboutMeAsync();

        Task<IEnumerable<RoomData>> GetAllRoomsAsync();

        Task<RoomDescriptionData> GetRoomDescriptionAsync(Guid roomId);
    }
}
