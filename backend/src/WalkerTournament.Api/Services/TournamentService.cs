using WalkerTournament.Api.Data;
using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Repositories;

namespace WalkerTournament.Api.Services;

public class TournamentService : ITournamentService
{
    private readonly AppDbContext _context;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ITournamentMemberRepository _memberRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IBracketStrategy _bracketStrategy;
    private readonly IAuditLogService _auditLogService;

    public TournamentService(
        AppDbContext context,
        ITournamentRepository tournamentRepository,
        ITournamentMemberRepository memberRepository,
        IMatchRepository matchRepository,
        IBracketStrategy bracketStrategy,
        IAuditLogService auditLogService)
    {
        _context = context;
        _tournamentRepository = tournamentRepository;
        _memberRepository = memberRepository;
        _matchRepository = matchRepository;
        _bracketStrategy = bracketStrategy;
        _auditLogService = auditLogService;
    }

    public async Task<ServiceResult<Tournament>> CreateAsync(
        string name, string? description, DateTime joinDeadline, int xpPerWin, int maxParticipants, string? imageUrl, Guid createdByUserId, CancellationToken ct = default)
    {
        var tournament = new Tournament
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            JoinDeadline = joinDeadline,
            XpPerWin = xpPerWin,
            MaxParticipants = maxParticipants,
            ImageUrl = imageUrl,
            Status = TournamentStatus.Draft,
            CreatedByUserId = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _tournamentRepository.AddAsync(tournament, ct);
        await _tournamentRepository.SaveChangesAsync(ct);

        await _auditLogService.LogAsync(createdByUserId, "Create", "Tournament", tournament.Id, ct);

        return new ServiceResult<Tournament>(true, Data: tournament);
    }

    public async Task<ServiceResult<Tournament>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var tournament = await _tournamentRepository.GetByIdWithMatchesAsync(id, ct);
        if (tournament == null)
        {
            return new ServiceResult<Tournament>(false, Error: "Tournament not found");
        }
        return new ServiceResult<Tournament>(true, Data: tournament);
    }

    public async Task<ServiceResult<IReadOnlyList<Tournament>>> ListAsync(CancellationToken ct = default)
    {
        var tournaments = await _tournamentRepository.ListAsync(ct);
        return new ServiceResult<IReadOnlyList<Tournament>>(true, Data: tournaments);
    }

    public async Task<ServiceResult> OpenAsync(Guid tournamentId, CancellationToken ct = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId, ct);
        if (tournament == null)
        {
            return new ServiceResult(false, Error: "Tournament not found");
        }

        if (tournament.Status != TournamentStatus.Draft)
        {
            return new ServiceResult(false, Error: "Tournament must be in Draft status to open");
        }

        tournament.Status = TournamentStatus.Open;
        tournament.UpdatedAt = DateTime.UtcNow;

        await _tournamentRepository.UpdateAsync(tournament, ct);
        await _tournamentRepository.SaveChangesAsync(ct);

        await _auditLogService.LogAsync(tournament.CreatedByUserId, "Open", "Tournament", tournament.Id, ct);

        return new ServiceResult(true);
    }

    public async Task<ServiceResult> ExtendDeadlineAsync(Guid tournamentId, DateTime newDeadline, CancellationToken ct = default)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId, ct);
        if (tournament == null)
        {
            return new ServiceResult(false, Error: "Tournament not found");
        }

        if (tournament.Status != TournamentStatus.Open)
        {
            return new ServiceResult(false, Error: "Can only extend deadline for Open tournaments");
        }

        if (newDeadline <= tournament.JoinDeadline)
        {
            return new ServiceResult(false, Error: "New deadline must be later than current deadline");
        }

        tournament.JoinDeadline = newDeadline;
        tournament.UpdatedAt = DateTime.UtcNow;

        await _tournamentRepository.UpdateAsync(tournament, ct);
        await _tournamentRepository.SaveChangesAsync(ct);

        await _auditLogService.LogAsync(tournament.CreatedByUserId, "ExtendDeadline", "Tournament", tournament.Id, ct);

        return new ServiceResult(true);
    }

    public async Task<ServiceResult> JoinAsync(Guid tournamentId, int walkerId, string walkerName, CancellationToken ct = default)
    {
        var tournament = await _tournamentRepository.GetByIdWithMembersAsync(tournamentId, ct);
        if (tournament == null)
        {
            return new ServiceResult(false, Error: "Tournament not found");
        }

        if (tournament.Status != TournamentStatus.Open)
        {
            return new ServiceResult(false, Error: "Tournament is not open for registration");
        }

        if (DateTime.UtcNow > tournament.JoinDeadline)
        {
            return new ServiceResult(false, Error: "Registration deadline has passed");
        }

        // Check for max participants
        if (tournament.Members.Count >= tournament.MaxParticipants)
        {
            return new ServiceResult(false, Error: "Tournament is full");
        }

        // Check for duplicate (WalkerId + WalkerName combination)
        var exists = await _memberRepository.ExistsAsync(tournamentId, walkerId, walkerName, ct);
        if (exists)
        {
            return new ServiceResult(false, Error: "This walker is already registered for this tournament");
        }

        var member = new TournamentMember
        {
            Id = Guid.NewGuid(),
            TournamentId = tournamentId,
            WalkerId = walkerId,
            WalkerName = walkerName,
            JoinedAt = DateTime.UtcNow,
            XpEarned = 0
        };

        await _memberRepository.AddAsync(member, ct);
        await _memberRepository.SaveChangesAsync(ct);

        return new ServiceResult(true);
    }

    public async Task<ServiceResult> LockAsync(Guid tournamentId, CancellationToken ct = default)
    {
        var tournament = await _tournamentRepository.GetByIdWithMembersAsync(tournamentId, ct);
        if (tournament == null)
        {
            return new ServiceResult(false, Error: "Tournament not found");
        }

        if (tournament.Status != TournamentStatus.Open)
        {
            return new ServiceResult(false, Error: "Tournament must be in Open status to lock");
        }

        if (tournament.Members.Count < 2)
        {
            return new ServiceResult(false, Error: "Tournament needs at least 2 participants");
        }

        tournament.Status = TournamentStatus.Locked;
        tournament.UpdatedAt = DateTime.UtcNow;

        await _tournamentRepository.UpdateAsync(tournament, ct);
        await _tournamentRepository.SaveChangesAsync(ct);

        await _auditLogService.LogAsync(tournament.CreatedByUserId, "Lock", "Tournament", tournament.Id, ct);

        return new ServiceResult(true);
    }

    public async Task<ServiceResult> DrawAsync(Guid tournamentId, CancellationToken ct = default)
    {
        var tournament = await _tournamentRepository.GetByIdWithMembersAsync(tournamentId, ct);
        if (tournament == null)
        {
            return new ServiceResult(false, Error: "Tournament not found");
        }

        if (tournament.Status != TournamentStatus.Locked)
        {
            return new ServiceResult(false, Error: "Tournament must be locked before drawing");
        }

        var participants = tournament.Members.ToList();
        var matches = _bracketStrategy.GenerateBracket(tournamentId, participants);

        await _matchRepository.AddRangeAsync(matches, ct);

        tournament.Status = TournamentStatus.InProgress;
        tournament.UpdatedAt = DateTime.UtcNow;

        await _tournamentRepository.UpdateAsync(tournament, ct);
        await _context.SaveChangesAsync(ct);

        await _auditLogService.LogAsync(tournament.CreatedByUserId, "Draw", "Tournament", tournament.Id, ct);

        return new ServiceResult(true);
    }
}
