using Connection.Common;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Connection.Rooms
{
    class RoomConnection : IRoomConnection
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IObserverComposite<MyIdData> myIdObserver;
        private readonly IObserverComposite<ConnectedUserData> userConnectedObserver;
        private readonly IObserverComposite<DisconnectedUserData> userDisconnectedObserver;
        private readonly IObserverComposite<RoomExpellingData> roomExpellingObserver;
        private readonly IObserverComposite<RoomJoiningData> roomJoiningObserver;
        private readonly IObserverComposite<UserIPData> userIPObserver;
        private readonly IObserverComposite<RoomJoinedData> roomJoinedObserver;
        private readonly IObserverComposite<RoomRejectedData> roomRejectedObserver;
        private readonly IObserverComposite<RoomLeavingData> roomLeavingObserver;
        private readonly IOptions<RoomOptions> roomOptions;

        private HubConnection signalRConnection;

        public RoomConnection(IHttpClientFactory httpClientFactory,
                              IObserverComposite<ConnectedUserData> userConnectedObserver,
                              IObserverComposite<DisconnectedUserData> userDisconnectedObserver,
                              IObserverComposite<RoomExpellingData> roomExpellingObserver,
                              IObserverComposite<RoomJoiningData> roomJoiningObserver,
                              IObserverComposite<UserIPData> userIPObserver,
                              IObserverComposite<RoomJoinedData> roomJoinedObserver,
                              IObserverComposite<RoomRejectedData> roomRejectedObserver,
                              IObserverComposite<RoomLeavingData> roomLeavingObserver,
                              IObserverComposite<MyIdData> myIdObserver,
                              IOptions<RoomOptions> roomOptions)
        {
            this.httpClientFactory = httpClientFactory;
            this.userConnectedObserver = userConnectedObserver;
            this.userDisconnectedObserver = userDisconnectedObserver;
            this.roomExpellingObserver = roomExpellingObserver;
            this.roomJoiningObserver = roomJoiningObserver;
            this.userIPObserver = userIPObserver;
            this.roomJoinedObserver = roomJoinedObserver;
            this.roomRejectedObserver = roomRejectedObserver;
            this.roomLeavingObserver = roomLeavingObserver;
            this.myIdObserver = myIdObserver;
            this.roomOptions = roomOptions;
        }

        private void InitializeSignalRHub()
        {
            signalRConnection = new HubConnectionBuilder().WithUrl(new Uri(roomOptions.Value.Address, "signalr/rooms"), options =>
                                                          {
                                                              options.AccessTokenProvider = () => Task.FromResult(roomOptions.Value.Token);
                                                          })
                                                          .AddMessagePackProtocol()
                                                          .Build();
        }

        public async Task StartAsync()
        {
            InitializeSignalRHub();
            InitializeSignalRCallbacks();
            await StartHubAsync();
        }

        private async Task StartHubAsync()
        {
            await signalRConnection.StartAsync();
        }

        private void InitializeSignalRCallbacks()
        {
            signalRConnection.On<Guid>("ThatIsYourId", userId =>
            {
                myIdObserver.OnNext(new MyIdData
                {
                    Id = userId
                });
            });

            signalRConnection.On<Guid>("HeIsConnected", userId =>
            {
                userConnectedObserver.OnNext(new ConnectedUserData
                {
                    UserId = userId
                });
            });

            signalRConnection.On<Guid>("HeIsDisconnected", userId =>
            {
                userDisconnectedObserver.OnNext(new DisconnectedUserData
                {
                    UserId = userId
                });
            });

            signalRConnection.On<Guid>("GetOutFromMyRoom", roomId =>
            {
                roomExpellingObserver.OnNext(new RoomExpellingData
                {
                    RoomId = roomId
                });
            });

            signalRConnection.On<Guid>("PleaseAddMeToYourRoom", userId =>
            {
                roomJoiningObserver.OnNext(new RoomJoiningData
                {
                    UserId = userId
                });
            });

            signalRConnection.On<Guid, string, int>("HeSaysThatHisIpIs", (userId, address, port) =>
            {
                userIPObserver.OnNext(new UserIPData
                {
                    UserId = userId,
                    IP = new IPEndPoint(IPAddress.Parse(address), port)
                });
            });

            signalRConnection.On<Guid, string>("YouAreJoinedToMyRoom", (roomId, securityKey) =>
            {
                roomJoinedObserver.OnNext(new RoomJoinedData
                {
                    RoomId = roomId,
                    SecurityKey = Convert.FromBase64String(securityKey)
                });
            });

            signalRConnection.On<Guid, string>("YouAreNotJoinedToMyRoom", (roomId, message) =>
            {
                roomRejectedObserver.OnNext(new RoomRejectedData
                {
                    RoomId = roomId,
                    Message = message
                });
            });

            signalRConnection.On<Guid>("ILeaveYourRoom", userId =>
            {
                roomLeavingObserver.OnNext(new RoomLeavingData
                {
                    UserId = userId
                });
            });
        }

        public async Task CreateMyRoomAsync()
        {
            await signalRConnection.InvokeAsync("CreateMyRoom");
        }

        public async Task<IEnumerable<RoomData>> GetAllRoomsAsync()
        {
            var httpClient = httpClientFactory.CreateClient("RoomsClient");
            var response = await httpClient.GetAsync("api/rooms");
            return await response.EnsureSuccessStatusCode()
                                 .Content
                                 .ReadAsAsync<IEnumerable<RoomData>>();
        }

        public async Task<RoomDescriptionData> GetRoomDescriptionAsync(Guid roomId)
        {
            var httpClient = httpClientFactory.CreateClient("RoomsClient");
            var response = await httpClient.GetAsync($"api/rooms/{roomId}");
            return await response.EnsureSuccessStatusCode()
                                 .Content
                                 .ReadAsAsync<RoomDescriptionData>();
        }

        public void Dispose()
        {
            signalRConnection.DisposeAsync();
        }

        public async Task DescribeMyRoomAsync(string description)
        {
            await signalRConnection.InvokeAsync("DescribeMyRoom", description);
        }

        public async Task DeleteMyRoomAsync()
        {
            await signalRConnection.InvokeAsync("DeleteMyRoom");
        }

        public async Task AskToJoinToHisRoomAsync(Guid ownerId)
        {
            await signalRConnection.InvokeAsync("IWantToJoinToYourRoom", ownerId);
        }

        public async Task TellMyIpAsync(Guid userId, IPAddress address, int port)
        {
            await signalRConnection.InvokeAsync("ITellYouMyIP", userId, address.ToString(), port);
        }

        public async Task DenyToUserToMyRoomJoiningAsync(Guid userId, string message)
        {
            await signalRConnection.InvokeAsync("IDoNotJoinYouToMyRoomBecause", userId, message);
        }

        public async Task LeaveThisRoomAsync(Guid roomId, Guid ownerId)
        {
            await signalRConnection.InvokeAsync("ILeaveThisRoom", roomId, ownerId);
        }

        public async Task JoinTheUserToMyRoomAsync(Guid userId, byte[] securityKey)
        {
            await signalRConnection.InvokeAsync("IJoinYouToMyRoom", userId, Convert.ToBase64String(securityKey));
        }

        public IDisposable Subscribe(IObserver<ConnectedUserData> observer)
        {
            userConnectedObserver.Add(observer);
            return new DisposeObserverCommand<ConnectedUserData>(userConnectedObserver, observer);
        }

        public IDisposable Subscribe(IObserver<DisconnectedUserData> observer)
        {
            userDisconnectedObserver.Add(observer);
            return new DisposeObserverCommand<DisconnectedUserData>(userDisconnectedObserver, observer);
        }

        public IDisposable Subscribe(IObserver<RoomExpellingData> observer)
        {
            roomExpellingObserver.Add(observer);
            return new DisposeObserverCommand<RoomExpellingData>(roomExpellingObserver, observer);
        }

        public IDisposable Subscribe(IObserver<RoomJoinedData> observer)
        {
            roomJoinedObserver.Add(observer);
            return new DisposeObserverCommand<RoomJoinedData>(roomJoinedObserver, observer);
        }

        public IDisposable Subscribe(IObserver<RoomJoiningData> observer)
        {
            roomJoiningObserver.Add(observer);
            return new DisposeObserverCommand<RoomJoiningData>(roomJoiningObserver, observer);
        }

        public IDisposable Subscribe(IObserver<RoomLeavingData> observer)
        {
            roomLeavingObserver.Add(observer);
            return new DisposeObserverCommand<RoomLeavingData>(roomLeavingObserver, observer);
        }

        public IDisposable Subscribe(IObserver<RoomRejectedData> observer)
        {
            roomRejectedObserver.Add(observer);
            return new DisposeObserverCommand<RoomRejectedData>(roomRejectedObserver, observer);
        }

        public IDisposable Subscribe(IObserver<UserIPData> observer)
        {
            userIPObserver.Add(observer);
            return new DisposeObserverCommand<UserIPData>(userIPObserver, observer);
        }

        public async Task KnowMyIdAsync()
        {
            await signalRConnection.SendAsync("IWantToKnowMyId");
        }

        public IDisposable Subscribe(IObserver<MyIdData> observer)
        {
            myIdObserver.Add(observer);
            return new DisposeObserverCommand<MyIdData>(myIdObserver, observer);
        }
    }
}
