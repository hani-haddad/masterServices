using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using AuthProject.Helpers;
using SharedModelNamespace.Shared;
using AuthProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AuthProject.DBRepositories;

namespace AuthProject.Services
{
    public interface IAuthService
    {
        Task<UserClaims> Login(UserCredintials user);
        string Create(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly IMongoDbRepository _mongoDbRepository;

        private readonly IMongoCollection<User> _users;
        private readonly IJwtHelper _jwt;

        public AuthService(IMongoDbRepository mongoDbRepository,IAuthDBSettings settings, IMongoClient mongoDb, IJwtHelper jwt)
        {
            _mongoDbRepository = mongoDbRepository;
            _jwt = jwt;
        }

        public async Task<UserClaims> Login(UserCredintials user)
        {
            User currentUser = await _mongoDbRepository.GetCurrentUser(user.UserName,user.Password);
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

        public  string Create(User user)
        {
           return  _mongoDbRepository.CreateUser(user);
        }
    }
}