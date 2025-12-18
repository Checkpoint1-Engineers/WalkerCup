using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Services;

public record ServiceResult(bool Success, string? Error = null);
public record ServiceResult<T>(bool Success, T? Data = default, string? Error = null);

public interface ITournamentService
{
    Task<ServiceResult<Tournament>> CreateAsync(string name, string? description, DateTime joinDeadline, int xpPerWin, int maxParticipants, string? imageUrl, Guid createdByUserId, CancellationToken ct = default);
    Task<ServiceResult<Tournament>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ServiceResult<IReadOnlyList<Tournament>>> ListAsync(CancellationToken ct = default);
    Task<ServiceResult> OpenAsync(Guid tournamentId, CancellationToken ct = default);
    Task<ServiceResult> ExtendDeadlineAsync(Guid tournamentId, DateTime newDeadline, CancellationToken ct = default);
    Task<ServiceResult> JoinAsync(Guid tournamentId, int walkerId, string walkerName, CancellationToken ct = default);
    Task<ServiceResult> LockAsync(Guid tournamentId, CancellationToken ct = default);
    Task<ServiceResult> DrawAsync(Guid tournamentId, CancellationToken ct = default);
}
