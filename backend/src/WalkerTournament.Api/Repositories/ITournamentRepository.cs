using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Repositories;

public interface ITournamentRepository
{
    Task<Tournament?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Tournament?> GetByIdWithMembersAsync(Guid id, CancellationToken ct = default);
    Task<Tournament?> GetByIdWithMatchesAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Tournament>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Tournament tournament, CancellationToken ct = default);
    Task UpdateAsync(Tournament tournament, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
