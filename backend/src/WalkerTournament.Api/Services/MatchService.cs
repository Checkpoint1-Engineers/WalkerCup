using Microsoft.EntityFrameworkCore;
using WalkerTournament.Api.Data;
using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Repositories;

namespace WalkerTournament.Api.Services;

public class MatchService : IMatchService
{
    private readonly IDbTransactionScope _transactionScope;
    private readonly IMatchRepository _matchRepository;
    private readonly ITournamentMemberRepository _memberRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IAuditLogService _auditLogService;

    public MatchService(
        IDbTransactionScope transactionScope,
        IMatchRepository matchRepository,
        ITournamentMemberRepository memberRepository,
        ITournamentRepository tournamentRepository,
        IAuditLogService auditLogService)
    {
        _transactionScope = transactionScope;
        _matchRepository = matchRepository;
        _memberRepository = memberRepository;
        _tournamentRepository = tournamentRepository;
        _auditLogService = auditLogService;
    }

    public async Task<ServiceResult> SetWinnerAsync(Guid matchId, Guid winnerId, Guid actorUserId, CancellationToken ct = default)
    {
        // Use transaction scope for atomicity across winner setting, XP award, and advancement
        await _transactionScope.BeginTransactionAsync(ct);
        try
        {
            var match = await _matchRepository.GetByIdWithTournamentAsync(matchId, ct);
            if (match == null)
            {
                await _transactionScope.RollbackAsync(ct);
                return new ServiceResult(false, Error: "Match not found");
            }

            if (match.Tournament.Status != TournamentStatus.InProgress)
            {
                await _transactionScope.RollbackAsync(ct);
                return new ServiceResult(false, Error: "Tournament is not in progress");
            }

            if (match.WinnerId != null)
            {
                await _transactionScope.RollbackAsync(ct);
                return new ServiceResult(false, Error: "Winner already set for this match");
            }

            // Handle Bye matches - if only one participant exists, they are the only valid winner
            bool isByeMatch = match.ParticipantAId != null && match.ParticipantBId == null;
            if (isByeMatch)
            {
                if (winnerId != match.ParticipantAId)
                {
                    await _transactionScope.RollbackAsync(ct);
                    return new ServiceResult(false, Error: "Invalid winner for bye match");
                }
            }
            else
            {
                // Validate winner is one of the participants
                if (match.ParticipantAId != winnerId && match.ParticipantBId != winnerId)
                {
                    await _transactionScope.RollbackAsync(ct);
                    return new ServiceResult(false, Error: "Winner must be one of the match participants");
                }
            }

            // Set winner
            match.WinnerId = winnerId;

            // Award XP to winner
            var winner = await _memberRepository.GetByIdAsync(winnerId, ct);
            if (winner != null)
            {
                winner.XpEarned += match.Tournament.XpPerWin;
                await _memberRepository.UpdateAsync(winner, ct);
            }

            // Set eliminated time for loser
            if (!isByeMatch)
            {
                var loserId = match.ParticipantAId == winnerId ? match.ParticipantBId : match.ParticipantAId;
                if (loserId != null)
                {
                    var loser = await _memberRepository.GetByIdAsync(loserId.Value, ct);
                    if (loser != null)
                    {
                        loser.EliminatedAt = DateTime.UtcNow;
                        await _memberRepository.UpdateAsync(loser, ct);
                    }
                }
            }

            // Advance winner to next match
            if (match.NextMatchId != null)
            {
                var nextMatch = await _matchRepository.GetByIdAsync(match.NextMatchId.Value, ct);
                if (nextMatch != null)
                {
                    if (match.IsParticipantASlotInNextMatch)
                    {
                        nextMatch.ParticipantAId = winnerId;
                    }
                    else
                    {
                        nextMatch.ParticipantBId = winnerId;
                    }
                    nextMatch.RowVersion = Guid.NewGuid().ToByteArray();
                    await _matchRepository.UpdateAsync(nextMatch, ct);
                }
            }
            else
            {
                // This is the final match - tournament is complete!
                var tournament = await _tournamentRepository.GetByIdAsync(match.TournamentId, ct);
                if (tournament != null)
                {
                    tournament.Status = TournamentStatus.Completed;
                    tournament.UpdatedAt = DateTime.UtcNow;
                    await _tournamentRepository.UpdateAsync(tournament, ct);
                }
            }

            // Update RowVersion for concurrency check
            match.RowVersion = Guid.NewGuid().ToByteArray();
            await _matchRepository.UpdateAsync(match, ct);

            await _transactionScope.SaveChangesAsync(ct);
            await _transactionScope.CommitAsync(ct);

            await _auditLogService.LogAsync(actorUserId, "SetWinner", "Match", matchId, ct);

            return new ServiceResult(true);
        }
        catch (DbUpdateConcurrencyException)
        {
            await _transactionScope.RollbackAsync(ct);
            return new ServiceResult(false, Error: "Match was modified by another user. Please refresh and try again.");
        }
        catch (Exception)
        {
            await _transactionScope.RollbackAsync(ct);
            throw;
        }
    }
}
