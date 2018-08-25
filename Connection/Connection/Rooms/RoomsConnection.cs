using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Connection.Rooms
{
    class RoomsConnection : IHostedService
    {
        private readonly HubConnection signalRConnection;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IObserver<ConnectedUserData> userConnectedObserver;

        public RoomsConnection(IHttpClientFactory httpClientFactory)
        {
            
        }

        private void InitializeSignalRCallbacks()
        {
            signalRConnection.On<string>("IAmConnected", userId =>
            {
                userConnectedObserver.OnNext(new ConnectedUserData
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
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("api/rooms");
            return await response.EnsureSuccessStatusCode()
                                 .Content
                                 .ReadAsAsync<IEnumerable<RoomData>>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await signalRConnection.DisposeAsync();
        }
    }
}
