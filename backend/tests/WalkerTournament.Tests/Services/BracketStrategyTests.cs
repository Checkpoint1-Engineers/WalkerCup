using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Tests.Services;

public class BracketStrategyTests
{
    private readonly SingleEliminationStrategy _strategy;

    public BracketStrategyTests()
    {
        _strategy = new SingleEliminationStrategy();
    }

    [Fact]
    public void GenerateBracket_ShouldCreateCorrectNumberOfMatches_ForPowerOf2()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        var participants = new List<TournamentMember>();
        for (int i = 0; i < 8; i++) participants.Add(new TournamentMember { Id = Guid.NewGuid() });

        // Act
        var matches = _strategy.GenerateBracket(tournamentId, participants);

        // Assert
        // 8 participants = 7 matches (4 + 2 + 1)
        Assert.Equal(7, matches.Count);
        
        // Verify Round 1
        Assert.Equal(4, matches.Count(m => m.RoundNumber == 1));
        
        // Verify Round 2
        Assert.Equal(2, matches.Count(m => m.RoundNumber == 2));
        
        // Verify Final
        Assert.Equal(1, matches.Count(m => m.RoundNumber == 3));
    }

    [Fact]
    public void GenerateBracket_ShouldHandleByes_ForNonPowerOf2()
    {
        // Arrange
        var tournamentId = Guid.NewGuid();
        var participants = new List<TournamentMember>();
        for (int i = 0; i < 6; i++) participants.Add(new TournamentMember { Id = Guid.NewGuid() });

        // Act
        var matches = _strategy.GenerateBracket(tournamentId, participants);

        // Assert
        // 6 participants -> Next power of 2 is 8
        // 8 slots -> 8 - 6 = 2 byes
        // Matches should still be 7 total slots (structure for 8), but some auto-resolved?
        // Let's check logic: strategy creates full tree for next power of 2
        // Total rounds = log2(8) = 3
        // Total matches = 7
        Assert.Equal(7, matches.Count);

        // Check for auto-winners (Byes) in Round 1
        var round1 = matches.Where(m => m.RoundNumber == 1).ToList();
        var byeMatches = round1.Where(m => m.WinnerId != null).ToList();
        
        // Should have 2 byes
        Assert.Equal(2, byeMatches.Count);
    }
}
