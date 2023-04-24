using Sat.Recruitment.Services;
using Xunit;

namespace Sat.Recruitment.Test.Services
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserEmailValidationServiceUnitTests
    {
        private UserEmailValidationService _service;

        public UserEmailValidationServiceUnitTests()
        {
            _service = new UserEmailValidationService();
        }

        [Fact]
        public void InvalidEmail_When_NormalizeEmail_Then_Return_NormalizedEmail()
        {
            // Arrange
            var invalidEmail = "email.+email@email.com";
            var normalizedEmail = "email@email.com";

            // Act
            var result = _service.NormalizeEmail(invalidEmail);

            // Assert
            Assert.Equal(normalizedEmail, result);
        }
    }
}
