using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using AuthProject.Helpers;
using AuthProject.Models;
using AuthProject.ViewModels;

namespace AuthProject.Services
{
    public interface IAuthService
    {
        UserClaims Login(UserCredintials user);

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
            User currentUser = _users.Find(user => true).ToList().Where(usr => usr.Username == user.UserName && usr.Password == user.Password).FirstOrDefault();
            if (currentUser != null)
            {
                UserClaims claims = new UserClaims();

                claims.Username = user.UserName;
                claims.FirstName = currentUser.FirstName;
                claims.LastName = currentUser.LastName;
                claims.Id = currentUser.Id;
                claims.Email = currentUser.Email;
                claims.Age = currentUser.Age;
                claims.Image = currentUser.Image;
                claims.MemberInGroups = currentUser.MemberInGroups;
                claims.AdminOfGroups = currentUser.AdminOfGroups;
                claims.RecivedInvitations = currentUser.RecivedInvitations;
                claims.SentInvitations = currentUser.SentInvitations;
                claims.Token = _jwt.GenerateToken(claims);
                return claims;
            }
            return null;
        }
    }
}