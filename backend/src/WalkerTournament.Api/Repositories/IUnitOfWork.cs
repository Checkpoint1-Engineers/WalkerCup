namespace WalkerTournament.Api.Repositories;

/// <summary>
/// Unit of Work pattern to manage transactions across repositories.
/// This keeps transaction logic in the repository layer, not in services.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    ITournamentRepository Tournaments { get; }
    ITournamentMemberRepository TournamentMembers { get; }
    IMatchRepository Matches { get; }
    
    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Saves all changes made in this unit of work.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
