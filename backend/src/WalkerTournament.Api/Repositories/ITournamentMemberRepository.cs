using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public interface ITournamentMemberRepository
{
    Task<TournamentMember?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid tournamentId, int walkerId, string walkerName, CancellationToken ct = default);
    Task<IReadOnlyList<TournamentMember>> ListByTournamentAsync(Guid tournamentId, CancellationToken ct = default);
    Task AddAsync(TournamentMember member, CancellationToken ct = default);
    Task UpdateAsync(TournamentMember member, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
