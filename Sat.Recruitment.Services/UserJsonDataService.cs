using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sat.Recruitment.Contracts.Services;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Shared;

namespace Sat.Recruitment.Services
{
    public class UserJsonDataService : IUserDataService
    {
        private const string path = "/Files/usersDB.json";
        private readonly IJsonFileHelper<List<User>> _usersFileHelper;

        public UserJsonDataService(IJsonFileHelper<List<User>> usersFileHelper)
        {
            _usersFileHelper = usersFileHelper;
        }

        private string GetFilePath()
        {
            return $"{Directory.GetCurrentDirectory()}{path}";
        }

        private List<User> LoadData()
        {
            return _usersFileHelper.ReadFile(GetFilePath());
        }

        private bool ExistsUser(List<User> usersDB, User newUser)
        {
            var existsUser = usersDB.Exists(x => x.Email == newUser.Email ||
                                            x.Phone == newUser.Phone);
            if (!existsUser)
            {
                existsUser = usersDB.Exists(x => x.Name == newUser.Name &&
                                            x.Address == newUser.Address);
            }

            return existsUser;
        }

        private bool SaveUsersToDB(List<User> usersDB)
        {
            var saved = true;
            try
            {
                _usersFileHelper.SaveFile(usersDB, GetFilePath());
            }
            catch (Exception ex)
            {
                saved = false;
                Console.WriteLine($"Error saving Users List: {ex.Message}");
            }

            return saved;
        }

        private bool InsertUser(List<User> usersDB, User newUser)
        {
            var id = usersDB.Max(u => u.Id);
            newUser.Id = id + 1;
            usersDB.Add(newUser);

            return SaveUsersToDB(usersDB);
        }

        public List<User> GetAll()
        {
            return LoadData();
        }

        public User GetById(int id)
        {
            var usersFromDB = LoadData();
            var user = usersFromDB.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public Result AddUser(User user)
        {
            var usersFromDB = LoadData();
            var existsUser = ExistsUser(usersFromDB, user);

            var result = new Result();
            if (existsUser)
            {
                result.Message = "The user is duplicated";
                return result;
            }

            var inserted = InsertUser(usersFromDB, user);
            if (inserted)
            {
                result.IsSuccess = true;
                result.Message = "User created";
            }
            else
                result.Message = "User could not be created.";

            return result;
        }

        public Result DeleteUserById(int id)
        {
            var result = new Result();

            var usersFromDB = LoadData();
            var user = usersFromDB.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                result.Message = "The user does not exists.";
                return result;
            }

            usersFromDB.Remove(user);
            var saved = SaveUsersToDB(usersFromDB);
            if (saved)
            {
                result.IsSuccess = true;
                result.Message = "User deleted";
            }
            else
                result.Message = "User could not be deleted.";

            return result;
        }
    }
}
