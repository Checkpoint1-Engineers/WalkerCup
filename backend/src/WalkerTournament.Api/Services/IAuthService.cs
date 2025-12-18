namespace WalkerTournament.Api.Services;

public record AuthResult(bool Success, string? Token = null, string? Error = null);

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password, CancellationToken ct = default);
}
