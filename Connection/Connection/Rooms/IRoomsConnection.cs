using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Connection.Rooms
{
    public interface IRoomsConnection : IHostedService, IDisposable
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
    }
}
