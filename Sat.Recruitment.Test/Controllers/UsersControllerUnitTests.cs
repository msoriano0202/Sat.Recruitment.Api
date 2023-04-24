using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Contracts.Managers;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Dto.Requests;
using Sat.Recruitment.Dto.Response;
using Sat.Recruitment.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace Sat.Recruitment.Test.Controllers
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsersControllerUnitTests
    {
        private UsersController _controller;
        private Mock<IUserManager> _userManagerMock = new Mock<IUserManager>();
        private Mock<IMapper> _mapperMock = new Mock<IMapper>();
        Fixture _fixture;

        public UsersControllerUnitTests()
        {
            _fixture = new Fixture();
            _controller = new UsersController(_userManagerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void When_GetAll_Then_Return_UsersList()
        {
            // Arrange
            var usersList = _fixture.CreateMany<User>().ToList();
            var usersListResponse = _fixture.CreateMany<UserResponse>().ToList();

            _userManagerMock
                .Setup(x => x.GetAll())
                .Returns(usersList);

            _mapperMock
                .Setup(x => x.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(usersListResponse);

            // Act
            IActionResult result = _controller.GetAll().Result;

            // Assert
            var response = result as ObjectResult;
            var responseValue = response.Value as List<UserResponse>;

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseValue);
            Assert.Equal(usersListResponse.Count, responseValue.Count);
            Assert.Equal(usersListResponse[0].Name, responseValue[0].Name);
        }

        [Fact]
        public void Given_ValidInputs_When_GetById_Then_Return_UserResponse()
        {
            // Arrange
            var id = 1;
            var user = _fixture.Create<User>();
            var userResponse = _fixture.Create<UserResponse>();

            _userManagerMock
                .Setup(x => x.GetById(It.Is<int>(x => x == id)))
                .Returns(user);

            _mapperMock
                .Setup(x => x.Map<UserResponse>(It.Is<User>(x => x.Name == user.Name)))
                .Returns(userResponse);

            // Act
            IActionResult result = _controller.GetById(id).Result;

            // Assert
            var response = result as ObjectResult;
            var responseValue = response.Value as UserResponse;

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseValue);
            Assert.Equal(userResponse.Name, responseValue.Name);
        }

        [Fact]
        public void Given_ValidInputs_When_GetById_Then_Return_NotFound()
        {
            // Arrange
            var id = 1;
            User user = null;

            _userManagerMock
                .Setup(x => x.GetById(It.Is<int>(x => x == id)))
                .Returns(user);

            // Act
            IActionResult result = _controller.GetById(id).Result;

            // Assert
            var response = result as NotFoundResult;
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void Given_ValidInputs_When_CreateUser_Then_Return_UserCreated()
        {
            // Arrange
            var request = _fixture.Create<CreateUserRequest>();
            request.Email = "mike@gmail.com";
            request.UserType = "Normal";
            request.Money = 124;

            var userMapped = _fixture.Create<User>();
            userMapped.Name = request.Name;
            userMapped.Email = request.Email;
            userMapped.UserType = UserType.Normal;
            userMapped.Money = request.Money;

            var responseResult = new Result { IsSuccess = true, Message = "User Created" };

            _userManagerMock
                .Setup(x => x.AddUser(It.Is<User>(x => x.Name == request.Name)))
                .Returns(responseResult);

            _mapperMock
                .Setup(x => x.Map<User>(It.Is<CreateUserRequest>(x => x.Name == request.Name)))
                .Returns(userMapped);

            // Act
            IActionResult result = _controller.CreateUser(request).Result;

            // Assert
            ObjectResult response = result as ObjectResult;
            var responseValue = response.Value as Result;

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseValue.IsSuccess);
            Assert.Equal("User Created", responseValue.Message);
        }

        [Fact]
        public void Given_ValidInputs_When_CreateUser_Then_Return_UserDuplicated()
        {
            // Arrange
            var request = _fixture.Create<CreateUserRequest>();
            request.Email = "Agustina@gmail.com";
            request.UserType = "Normal";
            request.Money = 124;

            var userMapped = _fixture.Create<User>();
            userMapped.Name = request.Name;
            userMapped.Email = request.Email;
            userMapped.UserType = UserType.Normal;
            userMapped.Money = request.Money;

            var responseResult = new Result { IsSuccess = false, Message = "User Duplicated" };

            _userManagerMock
                .Setup(x => x.AddUser(It.Is<User>(x => x.Name == request.Name)))
                .Returns(responseResult);

            _mapperMock
               .Setup(x => x.Map<User>(It.Is<CreateUserRequest>(x => x.Name == request.Name)))
               .Returns(userMapped);

            // Act
            IActionResult result = _controller.CreateUser(request).Result;

            // Assert
            ObjectResult response = result as ObjectResult;
            var responseValue = response.Value as Result;

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseValue.IsSuccess);
            Assert.Equal("User Duplicated", responseValue.Message);
        }

        [Fact]
        public void Given_ValidInputs_When_DeleteUser_Then_Return_UserDeleted()
        {
            // Arrange
            var id = 1;
            var responseResult = new Result { IsSuccess = true, Message = "User Deleted" };

            _userManagerMock
                .Setup(x => x.DeleteById(It.Is<int>(x => x == id)))
                .Returns(responseResult);

            // Act
            IActionResult result = _controller.DeleteById(id).Result;

            // Assert
            ObjectResult response = result as ObjectResult;
            var responseValue = response.Value as Result;

            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseValue.IsSuccess);
            Assert.Equal("User Deleted", responseValue.Message);
        }
    }
}
