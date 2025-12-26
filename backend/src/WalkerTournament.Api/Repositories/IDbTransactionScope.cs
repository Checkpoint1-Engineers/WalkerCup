namespace WalkerTournament.Api.Repositories;

/// <summary>
/// Manages database transaction scope for atomic operations across multiple repositories.
/// </summary>
public interface IDbTransactionScope : IDisposable
{
    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Commits the current transaction, persisting all changes.
    /// </summary>
    Task CommitAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Rolls back the current transaction, discarding all changes.
    /// </summary>
    Task RollbackAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
