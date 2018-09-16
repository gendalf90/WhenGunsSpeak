using Connection.Mediator;
using Connection.Rooms;
using Connection.Udp;
using Connection.Udp.Messaging;
using Connection.Udp.NatFucking;
using Datagrammer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Datagrammer.Hmac;
using System.Net.Http.Headers;
using System;
using Connection.Initialization;

namespace Connection
{
    public sealed class Bootstrap
    {
        public IConnection Build(ConnectionOptions connectionOptions)
        {
            var host = new HostBuilder().ConfigureServices(services =>
                                        {
                                            services.AddMessaging(connectionOptions)
                                                    .AddRooms(connectionOptions)
                                                    .AddObservers();
                                        })
                                        .Start();

            return new ConnectionMediator(host);
        }
    }
}
