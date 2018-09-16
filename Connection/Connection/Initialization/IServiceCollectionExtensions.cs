using Connection.Mediator;
using Connection.Rooms;
using Connection.Udp;
using Connection.Udp.Messaging;
using Connection.Udp.NatFucking;
using Datagrammer;
using Datagrammer.Hmac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;

namespace Connection.Initialization
{
    internal static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddObservers(this IServiceCollection services)
        {
            return services.AddObserver<ConnectedUserData>()
                           .AddObserver<DisconnectedUserData>()
                           .AddObserver<MessageData>()
                           .AddObserver<MyIPData>()
                           .AddObserver<RoomExpellingData>()
                           .AddObserver<RoomJoiningData>()
                           .AddObserver<RoomJoiningRequestData>()
                           .AddObserver<RoomLeavingData>()
                           .AddObserver<RoomRejectingData>()
                           .AddObserver<UserIPData>();
        }

        public static IServiceCollection AddObserver<T>(this IServiceCollection services)
        {
            return services.AddSingleton<IObserverComposite<T>, ObserverComposite<T>>()
                           .AddSingleton<IObserver<T>>(provider => provider.GetService<IObserverComposite<T>>());
        }

        public static IServiceCollection AddRooms(this IServiceCollection services, ConnectionOptions connectionOptions)
        {
            services.Configure<RoomsOptions>(options =>
                    {
                        options.RoomsAddress = connectionOptions.RoomsAddress;
                    })
                    .AddSingleton<IRoomsConnection, RoomsConnection>()
                    .AddSingleton<IHostedService>(provider =>
                    {
                        return provider.GetService<IRoomsConnection>();
                    })
                    .AddSingleton<IToken>(new JwtToken(connectionOptions.SecurityKey, connectionOptions.UserId))
                    .AddHttpClient("RoomsClient", (provider, client) =>
                    {
                        var token = provider.GetService<IToken>();
                        client.BaseAddress = connectionOptions.RoomsAddress;
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Get());
                    });

            return services;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection services, ConnectionOptions connectionOptions)
        {
            return services.Configure<UdpOptions>(options =>
                           {
                               options.MessagingEndPoint = connectionOptions.MessagingEndPoint;
                               options.NatFuckerAddress = connectionOptions.NatFuckerAddress;
                               options.NatFuckingPeriod = connectionOptions.NatFuckingPeriod;
                               options.UserId = connectionOptions.UserId;
                           })
                           .AddSingleton<IMessageHandler, MessageHandler>()
                           .AddSingleton<IMessageHandler, NatFuckingMessageHandler>()
                           .AddSingleton<IUdpConnection, UdpConnection>()
                           .AddSingleton<IMiddleware>(new HmacSha1Middleware(connectionOptions.SecurityKey))
                           .AddSingleton<IHostedService>(provider =>
                           {
                               return provider.GetService<IUdpConnection>();
                           })
                           .AddHostedDatagrammer();
        }
    }
}
