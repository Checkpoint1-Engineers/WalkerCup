using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Repositories;

namespace WalkerTournament.Api.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ILogger<AuditLogService> _logger;

    public AuditLogService(IAuditLogRepository auditLogRepository, ILogger<AuditLogService> logger)
    {
        _auditLogRepository = auditLogRepository;
        _logger = logger;
    }

    public async Task LogAsync(Guid actorUserId, string actionType, string entityType, Guid entityId, CancellationToken ct = default, string? metadata = null)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            ActorUserId = actorUserId,
            ActionType = actionType,
            EntityType = entityType,
            EntityId = entityId,
            Metadata = metadata,
            CreatedAt = DateTime.UtcNow
        };

        await _auditLogRepository.AddAsync(auditLog, ct);
        await _auditLogRepository.SaveChangesAsync(ct);

        _logger.LogInformation("Audit: User {UserId} performed {Action} on {EntityType} {EntityId}",
            actorUserId, actionType, entityType, entityId);
    }
}
