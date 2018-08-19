using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace RoomsService.Common.DescribeRoom
{
    class DescribeRoomStrategy : IDescribeRoomStrategy
    {
        private readonly IMongoDatabase database;

        public DescribeRoomStrategy(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task DescribeAsync(string roomId, string description)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", roomId);
            var update = Builders<BsonDocument>.Update.Set("description", description);
            await database.GetCollection<BsonDocument>("rooms").UpdateOneAsync(filter, update);
        }
    }
}
