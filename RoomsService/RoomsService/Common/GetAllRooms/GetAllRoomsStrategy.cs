using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using RoomsService.Controllers.Rooms;
using RoomsService.Initialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoomsService.Common.GetAllRooms
{
    class GetAllRoomsStrategy : IGetAllRoomsStrategy
    {
        static GetAllRoomsStrategy()
        {
            BsonClassMap.RegisterClassMap<RoomShortDto>(cm =>
            {
                cm.MapIdProperty(e => e.OwnerId);
                cm.MapProperty(e => e.Header).SetElementName("header");
            });
        }

        private readonly IDistributedCache cache;
        private readonly IMongoDatabase database;
        private readonly IOptions<CacheOptions> cacheOptions;

        public GetAllRoomsStrategy(IDistributedCache cache,
                                   IMongoDatabase database,
                                   IOptions<CacheOptions> cacheOptions)
        {
            this.cache = cache;
            this.database = database;
            this.cacheOptions = cacheOptions;
        }

        public async Task<IEnumerable<RoomShortDto>> GetAsync()
        {
            var cached = await GetFromCacheAsync();

            if(cached != null)
            {
                return cached;
            }

            var stored = await GetFromDatabaseAsync();
            await AddToCacheAsync(stored);
            return stored;
        }

        private async Task<IEnumerable<RoomShortDto>> GetFromCacheAsync()
        {
            var json = await cache.GetStringAsync(CacheKey);

            if(json != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<RoomShortDto>>(json);
            }

            return null;
        }

        private async Task<IEnumerable<RoomShortDto>> GetFromDatabaseAsync()
        {
            return await database.GetCollection<RoomShortDto>(CollectionName)
                                 .AsQueryable()
                                 .ToListAsync();
        }

        private async Task AddToCacheAsync(IEnumerable<RoomShortDto> rooms)
        {
            var json = JsonConvert.SerializeObject(rooms);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheOptions.Value.AllRoomsExpiration
            };
            await cache.SetStringAsync(CacheKey, json, options);
        }

        private string CacheKey => "rooms:all";

        private string CollectionName => "rooms";
    }
}
