using System;
using MongoDB.Bson;
using MongoDB.Driver;
using SharedModelNamespace.Shared.Helpers;
using SharedModelNamespace.Shared;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SharedModelNamespace.Shared.DBRepositories
{
    public interface IMongoDbRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetCurrentUser(string username, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<User> UpdateAsync(User user);
        Task<User> UpdatePasswordAsync(string id,string newPassword);
        Task<bool> RemoveAsync(string id);
        Task<bool> RemoveAsync(User user);
        // Task<IEnumerable<User>> GetUsersAsync();
        Task<string> CreateUserAsync(User user);
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

        public async Task<string> CreateUserAsync(User user)
        {
            try
            {
                bool Availability = CheckUsernameAvailability(user);
                if (Availability)
                {
                    await _users.InsertOneAsync(user);
                    return null;
                }
                else
                {
                    return "error: username available";
                }
            }
            catch (Exception e)
            {
                return "server internal error : "+e;
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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _users.Find(u => true).ToListAsync<User>();
        }
        
        private bool CheckUsernameAvailability(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, user.Username);
            if (_users.Find(filter).SingleOrDefault() == null) return true;
            else return false;
        }
        public async Task<User> UpdateAsync(User user) {
            
            var result = await _users.ReplaceOneAsync(x => x.Id.Equals(user.Id) , user);
            if (result.IsAcknowledged)
            {
                return await GetUserByIdAsync(user.Id);
            }
            return null;
        }

        public async Task<User> UpdatePasswordAsync(string id,string newPassword)
        {
            var usr = await GetUserByIdAsync(id);
            if(usr == null)
            {
                return null;
            }
            usr.Password = newPassword;
            return await UpdateAsync(usr);
        }

        public async Task<bool> RemoveAsync(User user) {
           var result = await _users.DeleteOneAsync(x => x.Id == user.Id);
           if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
            
        public async Task<bool> RemoveAsync(string id) {
           var result = await _users.DeleteOneAsync(x => x.Id.Equals(id));
           if (result.IsAcknowledged)
            {
                return true;
            }
            return false;
        }
    }
}
