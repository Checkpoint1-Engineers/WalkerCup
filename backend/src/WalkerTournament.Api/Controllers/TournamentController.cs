using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WalkerTournament.Api.DTOs;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Api.Controllers;

[ApiController]
[Route("api/tournaments")]
public class TournamentController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        return Guid.Parse(userIdClaim!.Value);
    }

    /// <summary>
    /// Create a new tournament (TeamAlan only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "TeamAlan")]
    public async Task<IActionResult> Create([FromBody] CreateTournamentRequest request, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _tournamentService.CreateAsync(
            request.Name,
            request.Description,
            request.JoinDeadline,
            request.XpPerWin,
            request.MaxParticipants,
            request.ImageUrl,
            userId,
            ct);

        if (!result.Success)
        {
            return BadRequest(new ErrorResponse(result.Error!));
        }

        var tournament = result.Data!;
        return CreatedAtAction(nameof(GetById), new { id = tournament.Id }, new TournamentListResponse(
            tournament.Id,
            tournament.Name,
            tournament.Description,
            tournament.JoinDeadline,
            tournament.Status,
            tournament.XpPerWin,
            tournament.MaxParticipants,
            tournament.ImageUrl,
            0, // MemberCount is 0 for new tournament
            tournament.CreatedAt
        ));
    }

    /// <summary>
    /// List all tournaments (TeamAlan only)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var result = await _tournamentService.ListAsync(ct);
        var tournaments = result.Data!.Select(t => new TournamentListResponse(
            t.Id,
            t.Name,
            t.Description,
            t.JoinDeadline,
            t.Status,
            t.XpPerWin,
            t.MaxParticipants,
            t.ImageUrl,
            t.Members.Count,
            t.CreatedAt
        ));
        return Ok(tournaments);
    }

    /// <summary>
    /// Get tournament details
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _tournamentService.GetByIdAsync(id, ct);
        if (!result.Success)
        {
            return NotFound(new ErrorResponse(result.Error!));
        }

        var t = result.Data!;
        var response = new TournamentResponse(
            t.Id,
            t.Name,
            t.Description,
            t.JoinDeadline,
            t.Status,
            t.XpPerWin,
            t.MaxParticipants,
            t.ImageUrl,
            t.CreatedBy.Email,
            t.CreatedAt,
            t.Members.Count,
            t.Members.Select(m => new TournamentMemberResponse(
                m.Id, m.WalkerId, m.WalkerName, m.JoinedAt, m.EliminatedAt, m.XpEarned)),
            t.Matches.Select(m => new MatchResponse(
                m.Id,
                m.RoundNumber,
                m.MatchOrder,
                m.ParticipantA != null ? new TournamentMemberResponse(
                    m.ParticipantA.Id, m.ParticipantA.WalkerId, m.ParticipantA.WalkerName,
                    m.ParticipantA.JoinedAt, m.ParticipantA.EliminatedAt, m.ParticipantA.XpEarned) : null,
                m.ParticipantB != null ? new TournamentMemberResponse(
                    m.ParticipantB.Id, m.ParticipantB.WalkerId, m.ParticipantB.WalkerName,
                    m.ParticipantB.JoinedAt, m.ParticipantB.EliminatedAt, m.ParticipantB.XpEarned) : null,
                m.Winner != null ? new TournamentMemberResponse(
                    m.Winner.Id, m.Winner.WalkerId, m.Winner.WalkerName,
                    m.Winner.JoinedAt, m.Winner.EliminatedAt, m.Winner.XpEarned) : null,
                m.NextMatchId
            ))
        );

        return Ok(response);
    }

    /// <summary>
    /// Open tournament for registration (TeamAlan only)
    /// </summary>
    [HttpPost("{id:guid}/open")]
    [Authorize(Roles = "TeamAlan")]
    public async Task<IActionResult> Open(Guid id, CancellationToken ct)
    {
        var result = await _tournamentService.OpenAsync(id, ct);
        if (!result.Success)
        {
            if (result.Error == "Tournament not found")
            {
                return NotFound(new ErrorResponse(result.Error!));
            }
            return BadRequest(new ErrorResponse(result.Error!));
        }
        return Ok(new { message = "Tournament opened successfully" });
    }

    /// <summary>
    /// Extend registration deadline (TeamAlan only)
    /// </summary>
    [HttpPost("{id:guid}/extend")]
    [Authorize(Roles = "TeamAlan")]
    public async Task<IActionResult> ExtendDeadline(Guid id, [FromBody] ExtendDeadlineRequest request, CancellationToken ct)
    {
        var result = await _tournamentService.ExtendDeadlineAsync(id, request.NewDeadline, ct);
        if (!result.Success)
        {
            if (result.Error == "Tournament not found")
            {
                return NotFound(new ErrorResponse(result.Error!));
            }
            return BadRequest(new ErrorResponse(result.Error!));
        }
        return Ok(new { message = "Deadline extended successfully" });
    }

    /// <summary>
    /// Join tournament as walker (anonymous, rate limited)
    /// </summary>
    [HttpPost("{id:guid}/join")]
    [EnableRateLimiting("join")]
    [AllowAnonymous]
    public async Task<IActionResult> Join(Guid id, [FromBody] JoinTournamentRequest request, CancellationToken ct)
    {
        var result = await _tournamentService.JoinAsync(id, request.WalkerId, request.WalkerName, request.Email, ct);
        if (!result.Success)
        {
            if (result.Error == "Tournament not found")
            {
                return NotFound(new ErrorResponse(result.Error!));
            }
            if (result.Error == "Tournament is full" || result.Error!.Contains("already registered"))
            {
                return Conflict(new ErrorResponse(result.Error!));
            }
            return BadRequest(new ErrorResponse(result.Error!));
        }
        return Ok(new { message = "Successfully joined tournament" });
    }

    /// <summary>
    /// Remove a member from the tournament (TeamAlan only)
    /// </summary>
    [HttpDelete("{id:guid}/members/{walkerId:int}")]
    [Authorize(Roles = "TeamAlan")]
    public async Task<IActionResult> RemoveMember(Guid id, int walkerId, CancellationToken ct)
    {
        var result = await _tournamentService.RemoveMemberAsync(id, walkerId, ct);
        if (!result.Success)
        {
            if (result.Error == "Tournament not found" || result.Error == "Member not found in this tournament")
            {
                return NotFound(new ErrorResponse(result.Error!));
            }
            return BadRequest(new ErrorResponse(result.Error!));
        }
        return Ok(new { message = "Member removed successfully" });
    }


    /// <summary>
    /// Lock tournament for drawing (TeamAlan only)
    /// </summary>
    [HttpPost("{id:guid}/lock")]
    [Authorize(Roles = "TeamAlan")]
    public async Task<IActionResult> Lock(Guid id, CancellationToken ct)
    {
        var result = await _tournamentService.LockAsync(id, ct);
        if (!result.Success)
        {
            if (result.Error == "Tournament not found")
            {
                return NotFound(new ErrorResponse(result.Error!));
            }
            return BadRequest(new ErrorResponse(result.Error!));
        }
        return Ok(new { message = "Tournament locked successfully" });
    }

    /// <summary>
    /// Generate bracket draw (TeamAlan only)
    /// </summary>
    [HttpPost("{id:guid}/draw")]
    [Authorize(Roles = "TeamAlan")]
    public async Task<IActionResult> Draw(Guid id, CancellationToken ct)
    {
        var result = await _tournamentService.DrawAsync(id, ct);
        if (!result.Success)
        {
            if (result.Error == "Tournament not found")
            {
                return NotFound(new ErrorResponse(result.Error!));
            }
            return BadRequest(new ErrorResponse(result.Error!));
        }
        return Ok(new { message = "Bracket generated successfully" });
    }
}
