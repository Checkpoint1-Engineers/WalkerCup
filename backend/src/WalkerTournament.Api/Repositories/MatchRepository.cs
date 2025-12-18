using Microsoft.EntityFrameworkCore;
using WalkerTournament.Api.Data;
using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly AppDbContext _context;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Matches
            .Include(m => m.ParticipantA)
            .Include(m => m.ParticipantB)
            .Include(m => m.Winner)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<Match?> GetByIdWithTournamentAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Matches
            .Include(m => m.Tournament)
            .Include(m => m.ParticipantA)
            .Include(m => m.ParticipantB)
            .Include(m => m.Winner)
            .Include(m => m.NextMatch)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<IReadOnlyList<Match>> ListByTournamentAsync(Guid tournamentId, CancellationToken ct = default)
    {
        return await _context.Matches
            .Include(m => m.ParticipantA)
            .Include(m => m.ParticipantB)
            .Include(m => m.Winner)
            .Where(m => m.TournamentId == tournamentId)
            .OrderBy(m => m.RoundNumber)
            .ThenBy(m => m.MatchOrder)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Match>> ListByRoundAsync(Guid tournamentId, int roundNumber, CancellationToken ct = default)
    {
        return await _context.Matches
            .Include(m => m.ParticipantA)
            .Include(m => m.ParticipantB)
            .Include(m => m.Winner)
            .Where(m => m.TournamentId == tournamentId && m.RoundNumber == roundNumber)
            .OrderBy(m => m.MatchOrder)
            .ToListAsync(ct);
    }

    public async Task AddRangeAsync(IEnumerable<Match> matches, CancellationToken ct = default)
    {
        await _context.Matches.AddRangeAsync(matches, ct);
    }

    public async Task UpdateAsync(Match match, CancellationToken ct = default)
    {
        _context.Matches.Update(match);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
