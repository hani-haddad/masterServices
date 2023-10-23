using System;
using MongoDB.Bson;
using MongoDB.Driver;
using SharedModelNamespace.Shared;

namespace AuthProject.DBRepositories
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
