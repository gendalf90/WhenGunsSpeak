﻿using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace RoomsService.Common.DeleteRoom
{
    class DeleteRoomStrategy : IDeleteRoomStrategy
    {
        private readonly IMongoDatabase database;

        public DeleteRoomStrategy(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task DeleteAsync(string roomId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", roomId);
            await database.GetCollection<BsonDocument>("rooms").DeleteOneAsync(filter);
        }
    }
}
