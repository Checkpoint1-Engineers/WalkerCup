using Moq;
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
    private readonly Mock<IDbTransactionScope> _transactionScopeMock;
    private readonly TournamentService _service;

    public TournamentServiceTests()
    {
        _tournamentRepoMock = new Mock<ITournamentRepository>();
        _memberRepoMock = new Mock<ITournamentMemberRepository>();
        _matchRepoMock = new Mock<IMatchRepository>();
        _bracketStrategyMock = new Mock<IBracketStrategy>();
        _auditLogServiceMock = new Mock<IAuditLogService>();
        _transactionScopeMock = new Mock<IDbTransactionScope>();

        // Setup transaction scope to return successfully by default
        _transactionScopeMock.Setup(t => t.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _transactionScopeMock.Setup(t => t.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _transactionScopeMock.Setup(t => t.RollbackAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _transactionScopeMock.Setup(t => t.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _service = new TournamentService(
            _transactionScopeMock.Object,
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

        _memberRepoMock.Setup(r => r.GetCountByTournamentAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _memberRepoMock.Setup(r => r.ExistsAsync(tournamentId, It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.JoinAsync(tournamentId, 1, "Walker1");

        // Assert
        Assert.True(result.Success);
        _memberRepoMock.Verify(r => r.AddAsync(It.IsAny<TournamentMember>(), It.IsAny<CancellationToken>()), Times.Once);
        _transactionScopeMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionScopeMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
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
        _transactionScopeMock.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
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
        _transactionScopeMock.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
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

        _tournamentRepoMock.Setup(r => r.GetByIdAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tournament);

        // Use the repository count method instead of Members collection
        _memberRepoMock.Setup(r => r.GetCountByTournamentAsync(tournamentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2); // Tournament is full

        // Act
        var result = await _service.JoinAsync(tournamentId, 1, "NewWalker");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Tournament is full", result.Error);
        _transactionScopeMock.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

