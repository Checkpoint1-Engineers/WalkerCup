namespace WalkerTournament.Api.Entities;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid ActorUserId { get; set; }
    public string ActionType { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public User ActorUser { get; set; } = null!;
}
