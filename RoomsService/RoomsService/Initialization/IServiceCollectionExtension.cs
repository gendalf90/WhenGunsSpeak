using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using RoomsService.Common.DeleteRoom;
using RoomsService.Common.DescribeRoom;
using RoomsService.Common.GetAllRooms;
using RoomsService.Common.GetRoom;
using RoomsService.Common.GetToken;
using RoomsService.Common.CreateRoom;
using RoomsService.Logs;
using System;

namespace RoomsService.Initialization
{
    static class IServiceCollectionExtension
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var securityKey = CreateTokenSecurityKey(configuration);

            services.Configure<SecurityOptions>(options =>
                    {
                        options.SecurityKey = securityKey;
                    })
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            RequireExpirationTime = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = securityKey
                        };
                    });

            return services;
        }

        public static IServiceCollection AddSignalR(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = configuration["REDIS_CONNECTION_STRING"];
            services.AddSignalR()
                    .AddMessagePackProtocol()
                    .AddRedis(redisConnection);
            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionSring = configuration["MONGODB_CONNECTION_STRING"];
            var client = new MongoClient(connectionSring);
            var database = client.GetDatabase("rooms");
            return services.AddSingleton(database);
        }

        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["REDIS_CONNECTION_STRING"];
            var allRoomsExpirationSeconds = configuration.GetValue<double?>("distributedCache:allRoomsExpirationSeconds");
            var roomExpirationSeconds = configuration.GetValue<double?>("distributedCache:roomExpirationSeconds");

            services.Configure<CacheOptions>(options =>
            {
                if(allRoomsExpirationSeconds.HasValue)
                {
                    options.AllRoomsExpiration = TimeSpan.FromSeconds(allRoomsExpirationSeconds.Value);
                }

                if(roomExpirationSeconds.HasValue)
                {
                    options.RoomExpiration = TimeSpan.FromSeconds(roomExpirationSeconds.Value);
                }
            });

            return services.AddDistributedRedisCache(options => options.Configuration = connectionString);
        }

        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            return services.AddTransient<IDeleteRoomStrategy, DeleteRoomStrategy>()
                           .AddTransient<IDescribeRoomStrategy, DescribeRoomStrategy>()
                           .AddTransient<IGetAllRoomsStrategy, GetAllRoomsStrategy>()
                           .AddTransient<IGetRoomStrategy, GetRoomStrategy>()
                           .AddTransient<ICreateRoomStrategy, CreateRoomStrategy>()
                           .AddTransient<IGetTokenStrategy, GetTokenStrategy>();
        }

        public static IServiceCollection AddLogs(this IServiceCollection services)
        {
            return services.AddTransient<IRoomLogger, RoomLogger>();
        }

        private static SecurityKey CreateTokenSecurityKey(IConfiguration configuration)
        {
            var sign = configuration["TOKEN_SIGNING_KEY"];
            var bytes = Convert.FromBase64String(sign);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
