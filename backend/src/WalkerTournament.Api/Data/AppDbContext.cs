using Microsoft.EntityFrameworkCore;
using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentMember> TournamentMembers => Set<TournamentMember>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
        });

        // Tournament configuration
        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasIndex(e => e.JoinDeadline);
            
            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedTournaments)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TournamentMember configuration
        modelBuilder.Entity<TournamentMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WalkerName).IsRequired().HasMaxLength(100);
            
            // Unique constraint on (TournamentId, WalkerId, WalkerName)
            entity.HasIndex(e => new { e.TournamentId, e.WalkerId, e.WalkerName }).IsUnique();
            
            entity.HasOne(e => e.Tournament)
                .WithMany(t => t.Members)
                .HasForeignKey(e => e.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Match configuration
        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Concurrency token
            entity.Property(e => e.RowVersion).IsConcurrencyToken();
            
            entity.HasOne(e => e.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(e => e.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ParticipantA)
                .WithMany(m => m.MatchesAsParticipantA)
                .HasForeignKey(e => e.ParticipantAId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ParticipantB)
                .WithMany(m => m.MatchesAsParticipantB)
                .HasForeignKey(e => e.ParticipantBId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Winner)
                .WithMany(m => m.MatchesWon)
                .HasForeignKey(e => e.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.NextMatch)
                .WithMany()
                .HasForeignKey(e => e.NextMatchId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActionType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EntityType).IsRequired().HasMaxLength(100);
            
            entity.HasOne(e => e.ActorUser)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(e => e.ActorUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
