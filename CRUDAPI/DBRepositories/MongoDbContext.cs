using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CRUDAPI.DBRepositories
{

    public class MongoDbContext
    {

        public bool CollectionExists(string collectionName, IMongoDatabase database = null)
        {
            if (database == null)
            {
                return false;
            }

            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return database.ListCollectionNames(options).Any();
        }


    }
}
