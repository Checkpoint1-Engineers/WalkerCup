using System.ComponentModel.DataAnnotations;

namespace WalkerTournament.Api.DTOs;

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password
);

public record LoginResponse(string Token, DateTime ExpiresAt);
