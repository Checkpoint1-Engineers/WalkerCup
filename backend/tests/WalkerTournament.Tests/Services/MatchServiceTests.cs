using Microsoft.EntityFrameworkCore;
using Moq;
using WalkerTournament.Api.Data;
using WalkerTournament.Api.Repositories;
using WalkerTournament.Api.Services;
using Entities = WalkerTournament.Api.Entities;
using TournamentStatus = WalkerTournament.Api.Entities.TournamentStatus;

namespace WalkerTournament.Tests.Services;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepoMock;
    private readonly Mock<ITournamentMemberRepository> _memberRepoMock;
    private readonly Mock<ITournamentRepository> _tournamentRepoMock;
    private readonly Mock<IAuditLogService> _logServiceMock;
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly MatchService _service;

    public MatchServiceTests()
    {
        _matchRepoMock = new Mock<IMatchRepository>();
        _memberRepoMock = new Mock<ITournamentMemberRepository>();
        _tournamentRepoMock = new Mock<ITournamentRepository>();
        _logServiceMock = new Mock<IAuditLogService>();
        _dbContextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

        _service = new MatchService(
            _dbContextMock.Object,
            _matchRepoMock.Object,
            _memberRepoMock.Object,
            _tournamentRepoMock.Object,
            _logServiceMock.Object
        );
    }

    [Fact]
    public async Task SetWinner_ShouldFail_WhenMatchNotFound()
    {
        // Arrange
        _matchRepoMock.Setup(r => r.GetByIdWithTournamentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Entities.Match?)null);

        // Act
        var result = await _service.SetWinnerAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Match not found", result.Error);
    }

    [Fact]
    public async Task SetWinner_ShouldFail_WhenTournamentNotInProgress()
    {
        // Arrange
        var match = new Entities.Match 
        { 
            Tournament = new Entities.Tournament { Status = TournamentStatus.Draft } 
        };
        _matchRepoMock.Setup(r => r.GetByIdWithTournamentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var result = await _service.SetWinnerAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Tournament is not in progress", result.Error);
    }

    [Fact]
    public async Task SetWinner_ShouldFail_WhenWinnerNotParticipant()
    {
        // Arrange
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();
        var match = new Entities.Match 
        { 
            ParticipantAId = p1, 
            ParticipantBId = p2,
            Tournament = new Entities.Tournament { Status = TournamentStatus.InProgress } 
        };
        _matchRepoMock.Setup(r => r.GetByIdWithTournamentAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var result = await _service.SetWinnerAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()); // Random new Guid as winner

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Winner must be one of the match participants", result.Error);
    }
}
