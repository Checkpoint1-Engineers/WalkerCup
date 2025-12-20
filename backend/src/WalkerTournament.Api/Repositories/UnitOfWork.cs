using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WalkerTournament.Api.Data;

namespace WalkerTournament.Api.Repositories;

/// <summary>
/// Unit of Work implementation that manages transactions at the repository layer.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    
    public ITournamentRepository Tournaments { get; }
    public ITournamentMemberRepository TournamentMembers { get; }
    public IMatchRepository Matches { get; }

    public UnitOfWork(
        AppDbContext context,
        ITournamentRepository tournaments,
        ITournamentMemberRepository tournamentMembers,
        IMatchRepository matches)
    {
        _context = context;
        Tournaments = tournaments;
        TournamentMembers = tournamentMembers;
        Matches = matches;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
