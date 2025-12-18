namespace WalkerTournament.Api.Entities;

public class TournamentMember
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public int WalkerId { get; set; }
    public string WalkerName { get; set; } = null!;
    public DateTime JoinedAt { get; set; }
    public DateTime? EliminatedAt { get; set; }
    public int XpEarned { get; set; }

    // Navigation properties
    public Tournament Tournament { get; set; } = null!;
    public ICollection<Match> MatchesAsParticipantA { get; set; } = new List<Match>();
    public ICollection<Match> MatchesAsParticipantB { get; set; } = new List<Match>();
    public ICollection<Match> MatchesWon { get; set; } = new List<Match>();
}
