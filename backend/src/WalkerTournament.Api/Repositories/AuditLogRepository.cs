using Microsoft.EntityFrameworkCore;
using WalkerTournament.Api.Data;
using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AuditLog>> ListByEntityAsync(string entityType, Guid entityId, CancellationToken ct = default)
    {
        return await _context.AuditLogs
            .Include(a => a.ActorUser)
            .Where(a => a.EntityType == entityType && a.EntityId == entityId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(AuditLog auditLog, CancellationToken ct = default)
    {
        await _context.AuditLogs.AddAsync(auditLog, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
