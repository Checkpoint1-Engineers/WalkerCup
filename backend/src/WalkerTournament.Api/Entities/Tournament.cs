namespace WalkerTournament.Api.Entities;

public class Tournament
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime JoinDeadline { get; set; }
    public TournamentStatus Status { get; set; } = TournamentStatus.Draft;
    public int XpPerWin { get; set; }
    public int MaxParticipants { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User CreatedBy { get; set; } = null!;
    public ICollection<TournamentMember> Members { get; set; } = new List<TournamentMember>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
