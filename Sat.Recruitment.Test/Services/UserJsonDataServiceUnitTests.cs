using AutoFixture;
using Moq;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Services;
using Sat.Recruitment.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sat.Recruitment.Test.Services
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserJsonDataServiceUnitTests
    {
        private UserJsonDataService _service;
        private Mock<IJsonFileHelper<List<User>>> _jsonFileHelper = new Mock<IJsonFileHelper<List<User>>>();
        Fixture _fixture;

        public UserJsonDataServiceUnitTests()
        {
            _fixture = new Fixture();
            _service = new UserJsonDataService(_jsonFileHelper.Object);
        }

        [Fact]
        public void When_GetAll_Then_Return_UsersList()
        {
            // Arrange
            var users = _fixture.CreateMany<User>().ToList();

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(users.Count, result.Count);
            Assert.Equal(users[0].Name, result[0].Name);
        }

        [Fact]
        public void When_GetById_Then_Return_User()
        {
            // Arrange
            var id = 1;
            var users = _fixture.CreateMany<User>().ToList();
            users.FirstOrDefault().Id = id;

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            // Act
            var result = _service.GetById(id);

            // Assert           
            Assert.Equal(users[0].Name, result.Name);
        }

        [Fact]
        public void When_AddUser_Then_Return_UserCreated()
        {
            // Arrange
            var users = _fixture.CreateMany<User>().ToList();
            var newUser = _fixture.Create<User>();

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            // Act
            var result = _service.AddUser(newUser);

            // Assert           
            Assert.True(result.IsSuccess);
            Assert.Equal("User created", result.Message);
        }

        [Fact]
        public void When_AddUser_Then_Return_UserCouldNotBeCreated()
        {
            // Arrange
            var users = _fixture.CreateMany<User>().ToList();
            var newUser = _fixture.Create<User>();

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            _jsonFileHelper
                .Setup(x => x.SaveFile(It.IsAny<List<User>>(), It.IsAny<string>()))
                .Throws(new Exception());

            // Act
            var result = _service.AddUser(newUser);

            // Assert           
            Assert.False(result.IsSuccess);
            Assert.Equal("User could not be created.", result.Message);
        }

        [Fact]
        public void When_AddUser_Then_Return_TheUserIsDuplicatedByEmailPhone()
        {
            // Arrange
            var users = _fixture.CreateMany<User>().ToList();
            var newUser = _fixture.Create<User>();
            newUser.Email = users[0].Email;
            newUser.Phone = users[0].Phone;

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            // Act
            var result = _service.AddUser(newUser);

            // Assert           
            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Message);
        }

        [Fact]
        public void When_AddUser_Then_Return_TheUserIsDuplicatedByNameAddress()
        {
            // Arrange
            var users = _fixture.CreateMany<User>().ToList();
            var newUser = _fixture.Create<User>();
            newUser.Name = users[0].Name;
            newUser.Address = users[0].Address;

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            // Act
            var result = _service.AddUser(newUser);

            // Assert           
            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Message);
        }

        [Fact]
        public void When_DeleteUserById_Then_Return_UserDeleted()
        {
            // Arrange
            var id = 1;
            var users = _fixture.CreateMany<User>().ToList();
            users[0].Id = id;

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            // Act
            var result = _service.DeleteUserById(id);

            // Assert           
            Assert.True(result.IsSuccess);
            Assert.Equal("User deleted", result.Message);
        }

        [Fact]
        public void When_DeleteUserById_Then_Return_TheUserDoesNotExists()
        {
            // Arrange
            var id = 1;
            var users = _fixture.CreateMany<User>().ToList();

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            // Act
            var result = _service.DeleteUserById(id);

            // Assert           
            Assert.False(result.IsSuccess);
            Assert.Equal("The user does not exists.", result.Message);
        }

        [Fact]
        public void When_DeleteUserById_Then_Return_TheUserCouldNotBeDeleted()
        {
            // Arrange
            var id = 1;
            var users = _fixture.CreateMany<User>().ToList();
            users[0].Id = id;

            _jsonFileHelper
                .Setup(x => x.ReadFile(It.IsAny<string>()))
                .Returns(users);

            _jsonFileHelper
               .Setup(x => x.SaveFile(It.IsAny<List<User>>(), It.IsAny<string>()))
               .Throws(new Exception());

            // Act
            var result = _service.DeleteUserById(id);

            // Assert           
            Assert.False(result.IsSuccess);
            Assert.Equal("User could not be deleted.", result.Message);
        }
    }
}
