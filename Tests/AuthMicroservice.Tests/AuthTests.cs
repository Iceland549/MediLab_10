using AuthMicroservice.Application.DTOs;
using AuthMicroservice.Application.Interfaces;
using AuthMicroservice.Application.Services;
using AuthMicroservice.Domain.Models;
using AuthMicroservice.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.AuthMicroservice.Tests
{
    public class AuthTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "this_is_a_very_long_jwt_key_for_production_which_is_at_least_32_bytes_long"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _authService = new AuthService(_userRepositoryMock.Object, _configuration);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsToken_WhenCredentialsAreValid()
        {
            var userDto = new UserDto { UserName = "testuser", Password = "password" };
            var user = new User { UserName = "testuser", Id = "1" };
            _userRepositoryMock.Setup(r => r.GetByUserNameAsync("testuser")).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.CheckPasswordAsync(user, "password")).ReturnsAsync(true);
            var token = await _authService.AuthenticateAsync(userDto);
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            var userDto = new UserDto { UserName = "noexist", Password = "password" };
            _userRepositoryMock.Setup(r => r.GetByUserNameAsync("noexist")).ReturnsAsync((User?)null);
            var token = await _authService.AuthenticateAsync(userDto);
            Assert.Null(token);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsNull_WhenPasswordIsInvalid()
        {
            var userDto = new UserDto { UserName = "testuser", Password = "wrongpassword" };
            var user = new User { UserName = "testuser", Id = "1" };
            _userRepositoryMock.Setup(r => r.GetByUserNameAsync("testuser")).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.CheckPasswordAsync(user, "wrongpassword")).ReturnsAsync(false);
            var token = await _authService.AuthenticateAsync(userDto);
            Assert.Null(token);
        }
    }
}
