using Sat.Recruitment.Contracts.Managers;
using Sat.Recruitment.Contracts.Services;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Shared;
using System.Collections.Generic;

namespace Sat.Recruitment.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserEmailValidationService _userEmailValidationService;
        private readonly IUserMoneyService _userMoneyService;
        private readonly IUserDataService _userDataService;

        public UserManager(
            IUserEmailValidationService userEmailValidationService,
            IUserMoneyService userMoneyService,
            IUserDataService userDataService)
        {
            _userEmailValidationService = userEmailValidationService;
            _userMoneyService = userMoneyService;
            _userDataService = userDataService;
        }

        public List<User> GetAll()
        {
            return _userDataService.GetAll();
        }

        public User GetById(int id)
        {
            return _userDataService.GetById(id);
        }

        public Result AddUser(User user)
        {
            user.Email = _userEmailValidationService.NormalizeEmail(user.Email);
            user.Money = _userMoneyService.AdjustMoney(user);

            return _userDataService.AddUser(user);
        }

        public Result DeleteById(int id)
        {
            return _userDataService.DeleteUserById(id);
        }
    }
}
