using AutoFixture;
using Moq;
using Sat.Recruitment.Contracts.Services;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Managers;
using Sat.Recruitment.Shared;
using System.Linq;
using Xunit;

namespace Sat.Recruitment.Test.Managers
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserManagerUnitTests
    {
        private UserManager _manager;
        private Mock<IUserEmailValidationService> _userEmailValidationServiceMock = new Mock<IUserEmailValidationService>();
        private Mock<IUserMoneyService> _userMoneyServiceMock = new Mock<IUserMoneyService>();
        private Mock<IUserDataService> _userDataServiceMock = new Mock<IUserDataService>();
        Fixture _fixture;

        public UserManagerUnitTests()
        {
            _fixture = new Fixture();
            _manager = new UserManager(_userEmailValidationServiceMock.Object, _userMoneyServiceMock.Object, _userDataServiceMock.Object);
        }

        [Fact]
        public void When_GetAll_Then_Return_UsersList()
        {
            // Arrange
            var usersList = _fixture.CreateMany<User>().ToList();

            _userDataServiceMock
                .Setup(x => x.GetAll())
                .Returns(usersList);

            // Act
            var result = _manager.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usersList.Count, result.Count);
            Assert.Equal(usersList[0].Name, result[0].Name);
        }

        [Fact]
        public void When_GetById_Then_Return_User()
        {
            // Arrange
            var id = 1;
            var user = _fixture.Create<User>();

            _userDataServiceMock
                .Setup(x => x.GetById(It.Is<int>(x => x == id)))
                .Returns(user);

            // Act
            var result = _manager.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, result.Name);
        }

        [Fact]
        public void When_AddUser_Then_Return_Result_UserAdded()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var vaildEmail = "email@email.com";
            var adjustedMoney = 11.11m;
            var resultMock = new Result { IsSuccess = true, Message = "User Added." };

            _userEmailValidationServiceMock
                .Setup(x => x.NormalizeEmail(It.Is<string>(x => x == user.Email)))
                .Returns(vaildEmail);

            _userMoneyServiceMock
               .Setup(x => x.AdjustMoney(It.IsAny<User>()))
               .Returns(adjustedMoney);

            _userDataServiceMock
                .Setup(x => x.AddUser(It.IsAny<User>()))
                .Returns(resultMock);

            // Act
            var result = _manager.AddUser(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.IsSuccess, resultMock.IsSuccess);
            Assert.Equal(result.Message, resultMock.Message);
        }

        [Fact]
        public void When_DeleteById_Then_Return_Result_UserDeleted()
        {
            // Arrange
            var id = 1;
            var resultMock = new Result { IsSuccess = true, Message = "User Deleted." };

            _userDataServiceMock
                .Setup(x => x.DeleteUserById(It.Is<int>(x => x == id)))
                .Returns(resultMock);

            // Act
            var result = _manager.DeleteById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.IsSuccess, resultMock.IsSuccess);
            Assert.Equal(result.Message, resultMock.Message);
        }
    }
}
