using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RoomsService.Common.DeleteRoom;
using RoomsService.Common.DescribeRoom;
using RoomsService.Common.SaveNewRoom;
using RoomsService.Logs;
using System;
using System.Threading.Tasks;

namespace RoomsService.Hubs
{
    [Authorize]
    class RoomsHub : Hub
    {
        private readonly ISaveNewRoomStrategy saveNewRoomStrategy;
        private readonly IDescribeRoomStrategy describeRoomStrategy;
        private readonly IDeleteRoomStrategy deleteRoomStrategy;
        private readonly IRoomLogger logger;

        public RoomsHub(ISaveNewRoomStrategy saveNewRoomStrategy,
                        IDescribeRoomStrategy describeRoomStrategy,
                        IDeleteRoomStrategy deleteRoomStrategy,
                        IRoomLogger logger)
        {
            this.saveNewRoomStrategy = saveNewRoomStrategy;
            this.describeRoomStrategy = describeRoomStrategy;
            this.deleteRoomStrategy = deleteRoomStrategy;
            this.logger = logger;
        }

        public async Task CreateMyRoom()
        {
            await Groups.AddToGroupAsync(MyConnectionId, MyRoomId);
            await saveNewRoomStrategy.SaveAsync(MyRoomId, MyId);
            AmIRoomOwner = true;
            LogThatRoomIsCreated();
        }

        private void LogThatRoomIsCreated()
        {
            logger.Information($"Room '{MyRoomId}' is created by owner '{MyId}'");
        }

        public async Task DescribeMyRoom(string description)
        {
            await describeRoomStrategy.DescribeAsync(MyRoomId, description);
        }

        public async Task DeleteMyRoom()
        {
            await Clients.OthersInGroup(MyRoomId).SendAsync("GetOutFromMyRoom", MyRoomId);
            await Groups.RemoveFromGroupAsync(MyConnectionId, MyRoomId);
            await deleteRoomStrategy.DeleteAsync(MyRoomId);
            AmIRoomOwner = false;
            LogThatRoomIsDeleted();
        }

        private void LogThatRoomIsDeleted()
        {
            logger.Information($"Room '{MyRoomId}' is deleted by owner '{MyId}'");
        }

        public async Task IWantToJoinToYourRoom(string ownerId, string userIp)
        {
            await Clients.User(ownerId).SendAsync("PleaseAddMeToYourRoom", MyId, userIp);
        }

        public async Task IJoinYouToMyRoom(string userId, string ownerIp)
        {
            await Clients.User(userId).SendAsync("YouAreJoinedToMyRoom", MyRoomId, ownerIp);
            LogThatUserIsJoinedToRoom(userId);
        }

        private void LogThatUserIsJoinedToRoom(string userId)
        {
            logger.Information($"User '{userId}' is joined to room '{MyRoomId}' with owner '{MyId}'");
        }

        public async Task IDoNotJoinYouToMyRoomBecause(string userId, string message)
        {
            await Clients.User(userId).SendAsync("YouAreNotJoinedToMyRoom", MyRoomId, message);
            LogThatUserIsNotJoinedToRoom(userId, message);
        }

        private void LogThatUserIsNotJoinedToRoom(string userId, string message)
        {
            logger.Information($"User '{userId}' is not joined to room '{MyRoomId}' with owner '{MyId}' because '{message}'");
        }

        public async Task ILeaveThisRoom(string roomId, string ownerId)
        {
            await Clients.User(ownerId).SendAsync("ILeaveYourRoom", MyId);
            await Groups.RemoveFromGroupAsync(MyConnectionId, roomId);
            LogThatUserLeftRoom(roomId, ownerId);
        }

        private void LogThatUserLeftRoom(string roomId, string ownerId)
        {
            logger.Information($"User '{MyId}' left room '{roomId}' with owner '{ownerId}'");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Others.SendAsync("IAmConnected", MyId);
            LogThatUserIsConnected();
        }

        private void LogThatUserIsConnected()
        {
            logger.Information($"User '{MyId}' is connected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("IAmDisconnected", MyId);
            await DisposeMeIfIAmRoomOwnerAsync();
            LogThatUserIsDisconnected(exception);
            await base.OnDisconnectedAsync(exception);
        }

        private void LogThatUserIsDisconnected(Exception e)
        {
            logger.Information(e, $"User '{MyId}' is disconnected");
        }

        private string MyConnectionId => Context.ConnectionId;

        private string MyRoomId => Context.UserIdentifier;

        private string MyId => Context.UserIdentifier;

        private bool AmIRoomOwner
        {
            get => Context.Items.TryGetValue("AmIRoomOwner", out object value) ? (bool)value : false;
            set => Context.Items["AmIRoomOwner"] = value;
        }

        private async Task DisposeMeIfIAmRoomOwnerAsync()
        {
            if(AmIRoomOwner)
            {
                await deleteRoomStrategy.DeleteAsync(MyRoomId);
            }
        }
    }
}
