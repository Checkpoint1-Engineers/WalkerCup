using Microsoft.EntityFrameworkCore;
using WalkerTournament.Api.Entities;
using WalkerTournament.Api.Utilities;
using Serilog;
using System.Text.Json;

namespace WalkerTournament.Api.Data;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly Random _random = new();

    public DataSeeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (!_context.Database.IsSqlite()) return;

        await _context.Database.MigrateAsync();

        var adminUser = await SeedAdminUserAsync();
        var users = await SeedMockUsersAsync();
        
        // Ensure we combine admin + mock users for participants
        var allUsers = new List<User> { adminUser };
        allUsers.AddRange(users);

        await SeedComplexTournamentsAsync(adminUser, allUsers);
    }

    private async Task<User> SeedAdminUserAsync()
    {
        var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "admin@teamalan.com");
        if (adminUser == null)
        {
            adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@teamalan.com",
                PasswordHash = _passwordHasher.Hash("Admin123!"),
                Role = "TeamAlan",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();
            Log.Information("Seeded default admin user");
        }
        return adminUser;
    }

    private async Task<List<User>> SeedMockUsersAsync()
    {
        if (await _context.Users.CountAsync() > 5) 
        {
            return await _context.Users.Where(u => u.Email != "admin@teamalan.com").ToListAsync();
        }

        var prefixes = new[] { "Neon", "Cyber", "Dark", "Light", "Iron", "Steel", "Glitch", "Null", "Echo", "Flux" };
        var suffixes = new[] { "Walker", "Ninja", "Ghost", "Runner", "Stryker", "Zero", "One", "Blade", "Soul", "Mind" };
        
        var users = new List<User>();
        for (int i = 0; i < 50; i++)
        {
            var name = $"{prefixes[_random.Next(prefixes.Length)]}{suffixes[_random.Next(suffixes.Length)]}{_random.Next(100, 999)}";
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = $"{name.ToLower()}@walker.com",
                PasswordHash = _passwordHasher.Hash("Password123!"),
                Role = "Walker",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();
        Log.Information($"Seeded {users.Count} mock users");
        return users;
    }

    private async Task SeedComplexTournamentsAsync(User admin, List<User> allUsers)
    {
        if (await _context.Tournaments.AnyAsync()) return;

        var tournaments = new List<Tournament>();

        // 1. OPEN: "Neon Qualifiers" (Almost Full)
        var neon = CreateTournament(admin.Id, "Neon Qualifiers", "High-speed tactical combat.", TournamentStatus.Open, 64, 150, "https://images.unsplash.com/photo-1555680202-c86f0e12f086?q=80&w=2670&auto=format&fit=crop");
        await AddParticipants(neon, allUsers, 58); // Almost full
        tournaments.Add(neon);

        // 2. OPEN: "Iron Gauntlet" (Empty)
        var iron = CreateTournament(admin.Id, "Iron Gauntlet", "Survival of the fittest.", TournamentStatus.Open, 32, 100, "https://images.unsplash.com/photo-1542751371-adc38448a05e?q=80&w=2670&auto=format&fit=crop");
        tournaments.Add(iron); // No participants

        // 3. IN PROGRESS: "Midnight Championship" (Round 2 Active)
        var midnight = CreateTournament(admin.Id, "Midnight Championship", "The elite midnight league.", TournamentStatus.InProgress, 16, 500, "https://images.unsplash.com/photo-1534423861386-85a16f5d13fd?q=80&w=2670&auto=format&fit=crop");
        var midnightMembers = await AddParticipants(midnight, allUsers, 16);
        GenerateBracket(midnight, midnightMembers, roundsToComplete: 1); // Complete Round 1
        tournaments.Add(midnight);

        // 4. COMPLETED: "Walker Prime 2024"
        var prime = CreateTournament(admin.Id, "Walker Prime 2024", "Legacy tournament.", TournamentStatus.Completed, 8, 1000, "https://images.unsplash.com/photo-1623934199716-dc28888775a9?q=80&w=2670&auto=format&fit=crop");
        var primeMembers = await AddParticipants(prime, allUsers, 8);
        GenerateBracket(prime, primeMembers, roundsToComplete: 3); // Complete all rounds
        tournaments.Add(prime);

        _context.Tournaments.AddRange(tournaments);
        await _context.SaveChangesAsync();
        Log.Information("Seeded complex tournaments");
    }

    private Tournament CreateTournament(Guid creatorId, string name, string desc, TournamentStatus status, int max, int xp, string img)
    {
        return new Tournament
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = desc,
            Status = status,
            JoinDeadline = DateTime.UtcNow.AddDays(7),
            XpPerWin = xp,
            MaxParticipants = max,
            ImageUrl = img,
            CreatedByUserId = creatorId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private async Task<List<TournamentMember>> AddParticipants(Tournament t, List<User> users, int count)
    {
        var participants = new List<TournamentMember>();
        // Shuffle users
        var shuffled = users.OrderBy(x => _random.Next()).Take(count).ToList();

        foreach (var u in shuffled)
        {
            var member = new TournamentMember
            {
                Id = Guid.NewGuid(),
                TournamentId = t.Id,
                WalkerId = _random.Next(10000, 99999),
                WalkerName = u.Email.Split('@')[0], // Use email handle as name
                Email = u.Email,
                JoinedAt = DateTime.UtcNow,
                Tournament = t
            };
            t.Members.Add(member); // Add to nav property
            participants.Add(member);
        }
        return participants;
    }

    private void GenerateBracket(Tournament t, List<TournamentMember> members, int roundsToComplete)
    {
        // Simple single elimination bracket generation
        // Assumes power of 2 participants for simplicity in seed
        int totalRounds = (int)Math.Log2(members.Count);
        var matches = new List<Match>();
        var currentRoundMatches = new List<Match>();
        
        // Round 1
        for (int i = 0; i < members.Count / 2; i++)
        {
            currentRoundMatches.Add(new Match
            {
                Id = Guid.NewGuid(),
                TournamentId = t.Id,
                RoundNumber = 1,
                MatchOrder = i,
                ParticipantAId = members[i * 2].Id,
                ParticipantA = members[i * 2],
                ParticipantBId = members[i * 2 + 1].Id,
                ParticipantB = members[i * 2 + 1],
                Tournament = t
            });
        }
        matches.AddRange(currentRoundMatches);

        // Subsequent Rounds
        for (int round = 2; round <= totalRounds; round++)
        {
            var nextRoundMatches = new List<Match>();
            for (int i = 0; i < currentRoundMatches.Count / 2; i++)
            {
                var nextMatch = new Match
                {
                    Id = Guid.NewGuid(),
                    TournamentId = t.Id,
                    RoundNumber = round,
                    MatchOrder = i,
                    Tournament = t
                };
                
                // Link previous matches
                var matchA = currentRoundMatches[i * 2];
                var matchB = currentRoundMatches[i * 2 + 1];
                
                matchA.NextMatchId = nextMatch.Id;
                matchA.NextMatch = nextMatch;
                matchA.IsParticipantASlotInNextMatch = true;

                matchB.NextMatchId = nextMatch.Id;
                matchB.NextMatch = nextMatch;
                matchB.IsParticipantASlotInNextMatch = false;

                nextRoundMatches.Add(nextMatch);
            }
            matches.AddRange(nextRoundMatches);
            currentRoundMatches = nextRoundMatches;
        }

        // Simulate Results
        foreach (var m in matches.Where(m => m.RoundNumber <= roundsToComplete))
        {
            if (m.ParticipantAId != null && m.ParticipantBId != null)
            {
                // Determine winner
                var winner = _random.Next(2) == 0 ? m.ParticipantA : m.ParticipantB;
                m.WinnerId = winner!.Id;
                m.Winner = winner;

                // Propagate to next match
                if (m.NextMatch != null)
                {
                    if (m.IsParticipantASlotInNextMatch)
                    {
                        m.NextMatch.ParticipantAId = winner.Id;
                        m.NextMatch.ParticipantA = winner;
                    }
                    else
                    {
                        m.NextMatch.ParticipantBId = winner.Id;
                        m.NextMatch.ParticipantB = winner;
                    }
                }
            }
        }

        foreach(var m in matches) t.Matches.Add(m);
    }
}
