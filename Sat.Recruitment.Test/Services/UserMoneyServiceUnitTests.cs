using Xunit;
using AutoFixture;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Services;

namespace Sat.Recruitment.Test.Services
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserMoneyServiceUnitTests
    {
        private UserMoneyService _service;
        Fixture _fixture;

        public UserMoneyServiceUnitTests()
        {
            _fixture = new Fixture();
            _service = new UserMoneyService();
        }

        [Fact]
        public void When_MoneyGreatherThan100_AdjustMoneyForNormalUser_Then_Return_AdjustedMoney()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.UserType = Shared.UserType.Normal;
            user.Money = 101;

            // Act
            var result = _service.AdjustMoney(user);

            // Assert
            Assert.Equal(user.Money + user.Money * 0.12m, result);
        }

        [Fact]
        public void When_MoneyLessThan100_AdjustMoneyForNormalUser_Then_Return_AdjustedMoney()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.UserType = Shared.UserType.Normal;
            user.Money = 11;

            // Act
            var result = _service.AdjustMoney(user);

            // Assert
            Assert.Equal(user.Money + user.Money * 0.8m, result);
        }

        [Fact]
        public void When_GreatherThan100_AdjustMoneyForSuperUser_Then_Return_AdjustedMoney()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.UserType = Shared.UserType.SuperUser;
            user.Money = 101;

            // Act
            var result = _service.AdjustMoney(user);

            // Assert
            Assert.Equal(user.Money + user.Money * 0.20m, result);
        }

        [Fact]
        public void When_GreatherThan100_AdjustMoneyForPremiumUser_Then_Return_AdjustedMoney()
        {
            // Arrange
            var user = _fixture.Create<User>();
            user.UserType = Shared.UserType.Premium;
            user.Money = 101;

            // Act
            var result = _service.AdjustMoney(user);

            // Assert
            Assert.Equal(user.Money + user.Money * 2, result);
        }
    }
}
