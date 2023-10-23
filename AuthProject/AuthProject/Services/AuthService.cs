using System.Threading.Tasks;
using MongoDB.Driver;
using AuthProject.Helpers;
using AuthProject.ViewModels;
using SharedModelNamespace.Shared;
namespace AuthProject.Services
{

    public interface IAuthService
    {
        Task<UserClaims> Login(UserCredintials user);
        string Create(User user);
    }


    public class AuthService : IAuthService
    {
        private readonly  SharedModelNamespace.Shared.DBRepositories.IMongoDbRepository _mongoDbRepository;

        private readonly IMongoCollection<User> _users;
        private readonly IJwtHelper _jwt;

        public AuthService(SharedModelNamespace.Shared.DBRepositories.IMongoDbRepository mongoDbRepository
                            , IMongoClient mongoDb, IJwtHelper jwt)
        {
            _mongoDbRepository = mongoDbRepository;
            _jwt = jwt;
        }

        public async Task<UserClaims> Login(UserCredintials user)
        {
            User currentUser = await _mongoDbRepository.GetCurrentUser(user.UserName,user.Password);
            if (currentUser != null)
            {

                UserClaims claims = new()
                {
                    Username = user.UserName,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Id = currentUser.Id,
                    Email = currentUser.Email,
                    Phone = currentUser.Phone
                };
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