using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkerTournament.Api.DTOs;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Api.Controllers;

[ApiController]
[Route("api/matches")]
[Authorize(Roles = "TeamAlan")]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        return Guid.Parse(userIdClaim!.Value);
    }

    /// <summary>
    /// Set winner of a match (TeamAlan only)
    /// </summary>
    [HttpPost("{id:guid}/winner")]
    public async Task<IActionResult> SetWinner(Guid id, [FromBody] SetWinnerRequest request, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var result = await _matchService.SetWinnerAsync(id, request.WinnerId, userId, ct);

        if (!result.Success)
        {
            if (result.Error == "Match not found")
            {
                return NotFound(new ErrorResponse(result.Error));
            }
            if (result.Error!.Contains("modified by another user", StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new ErrorResponse(result.Error));
            }
            return BadRequest(new ErrorResponse(result.Error));
        }

        return Ok(new { message = "Winner set successfully" });
    }

}
