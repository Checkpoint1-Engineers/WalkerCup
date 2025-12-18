using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Services;

public interface IBracketStrategy
{
    IReadOnlyList<Match> GenerateBracket(Guid tournamentId, IReadOnlyList<TournamentMember> participants);
}
