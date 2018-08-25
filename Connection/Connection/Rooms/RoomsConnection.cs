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

        public RoomsConnection(IHttpClientFactory httpClientFactory)
        {
            
        }

        private void InitializeSignalRCallbacks()
        {
            signalRConnection.On<string>("IAmConnected", userId =>
            {
                OnUserConnected(this, new UserConnectedEventArgs(userId));
            });
        }

        public async Task CreateMyRoomAsync()
        {
            await signalRConnection.InvokeAsync("CreateMyRoom");
        }

        public async Task<IEnumerable<RoomShortDto>> GetAllRoomsAsync()
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("api/rooms");
            return await response.EnsureSuccessStatusCode()
                                 .Content
                                 .ReadAsAsync<IEnumerable<RoomShortDto>>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<UserConnectedEventArgs> OnUserConnected;
    }
}
