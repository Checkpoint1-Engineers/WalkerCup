namespace WalkerTournament.Api.Services;

public interface IMatchService
{
    Task<ServiceResult> SetWinnerAsync(Guid matchId, Guid winnerId, Guid actorUserId, CancellationToken ct = default);
}
