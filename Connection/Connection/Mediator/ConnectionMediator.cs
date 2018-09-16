using Connection.Udp;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Connection.Rooms;

namespace Connection.Mediator
{
    class ConnectionMediator : IConnection
    {
        private readonly IHost host;

        public ConnectionMediator(IHost host)
        {
            this.host = host;
        }

        public async Task SendAsync(MessageData message)
        {
            await host.Services
                      .GetService<IUdpConnection>()
                      .SendAsync(message);
        }

        public async Task StopAsync(CancellationToken token)
        {
            await host.StopAsync(token);
        }

        public void Dispose()
        {
            host.Dispose();
        }

        public async Task CreateMyRoomAsync()
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .CreateMyRoomAsync();
        }

        public async Task DescribeMyRoomAsync(string description)
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .DescribeMyRoomAsync(description);
        }

        public async Task DeleteMyRoomAsync()
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .DeleteMyRoomAsync();
        }

        public async Task AskToJoinToHisRoomAsync(Guid ownerId)
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .AskToJoinToHisRoomAsync(ownerId);
        }

        public async Task TellMyIpAsync(Guid userId, IPAddress address, int port)
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .TellMyIpAsync(userId, address, port);
        }

        public async Task JoinTheUserToMyRoomAsync(Guid userId)
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .JoinTheUserToMyRoomAsync(userId);
        }

        public async Task DenyToUserToMyRoomJoiningAsync(Guid userId, string message)
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .DenyToUserToMyRoomJoiningAsync(userId, message);
        }

        public async Task LeaveThisRoomAsync(Guid roomId, Guid ownerId)
        {
            await host.Services
                      .GetService<IRoomsConnection>()
                      .LeaveThisRoomAsync(roomId, ownerId);
        }

        public async Task<IEnumerable<RoomData>> GetAllRoomsAsync()
        {
            return await host.Services
                             .GetService<IRoomsConnection>()
                             .GetAllRoomsAsync();
        }

        public async Task<RoomDescriptionData> GetRoomDescriptionAsync(Guid roomId)
        {
            return await host.Services
                             .GetService<IRoomsConnection>()
                             .GetRoomDescriptionAsync(roomId);
        }

        public IDisposable Subscribe(IObserver<ConnectedUserData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<DisconnectedUserData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<MyIPData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<RoomExpellingData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<RoomJoiningData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<RoomJoiningRequestData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<RoomLeavingData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<RoomRejectingData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<UserIPData> observer)
        {
            return SubscribeAs(observer);
        }

        public IDisposable Subscribe(IObserver<MessageData> observer)
        {
            return SubscribeAs(observer);
        }

        private IDisposable SubscribeAs<T>(IObserver<T> observer)
        {
            var observerComposite = host.Services.GetService<IObserverComposite<T>>();
            observerComposite.Add(observer);
            return new DisposeObserverCommand<T>(observerComposite, observer);
        }
    }
}
