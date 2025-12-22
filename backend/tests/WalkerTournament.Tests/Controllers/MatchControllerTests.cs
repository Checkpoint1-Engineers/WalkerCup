using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WalkerTournament.Api.Controllers;
using WalkerTournament.Api.DTOs;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Tests.Controllers;

public class MatchControllerTests
{
    private readonly Mock<IMatchService> _serviceMock;
    private readonly MatchController _controller;

    public MatchControllerTests()
    {
        _serviceMock = new Mock<IMatchService>();
        _controller = new MatchController(_serviceMock.Object);

        // Setup User context
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
    }

    [Fact]
    public async Task SetWinner_ShouldReturnOk_WhenSuccess()
    {
        // Arrange
        var request = new SetWinnerRequest(Guid.NewGuid());
        _serviceMock.Setup(s => s.SetWinnerAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceResult(true));

        // Act
        var result = await _controller.SetWinner(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task SetWinner_ShouldReturnConflict_WhenConcurrencyError()
    {
        // Arrange
        var request = new SetWinnerRequest(Guid.NewGuid());
        _serviceMock.Setup(s => s.SetWinnerAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceResult(false, Error: "Match was modified by another user."));

        // Act
        var result = await _controller.SetWinner(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        var error = Assert.IsType<ErrorResponse>(conflictResult.Value);
        Assert.Contains("modified by another user", error.Error);
    }

    [Fact]
    public async Task SetWinner_ShouldReturnBadRequest_WhenLogicError()
    {
        // Arrange
        var request = new SetWinnerRequest(Guid.NewGuid());
        _serviceMock.Setup(s => s.SetWinnerAsync(
            It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceResult(false, Error: "Invalid winner"));

        // Act
        var result = await _controller.SetWinner(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var error = Assert.IsType<ErrorResponse>(badRequest.Value);
        Assert.Equal("Invalid winner", error.Error);
    }
}
