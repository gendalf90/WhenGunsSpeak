using Connection.Rooms;
using Connection.Udp;
using Connection.Udp.Messaging;
using Connection.Udp.NatFucking;
using Datagrammer;
using Microsoft.Extensions.DependencyInjection;
using Connection.Initialization;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Datagrammer.Hmac;

namespace Connection
{
    public sealed class Bootstrap
    {
        public async Task<IRoomConnection> StartRoomConnectionAsync(RoomConnectionOptions connectionOptions)
        {
            var token = await InitializeRoomTokenAsync(connectionOptions);

            var services = new ServiceCollection().Configure<RoomOptions>(options =>
                                                  {
                                                      options.Token = token;
                                                      options.Address = connectionOptions.RoomsAddress;
                                                  })
                                                  .AddObserver<ConnectedUserData>()
                                                  .AddObserver<DisconnectedUserData>()
                                                  .AddObserver<RoomExpellingData>()
                                                  .AddObserver<RoomJoinedData>()
                                                  .AddObserver<RoomJoiningData>()
                                                  .AddObserver<RoomLeavingData>()
                                                  .AddObserver<RoomRejectedData>()
                                                  .AddObserver<UserIPData>()
                                                  .AddObserver<MyIdData>()
                                                  .AddSingleton<RoomConnection>();

            services.AddHttpClient("RoomsClient", (client) =>
                    {
                        client.BaseAddress = connectionOptions.RoomsAddress;
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    });

            var connection = services.BuildServiceProvider().GetService<RoomConnection>();
            await connection.StartAsync();
            return connection;
        }

        private async Task<string> InitializeRoomTokenAsync(RoomConnectionOptions connectionOptions)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = connectionOptions.RoomsAddress;
                return await client.GetStringAsync("api/token");
            }
        }

        public Task<IMessageConnection> StartMessageConnectionAsync(MessageConnectionOptions connectionOptions)
        {
            var services = new ServiceCollection().Configure<UdpOptions>(options =>
                                                  {
                                                      options.NatFuckerAddress = connectionOptions.NatFuckerAddress;
                                                      options.NatFuckingPeriod = connectionOptions.NatFuckingPeriod;
                                                      options.SecurityKey = connectionOptions.SecurityKey;
                                                      options.UserId = connectionOptions.UserId;
                                                  })
                                                  .Configure<DatagramOptions>(options =>
                                                  {
                                                      options.ListeningPoint = connectionOptions.ListeningPoint;
                                                      options.ReceivingParallelismDegree = connectionOptions.ReceivingParallelismDegree;
                                                  })
                                                  .AddObserver<MessageData>()
                                                  .AddObserver<MyIPData>()
                                                  .AddSingleton<MessageHandler>()
                                                  .AddSingleton<IMessageHandler>(provider =>
                                                  {
                                                      var messageHander = provider.GetService<MessageHandler>();
                                                      var securityKey = connectionOptions.SecurityKey;
                                                      return new HmacSha1HandlerDecorator(messageHander, securityKey);
                                                  })
                                                  .AddSingleton<IMessageHandler, NatFuckingMessageHandler>()
                                                  .AddSingleton<UdpConnection>()
                                                  .AddDatagrammer()
                                                  .BuildServiceProvider();

            var connection = services.GetService<UdpConnection>();
            connection.Start();
            return Task.FromResult<IMessageConnection>(connection);
        }
    }
}
