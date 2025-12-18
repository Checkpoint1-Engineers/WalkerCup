using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WalkerTournament.Api.DTOs;
using WalkerTournament.Api.Services;

namespace WalkerTournament.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login for TeamAlan administrators
    /// </summary>
    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password, ct);

        if (!result.Success)
        {
            return Unauthorized(new ErrorResponse(result.Error!));
        }

        return Ok(new LoginResponse(result.Token!, DateTime.UtcNow.AddHours(1)));
    }
}
