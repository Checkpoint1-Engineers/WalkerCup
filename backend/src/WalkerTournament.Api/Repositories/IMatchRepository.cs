using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Match?> GetByIdWithTournamentAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Match>> ListByTournamentAsync(Guid tournamentId, CancellationToken ct = default);
    Task<IReadOnlyList<Match>> ListByRoundAsync(Guid tournamentId, int roundNumber, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Match> matches, CancellationToken ct = default);
    Task UpdateAsync(Match match, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
