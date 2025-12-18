namespace WalkerTournament.Api.Services;

public interface IAuditLogService
{
    Task LogAsync(Guid actorUserId, string actionType, string entityType, Guid entityId, CancellationToken ct = default, string? metadata = null);
}
