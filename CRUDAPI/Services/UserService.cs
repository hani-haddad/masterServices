using System;
using System.Collections.Generic;
using CRUDAPI.DBRepositories;
using MongoDB.Driver;
using NuGet.Configuration;
using SharedModelNamespace.Shared;
using SharedModelNamespace.Shared.ViewModels;

namespace CRUDAPI.Services
{

public interface IUserService
    {
        List<UserViewModel> Get();
        UserViewModel Get(string id);
        User Create(User user);
        void Update(string id, User user);
        void Remove(User user);
        void Remove(string id);
    }
	public class UserService : MongoDbContext , IUserService
	{
        private IMongoCollection<User> _users;

        public UserService(IAuthDBSettings settings, IMongoClient mongoDb)
        {
            // database: Represents the Mongo database for performing operations.
            // The constructor of this class is provided the MongoDB Database Name

            var database = mongoDb.GetDatabase(settings.DatabaseName);

            //check if Collection exiest to avoid any dropdown
            if (!base.CollectionExists("Users", database))
            {
                database.CreateCollection(name: "Users");
            }

            // _users: To access to data in a specific collection.
            // Perform CRUD operations against the collection after this method is called
            _users = database.GetCollection<User>("Users");


        }

        public List<UserViewModel> Get()
        {
            List<UserViewModel> userViewModelList = new List<UserViewModel>();
            var usrList = _users.Find(user => true).ToList();
            if (usrList != null)
            {
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
                return userViewModelList;
            }


            return null;
        }

        public UserViewModel Get(string id)
        {
            UserViewModel userViewModel = new UserViewModel();
            var usr = _users.Find<User>(x => x.Id.Equals(id)).FirstOrDefault();
            if (usr != null)
            {
                userViewModel.Id = usr.Id;
                userViewModel.Username = usr.Username;
                userViewModel.Email = usr.Email;
                userViewModel.FirstName = usr.FirstName;
                userViewModel.LastName = usr.LastName;
                userViewModel.Phone = usr.Phone;

                return userViewModel;
            }

            return null;
        }

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Update(string id, User user) =>
            _users.ReplaceOne(x => x.Id.Equals(id), user);

        public void UpdatePassword(string id, string password)
        {
            var usr = _users.Find<User>(x => x.Id.Equals(id)).FirstOrDefault();
            if (usr == null)
            {
                return;
            }
            usr.Password = password;
            Update(id, usr);
        }

        public void Remove(User user) =>
            _users.DeleteOne(x => x.Id == user.Id);

        public void Remove(string id) =>
            _users.DeleteOne(x => x.Id.Equals(id));


    }
}

