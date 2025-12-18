using Microsoft.EntityFrameworkCore;
using WalkerTournament.Api.Data;
using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public class TournamentMemberRepository : ITournamentMemberRepository
{
    private readonly AppDbContext _context;

    public TournamentMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TournamentMember?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.TournamentMembers
            .Include(m => m.Tournament)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<bool> ExistsAsync(Guid tournamentId, int walkerId, string walkerName, CancellationToken ct = default)
    {
        return await _context.TournamentMembers
            .AnyAsync(m => m.TournamentId == tournamentId 
                        && m.WalkerId == walkerId 
                        && m.WalkerName == walkerName, ct);
    }

    public async Task<IReadOnlyList<TournamentMember>> ListByTournamentAsync(Guid tournamentId, CancellationToken ct = default)
    {
        return await _context.TournamentMembers
            .Where(m => m.TournamentId == tournamentId)
            .OrderBy(m => m.JoinedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(TournamentMember member, CancellationToken ct = default)
    {
        await _context.TournamentMembers.AddAsync(member, ct);
    }

    public async Task UpdateAsync(TournamentMember member, CancellationToken ct = default)
    {
        _context.TournamentMembers.Update(member);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
