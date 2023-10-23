using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using AuthProject.Helpers;
using AuthProject.Models;
using SharedModelNamespace.Shared;
using AuthProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuthProject.Services
{
    public interface IAuthService
    {
        UserClaims Login(UserCredintials user);
        string Create(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IJwtHelper _jwt;

        public AuthService(IAuthDBSettings settings, IMongoClient mongoDb, IJwtHelper jwt)
        {
            // database: Represents the Mongo database for performing operations.
            // The constructor of this class is provided the MongoDB Database Name

            var database = mongoDb.GetDatabase(settings.DatabaseName);

            // _users: To access to data in a specific collection.
            // Perform CRUD operations against the collection after this method is called
            _users = database.GetCollection<User>("Users");

            _jwt = jwt;
        }

        public UserClaims Login(UserCredintials user)
        {
            List<User> Userrrrr = new List<User>();

            User currentUser = _users.Find(user => true).ToList().Where(usr => usr.Username == user.UserName && usr.Password == user.Password).FirstOrDefault();
            if (currentUser != null)
            {
                UserClaims claims = new UserClaims();

                claims.Username = user.UserName;
                claims.FirstName = currentUser.FirstName;
                claims.LastName = currentUser.LastName;
                claims.Id = currentUser.Id;
                claims.Email = currentUser.Email;
                claims.Phone = currentUser.Phone;
                claims.Token = _jwt.GenerateToken(claims);
                return claims;
            }
            return null;
        }

        public string Create(User user)
        {
            try
            {
                bool Availability  = CheckUsernameAvailability(user);
                if (Availability){
                    _users.InsertOne(user);
                    return null;
                }
                else{
                    return "error: username available";
                }
                
            }
            catch(Exception e)
            {
                return "server internal error";
            }
                
        }
        private bool CheckUsernameAvailability(User user){
        var filter = Builders<User>.Filter.Eq(u => u.Username, user.Username);
        if(_users.Find(filter).SingleOrDefault() == null) return true;
        else return false;
        }
    }
}