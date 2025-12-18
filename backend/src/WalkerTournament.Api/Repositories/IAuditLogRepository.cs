using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public interface IAuditLogRepository
{
    Task<IReadOnlyList<AuditLog>> ListByEntityAsync(string entityType, Guid entityId, CancellationToken ct = default);
    Task AddAsync(AuditLog auditLog, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
