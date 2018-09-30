using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RoomsService.Common.DeleteRoom;
using RoomsService.Common.DescribeRoom;
using RoomsService.Common.CreateRoom;
using RoomsService.Logs;
using System;
using System.Threading.Tasks;

namespace RoomsService.Hubs
{
    [Authorize]
    public class RoomsHub : Hub
    {
        private readonly ICreateRoomStrategy createRoomStrategy;
        private readonly IDescribeRoomStrategy describeRoomStrategy;
        private readonly IDeleteRoomStrategy deleteRoomStrategy;
        private readonly IRoomLogger logger;

        public RoomsHub(ICreateRoomStrategy createRoomStrategy,
                        IDescribeRoomStrategy describeRoomStrategy,
                        IDeleteRoomStrategy deleteRoomStrategy,
                        IRoomLogger logger)
        {
            this.createRoomStrategy = createRoomStrategy;
            this.describeRoomStrategy = describeRoomStrategy;
            this.deleteRoomStrategy = deleteRoomStrategy;
            this.logger = logger;
        }

        public async Task CreateMyRoom(string header)
        {
            await createRoomStrategy.CreateAsync(MyId, header);
            LogThatRoomIsCreated(header);
        }

        private void LogThatRoomIsCreated(string header)
        {
            logger.Information($"Room with header '{header}' is created by owner '{MyId}'");
        }

        public async Task DescribeMyRoom(string description)
        {
            await describeRoomStrategy.DescribeAsync(MyId, description);
        }

        public async Task DeleteMyRoom()
        {
            await Clients.Others.SendAsync("GetOutFromMyRoom", MyId);
            await deleteRoomStrategy.DeleteAsync(MyId);
            LogThatRoomIsDeleted();
        }

        private void LogThatRoomIsDeleted()
        {
            logger.Information($"Room with owner '{MyId}' is deleted");
        }

        public async Task IWantToJoinToYourRoom(string ownerId)
        {
            await Clients.User(ownerId).SendAsync("PleaseAddMeToYourRoom", MyId);
        }

        public async Task ITellYouMyIP(string userId, string address, int port)
        {
            await Clients.User(userId).SendAsync("HeSaysThatHisIpIs", MyId, address, port);
        }

        public async Task IWantToStartMessagingWithYou(string userId)
        {
            await Clients.User(userId).SendAsync("HeSuggestsToStartMessaging", MyId);
        }

        public async Task IAmReadyToStartMessagingWithYou(string userId, string securityKey)
        {
            await Clients.User(userId).SendAsync("HeIsReadyToStartMessaging", MyId, securityKey);
            LogThatMessagingWithUserIsStarted(userId);
        }

        private void LogThatMessagingWithUserIsStarted(string userId)
        {
            logger.Information($"User '{MyId}' have accepted messaging request from user '{userId}'");
        }

        public async Task IJoinYouToMyRoom(string userId)
        {
            await Clients.User(userId).SendAsync("YouAreJoinedToMyRoom", MyId);
            LogThatUserIsJoinedToRoom(userId);
        }

        private void LogThatUserIsJoinedToRoom(string userId)
        {
            logger.Information($"User '{userId}' is joined to room with owner '{MyId}'");
        }

        public async Task IDoNotJoinYouToMyRoomBecause(string userId, string message)
        {
            await Clients.User(userId).SendAsync("YouAreNotJoinedToMyRoom", MyId, message);
            LogThatUserIsNotJoinedToRoom(userId, message);
        }

        private void LogThatUserIsNotJoinedToRoom(string userId, string message)
        {
            logger.Information($"User '{userId}' is not joined to room with owner '{MyId}' because '{message}'");
        }

        public async Task ILeaveYourRoom(string userId)
        {
            await Clients.User(userId).SendAsync("HeLeavesMyRoom", MyId);
            LogThatUserLeftRoom(userId);
        }

        private void LogThatUserLeftRoom(string ownerId)
        {
            logger.Information($"User '{MyId}' left room with owner '{ownerId}'");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Others.SendAsync("HeIsConnected", MyId);
            LogThatUserIsConnected();
        }

        private void LogThatUserIsConnected()
        {
            logger.Information($"User '{MyId}' is connected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("HeIsDisconnected", MyId);
            await deleteRoomStrategy.DeleteAsync(MyId);
            LogThatUserIsDisconnected(exception);
            await base.OnDisconnectedAsync(exception);
        }

        private void LogThatUserIsDisconnected(Exception e)
        {
            logger.Information(e, $"User '{MyId}' is disconnected");
        }

        public async Task IWantToKnowAboutMe()
        {
            await Clients.Caller.SendAsync("ThisIsYou", MyId);
        }

        private string MyId => Context.UserIdentifier;
    }
}
