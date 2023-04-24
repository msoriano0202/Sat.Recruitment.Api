using System;
using Sat.Recruitment.Contracts.Services;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Shared;

namespace Sat.Recruitment.Services
{
    public class UserMoneyService : IUserMoneyService
    {
        public decimal AdjustMoney(User user)
        {
            switch (user.UserType)
            {
                case UserType.Normal:
                    return AdjustMoneyForNormalUser(user.Money);
                case UserType.SuperUser:
                    return AdjustMoneyForSuperUser(user.Money);
                case UserType.Premium:
                    return AdjustMoneyForPremiumUser(user.Money);
                default:
                    return user.Money;
            }
        }

        private decimal AdjustMoneyForNormalUser(decimal money)
        {
            var adjustedMoney = money;

            if (money > 100)
            {
                var percentage = Convert.ToDecimal(0.12);
                var gif = money * percentage;
                adjustedMoney = money + gif;
            }
            else if (money < 100)
            {
                if (money > 10)
                {
                    var percentage = Convert.ToDecimal(0.8);
                    var gif = money * percentage;
                    adjustedMoney = money + gif;
                }
            }

            return adjustedMoney;
        }

        private decimal AdjustMoneyForSuperUser(decimal money)
        {
            var adjustedMoney = money;

            if (money > 100)
            {
                var percentage = Convert.ToDecimal(0.20);
                var gif = money * percentage;
                adjustedMoney = money + gif;
            }

            return adjustedMoney;
        }

        private decimal AdjustMoneyForPremiumUser(decimal money)
        {
            var adjustedMoney = money;

            if (money > 100)
            {
                var gif = money * 2;
                adjustedMoney = money + gif;
            }

            return adjustedMoney;
        }
    }
}
