using Sat.Recruitment.Domain;
using Sat.Recruitment.Shared;
using System.Collections.Generic;

namespace Sat.Recruitment.Contracts.Managers
{
    public interface IUserManager
    {
        List<User> GetAll();
        User GetById(int id);
        Result AddUser(User user);
        Result DeleteById(int id);
    }
}
