using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace RoomsService.Common.SaveNewRoom
{
    class SaveNewRoomStrategy : ISaveNewRoomStrategy
    {
        private readonly IMongoDatabase database;

        public SaveNewRoomStrategy(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task SaveAsync(string roomId, string ownerId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", roomId);
            var update = Builders<BsonDocument>.Update.SetOnInsert("owner", ownerId);
            var options = new UpdateOptions { IsUpsert = true };
            await database.GetCollection<BsonDocument>("rooms").UpdateOneAsync(filter, update, options);
        }
    }
}
