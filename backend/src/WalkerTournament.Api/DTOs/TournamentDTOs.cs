using System.ComponentModel.DataAnnotations;
using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.DTOs;

public record CreateTournamentRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(1000)] string? Description,
    [Required] DateTime JoinDeadline,
    [Required][Range(1, 10000)] int XpPerWin,
    [Required][Range(2, 1000)] int MaxParticipants,
    [MaxLength(500)] string? ImageUrl
);

public record ExtendDeadlineRequest([Required] DateTime NewDeadline);

public record JoinTournamentRequest(
    [Required] int WalkerId,
    [Required][MaxLength(100)] string WalkerName,
    [Required][EmailAddress] string Email
);

public record TournamentResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTime JoinDeadline,
    TournamentStatus Status,
    int XpPerWin,
    int MaxParticipants,
    string? ImageUrl,
    string CreatedByEmail,
    DateTime CreatedAt,
    int MemberCount,
    IEnumerable<TournamentMemberResponse>? Members,
    IEnumerable<MatchResponse>? Matches
);

public record TournamentMemberResponse(
    Guid Id,
    int WalkerId,
    string WalkerName,
    DateTime JoinedAt,
    DateTime? EliminatedAt,
    int XpEarned
);

public record TournamentListResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTime JoinDeadline,
    TournamentStatus Status,
    int XpPerWin,
    int MaxParticipants,
    string? ImageUrl,
    int MemberCount,
    DateTime CreatedAt
);
