using Microsoft.Extensions.Options;
using Moq;
using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Repositories;
using WalkerTournament.Api.Services;
using WalkerTournament.Api.Utilities;

namespace WalkerTournament.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        
        var jwtSettings = new JwtSettings 
        { 
            Secret = "SuperSecretKeyForTestingOnly12345!", 
            Issuer = "TestIssuer", 
            Audience = "TestAudience",
            ExpirationMinutes = 60 
        };
        _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
        _jwtSettingsMock.Setup(s => s.Value).Returns(jwtSettings);

        _service = new AuthService(
            _userRepoMock.Object,
            _passwordHasherMock.Object,
            _jwtSettingsMock.Object
        );
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var email = "test@walker.com";
        var password = "password123";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = "hashed_password",
            Role = "Walker"
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        _passwordHasherMock.Setup(h => h.Verify(password, user.PasswordHash))
            .Returns(true);

        // Act
        var result = await _service.LoginAsync(email, password);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenUserNotFound()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginAsync("unknown@walker.com", "password");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid email or password", result.Error);
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenPasswordIsInvalid()
    {
        // Arrange
        var user = new User
        {
            Email = "test@walker.com",
            PasswordHash = "hashed_password"
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        _passwordHasherMock.Setup(h => h.Verify(It.IsAny<string>(), user.PasswordHash))
            .Returns(false);

        // Act
        var result = await _service.LoginAsync(user.Email, "wrong_password");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid email or password", result.Error);
    }
}
