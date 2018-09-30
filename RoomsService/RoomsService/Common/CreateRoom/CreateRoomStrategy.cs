using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace RoomsService.Common.CreateRoom
{
    class CreateRoomStrategy : ICreateRoomStrategy
    {
        private readonly IMongoDatabase database;

        public CreateRoomStrategy(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task CreateAsync(string ownerId, string header)
        {
            await database.GetCollection<BsonDocument>("rooms").InsertOneAsync(new BsonDocument
            {
                ["_id"] = ownerId,
                ["header"] = header
            });
        }
    }
}
