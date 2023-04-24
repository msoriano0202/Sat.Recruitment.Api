using Sat.Recruitment.Domain;
using Sat.Recruitment.Shared;
using System.Collections.Generic;

namespace Sat.Recruitment.Contracts.Services
{
    public interface IUserDataService
    {
        List<User> GetAll();
        User GetById(int id);
        Result AddUser(User user);
        Result DeleteUserById(int id);
    }
}
