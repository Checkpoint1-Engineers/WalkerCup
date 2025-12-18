namespace WalkerTournament.Api.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "TeamAlan";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Tournament> CreatedTournaments { get; set; } = new List<Tournament>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
