using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Connection.Rooms
{
    class RoomsConnection : IRoomsConnection
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IToken token;
        private readonly IObserver<ConnectedUserData> userConnectedObserver;
        private readonly IObserver<DisconnectedUserData> userDisconnectedObserver;
        private readonly IObserver<RoomExpellingData> roomExpellingObserver;
        private readonly IObserver<RoomJoiningRequestData> roomAddingRequestObserver;
        private readonly IObserver<UserIPData> userIPObserver;
        private readonly IObserver<RoomJoiningData> roomJoiningObserver;
        private readonly IObserver<RoomRejectingData> roomRejectingObserver;
        private readonly IObserver<RoomLeavingData> roomLeavingObserver;
        private readonly IOptions<RoomsOptions> options;

        private HubConnection signalRConnection;

        public RoomsConnection(IHttpClientFactory httpClientFactory,
                               IToken token,
                               IObserver<ConnectedUserData> userConnectedObserver,
                               IObserver<DisconnectedUserData> userDisconnectedObserver,
                               IObserver<RoomExpellingData> roomExpellingObserver,
                               IObserver<RoomJoiningRequestData> roomAddingRequestObserver,
                               IObserver<UserIPData> userIPObserver,
                               IObserver<RoomJoiningData> roomJoiningObserver,
                               IObserver<RoomRejectingData> roomRejectingObserver,
                               IObserver<RoomLeavingData> roomLeavingObserver,
                               IOptions<RoomsOptions> options)
        {
            this.httpClientFactory = httpClientFactory;
            this.token = token;
            this.userConnectedObserver = userConnectedObserver;
            this.userDisconnectedObserver = userDisconnectedObserver;
            this.roomExpellingObserver = roomExpellingObserver;
            this.roomAddingRequestObserver = roomAddingRequestObserver;
            this.userIPObserver = userIPObserver;
            this.roomJoiningObserver = roomJoiningObserver;
            this.roomRejectingObserver = roomRejectingObserver;
            this.roomLeavingObserver = roomLeavingObserver;
            this.options = options;

            InitializeSignalRHub();
            InitializeSignalRCallbacks();
        }

        private void InitializeSignalRHub()
        {
            signalRConnection = new HubConnectionBuilder().WithUrl(new Uri(options.Value.RoomsAddress, "signalr/rooms"), options =>
                                                          {
                                                              options.AccessTokenProvider = () =>
                                                              {
                                                                  return Task.FromResult(token.Get());
                                                              };
                                                          })
                                                          .AddMessagePackProtocol()
                                                          .Build();
        }

        private void InitializeSignalRCallbacks()
        {
            signalRConnection.On<Guid>("IAmConnected", userId =>
            {
                userConnectedObserver.OnNext(new ConnectedUserData
                {
                    UserId = userId
                });
            });

            signalRConnection.On<Guid>("IAmDisconnected", userId =>
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
                roomAddingRequestObserver.OnNext(new RoomJoiningRequestData
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

            signalRConnection.On<Guid>("YouAreJoinedToMyRoom", roomId =>
            {
                roomJoiningObserver.OnNext(new RoomJoiningData
                {
                    RoomId = roomId
                });
            });

            signalRConnection.On<Guid, string>("YouAreNotJoinedToMyRoom", (roomId, message) =>
            {
                roomRejectingObserver.OnNext(new RoomRejectingData
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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await signalRConnection.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await signalRConnection.StopAsync(cancellationToken);
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

        public async Task JoinTheUserToMyRoomAsync(Guid userId)
        {
            await signalRConnection.InvokeAsync("IJoinYouToMyRoom", userId);
        }

        public async Task DenyToUserToMyRoomJoiningAsync(Guid userId, string message)
        {
            await signalRConnection.InvokeAsync("IDoNotJoinYouToMyRoomBecause", userId, message);
        }

        public async Task LeaveThisRoomAsync(Guid roomId, Guid ownerId)
        {
            await signalRConnection.InvokeAsync("ILeaveThisRoom", roomId, ownerId);
        }
    }
}
