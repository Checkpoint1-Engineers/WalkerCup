using Microsoft.EntityFrameworkCore;
using WalkerTournament.Api.Data;
using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public class TournamentRepository : ITournamentRepository
{
    private readonly AppDbContext _context;

    public TournamentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tournament?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Tournaments
            .Include(t => t.CreatedBy)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<Tournament?> GetByIdWithMembersAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Tournaments
            .Include(t => t.CreatedBy)
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<Tournament?> GetByIdWithMatchesAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Tournaments
            .Include(t => t.CreatedBy)
            .Include(t => t.Members)
            .Include(t => t.Matches)
                .ThenInclude(m => m.ParticipantA)
            .Include(t => t.Matches)
                .ThenInclude(m => m.ParticipantB)
            .Include(t => t.Matches)
                .ThenInclude(m => m.Winner)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<IReadOnlyList<Tournament>> ListAsync(CancellationToken ct = default)
    {
        return await _context.Tournaments
            .Include(t => t.CreatedBy)
            .Include(t => t.Members)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Tournament tournament, CancellationToken ct = default)
    {
        await _context.Tournaments.AddAsync(tournament, ct);
    }

    public async Task UpdateAsync(Tournament tournament, CancellationToken ct = default)
    {
        _context.Tournaments.Update(tournament);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
