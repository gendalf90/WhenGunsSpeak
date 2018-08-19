﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RoomsService.Controllers.Rooms;
using RoomsService.Initialization;
using System.Threading.Tasks;

namespace RoomsService.Common.GetRoom
{
    class GetRoomStrategy : IGetRoomStrategy
    {
        static GetRoomStrategy()
        {
            BsonClassMap.RegisterClassMap<RoomDto>(cm =>
            {
                cm.MapIdProperty(e => e.RoomId);
                cm.MapProperty(e => e.OwnerId).SetElementName("owner");
                cm.MapProperty(e => e.Description).SetElementName("description");
            });
        }

        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<CacheOptions> cacheOptions;

        public GetRoomStrategy(IDistributedCache cache,
                               IMongoDatabase database,
                               IOptions<CacheOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public async Task<RoomDto> GetByIdAsync(string id)
        {
            var cached = await GetFromCacheByIdAsync(id);

            if (cached != null)
            {
                return cached;
            }

            var stored = await GetFromDatabaseByIdAsync(id);
            await AddToCacheAsync(stored);
            return stored;
        }

        private async Task<RoomDto> GetFromCacheByIdAsync(string id)
        {
            var key = GetCacheKeyById(id);
            var bson = await cache.GetAsync(key);

            if (bson != null)
            {
                return BsonSerializer.Deserialize<RoomDto>(bson);
            }

            return null;
        }

        private async Task<RoomDto> GetFromDatabaseByIdAsync(string id)
        {
            return await database.GetCollection<RoomDto>(CollectionName)
                                 .Find(room => room.RoomId == id)
                                 .FirstOrDefaultAsync();
        }

        private async Task AddToCacheAsync(RoomDto room)
        {
            var key = GetCacheKeyById(room.RoomId);
            var bson = room.ToBson();
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheOptions.Value.RoomExpiration
            };
            await cache.SetAsync(key, bson, options);
        }

        private string GetCacheKeyById(string id) => $"room:id:{id}";

        private string CollectionName => "rooms";
    }
}
