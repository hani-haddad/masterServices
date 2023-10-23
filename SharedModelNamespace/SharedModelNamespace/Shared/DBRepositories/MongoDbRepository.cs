using System;
using MongoDB.Bson;
using MongoDB.Driver;
using SharedModelNamespace.Shared.Helpers;
using SharedModelNamespace.Shared;
using System.Threading.Tasks;
using System.Linq;

namespace SharedModelNamespace.Shared.DBRepositories
{
    public interface IMongoDbRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetCurrentUser(string username, string password);

        // Task<IEnumerable<User>> GetUsersAsync();
        string CreateUser(User user);
    }
    public class MongoDbRepository : IMongoDbRepository
    {
        private readonly IMongoCollection<User> _users;

        public MongoDbRepository(IAuthDBSettings settings, IMongoClient mongoDb)
        {
            var database = mongoDb.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

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

        public string CreateUser(User user)
        {
            try
            {
                bool Availability = CheckUsernameAvailability(user);
                if (Availability)
                {
                    _users.InsertOne(user);
                    return null;
                }
                else
                {
                    return "error: username available";
                }
            }
            catch (Exception e)
            {
                return "server internal error";
            }
        }

        public async Task<User> GetCurrentUser(string username, string password)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(usr => usr.Username, username),
                Builders<User>.Filter.Eq(usr => usr.Password, password)
            );
            User cuurent = await _users.Find(filter).FirstOrDefaultAsync();
            return cuurent;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return await _users.Find(filter).FirstOrDefaultAsync();
        }

        private bool CheckUsernameAvailability(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, user.Username);
            if (_users.Find(filter).SingleOrDefault() == null) return true;
            else return false;
        }
    }
}
