using Sat.Recruitment.Domain;

namespace Sat.Recruitment.Contracts.Services
{
    public interface IUserMoneyService
    {
        decimal AdjustMoney(User user);
    }
}
