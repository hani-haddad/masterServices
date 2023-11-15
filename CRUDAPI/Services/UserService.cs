using System;
using SharedModelNamespace.Shared;
using SharedModelNamespace.Shared.ViewModels;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CRUDAPI.Services
{
    public interface IUserService
    {
        Task<List<UserViewModel>> Get();
        Task<UserViewModel> Get(string id);
        Task<string> Create(User user);
        Task<User> Update(User user);
        Task<User> UpdatePassword(string id, string password);
        Task<bool> Remove(User user);
        Task<bool> Remove(string id);
    }
        public class UserService : IUserService
	{
        private readonly SharedModelNamespace.Shared.DBRepositories.IMongoDbRepository _mongoDbRepository;

        public UserService(SharedModelNamespace.Shared.DBRepositories.IMongoDbRepository mongoDbRepository )
		{
            _mongoDbRepository = mongoDbRepository;

        }

        public  async Task<string> Create(User user)
        {
           return  await _mongoDbRepository.CreateUserAsync(user);
        }

        public async Task<List<UserViewModel>> Get()
        {
            List<UserViewModel> userViewModelList = new List<UserViewModel>();
            var usrList = await _mongoDbRepository.GetAllUsersAsync(); 
            foreach (var usr in usrList)
            {
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.Id = usr.Id;
                userViewModel.Username = usr.Username;
                userViewModel.Email = usr.Email;
                userViewModel.FirstName = usr.FirstName;
                userViewModel.LastName = usr.LastName;
                userViewModel.Phone = usr.Phone;
                userViewModelList.Add(userViewModel);
            }
            return  userViewModelList;
        }

        public async Task<UserViewModel> Get(string id)
        {
            User user = await _mongoDbRepository.GetUserByIdAsync(id);
            UserViewModel _usr = new UserViewModel();
            _usr.Username = user.Username;
            _usr.FirstName = user.FirstName;
            _usr.LastName = user.LastName;
            _usr.Id = user.Id;
            _usr.Email = user.Email;
            _usr.Phone = user.Phone;
            return  _usr;
        }

        public async Task<bool> Remove(User user)
        {
           return await _mongoDbRepository.RemoveAsync(user);
        }

        public async Task<bool> Remove(string id)
        {
           return await _mongoDbRepository.RemoveAsync(id);
        }

        public async Task<User> Update(User user)
        {
           return await _mongoDbRepository.UpdateAsync(user);
        }

        public async Task<User> UpdatePassword(string id, string password)
        {
           return await _mongoDbRepository.UpdatePasswordAsync(id, password);
        }
    }
}

