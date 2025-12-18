using System.ComponentModel.DataAnnotations;

namespace WalkerTournament.Api.DTOs;

public record SetWinnerRequest([Required] Guid WinnerId);

public record MatchResponse(
    Guid Id,
    int RoundNumber,
    int MatchOrder,
    TournamentMemberResponse? ParticipantA,
    TournamentMemberResponse? ParticipantB,
    TournamentMemberResponse? Winner,
    Guid? NextMatchId
);
