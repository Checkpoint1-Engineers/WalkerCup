using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WalkerTournament.Api.Controllers;
using WalkerTournament.Api.DTOs;
using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Tests.Controllers;

public class TournamentControllerTests
{
    private readonly Mock<ITournamentService> _serviceMock;
    private readonly TournamentController _controller;

    public TournamentControllerTests()
    {
        _serviceMock = new Mock<ITournamentService>();
        _controller = new TournamentController(_serviceMock.Object);

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
    public async Task Create_ShouldReturnCreated_WhenSuccess()
    {
        // Arrange
        var request = new CreateTournamentRequest(
            "New Tourney", "Desc", DateTime.UtcNow.AddDays(1), 100, 16, null
        );
        var createdTournament = new Tournament 
        { 
            Id = Guid.NewGuid(), 
            Name = request.Name,
            CreatedBy = new User()
        };

        _serviceMock.Setup(s => s.CreateAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), 
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), 
            It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceResult<Tournament>(true, Data: createdTournament));

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(TournamentController.GetById), createdResult.ActionName);
        var response = Assert.IsType<TournamentListResponse>(createdResult.Value);
        Assert.Equal(createdTournament.Id, response.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenMissing()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceResult<Tournament>(false, Error: "Not found"));

        // Act
        var result = await _controller.GetById(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Join_ShouldReturnOk_WhenSuccess()
    {
        // Arrange
        var request = new JoinTournamentRequest(123, "Walker1");
        _serviceMock.Setup(s => s.JoinAsync(It.IsAny<Guid>(), 123, "Walker1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceResult(true));

        // Act
        var result = await _controller.Join(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        // The controller returns Ok(new { message = "..." }) which is an anonymous object
        // We just verify it's a 200 OK
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Join_ShouldReturnBadRequest_WhenFull()
    {
        // Arrange
        var request = new JoinTournamentRequest(123, "Walker1");
        _serviceMock.Setup(s => s.JoinAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ServiceResult(false, Error: "Tournament is full"));

        // Act
        var result = await _controller.Join(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var error = Assert.IsType<ErrorResponse>(badRequest.Value);
        Assert.Equal("Tournament is full", error.Error);
    }
}
