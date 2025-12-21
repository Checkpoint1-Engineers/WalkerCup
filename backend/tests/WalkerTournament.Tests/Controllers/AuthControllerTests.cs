using Microsoft.AspNetCore.Mvc;
using Moq;
using WalkerTournament.Api.Controllers;
using WalkerTournament.Api.DTOs;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest("test@walker.com", "password");
        var token = "valid_token";
        
        _authServiceMock.Setup(s => s.LoginAsync(request.Email, request.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AuthResult(true, Token: token));

        // Act
        var result = await _controller.Login(request, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<LoginResponse>(okResult.Value);
        Assert.Equal(token, response.Token);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new LoginRequest("test@walker.com", "wrong_password");
        
        _authServiceMock.Setup(s => s.LoginAsync(request.Email, request.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AuthResult(false, Error: "Invalid credentials"));

        // Act
        var result = await _controller.Login(request, CancellationToken.None);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var response = Assert.IsType<ErrorResponse>(unauthorizedResult.Value);
        Assert.Equal("Invalid credentials", response.Error);
    }
}
