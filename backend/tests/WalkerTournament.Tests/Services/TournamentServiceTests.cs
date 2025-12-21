using Moq;
using WalkerTournament.Api.Data;
using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Repositories;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Tests.Services;

public class TournamentServiceTests
{
    private readonly Mock<ITournamentRepository> _tournamentRepoMock;
    private readonly Mock<ITournamentMemberRepository> _memberRepoMock;
    private readonly Mock<IMatchRepository> _matchRepoMock;
    private readonly Mock<IBracketStrategy> _bracketStrategyMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly TournamentService _service;

    public TournamentServiceTests()
    {
        _tournamentRepoMock = new Mock<ITournamentRepository>();
        _memberRepoMock = new Mock<ITournamentMemberRepository>();
        _matchRepoMock = new Mock<IMatchRepository>();
        _bracketStrategyMock = new Mock<IBracketStrategy>();
        _auditLogServiceMock = new Mock<IAuditLogService>();
        _dbContextMock = new Mock<AppDbContext>(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());

        _service = new TournamentService(
            _dbContextMock.Object,
            _tournamentRepoMock.Object,
            _memberRepoMock.Object,
            _matchRepoMock.Object,
            _bracketStrategyMock.Object,
            _auditLogServiceMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTournament_WhenValidDataProvided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var name = "Test Tournament";
        var description = "Test Description";
        var deadline = DateTime.UtcNow.AddDays(7);
        var xp = 100;
        var maxParticipants = 32;
        string? imageUrl = null;

        // Act
        var result = await _service.CreateAsync(name, description, deadline, xp, maxParticipants, imageUrl, userId);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(name, result.Data.Name);
        Assert.Equal(maxParticipants, result.Data.MaxParticipants);
        _tournamentRepoMock.Verify(r => r.AddAsync(It.IsAny<Tournament>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task JoinAsync_ShouldReturnSuccess_WhenTournamentIsOpenAndWithinDeadline()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament
        {
            Id = tournamentId,
            Status = TournamentStatus.Open,
            JoinDeadline = DateTime.UtcNow.AddDays(1),
            Name = "Test Tournament",
            MaxParticipants = 10
        };

        _tournamentRepoMock.Setup(r => r.GetByIdAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tournament);

        _memberRepoMock.Setup(r => r.ExistsAsync(tournamentId, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.JoinAsync(tournamentId, 1, "Walker1");

        // Assert
        Assert.True(result.Success);
        _memberRepoMock.Verify(r => r.AddAsync(It.IsAny<TournamentMember>(), It.IsAny<CancellationToken>()), Times.Once);
        _memberRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task JoinAsync_ShouldFail_WhenTournamentDoesNotExist()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        _tournamentRepoMock.Setup(r => r.GetByIdAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tournament?)null);

        // Act
        var result = await _service.JoinAsync(tournamentId, 1, "Walker1");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Tournament not found", result.Error);
    }

    [Fact]
    public async Task JoinAsync_ShouldFail_WhenDeadlineExceeded()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament
        {
            Id = tournamentId,
            Status = TournamentStatus.Open,
            JoinDeadline = DateTime.UtcNow.AddDays(-1),
            Name = "Past Tournament"
        };

        _tournamentRepoMock.Setup(r => r.GetByIdAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tournament);

        // Act
        var result = await _service.JoinAsync(tournamentId, 1, "Walker1");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Registration deadline has passed", result.Error);
    }

    [Fact]
    public async Task JoinAsync_ShouldFail_WhenTournamentIsFull()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        var tournament = new Tournament
        {
            Id = tournamentId,
            Status = TournamentStatus.Open,
            JoinDeadline = DateTime.UtcNow.AddDays(1),
            Name = "Full Tournament",
            MaxParticipants = 2
        };
        
        // Simulate 2 existing members
        tournament.Members.Add(new TournamentMember { Id = Guid.NewGuid() });
        tournament.Members.Add(new TournamentMember { Id = Guid.NewGuid() });

        _tournamentRepoMock.Setup(r => r.GetByIdAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tournament);

        // Act
        var result = await _service.JoinAsync(tournamentId, 1, "NewWalker");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Tournament is full", result.Error);
    }
}
