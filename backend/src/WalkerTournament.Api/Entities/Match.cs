using System.ComponentModel.DataAnnotations;

namespace WalkerTournament.Api.Entities;

public class Match
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public int RoundNumber { get; set; }
    public int MatchOrder { get; set; }
    public Guid? ParticipantAId { get; set; }
    public Guid? ParticipantBId { get; set; }
    public Guid? WinnerId { get; set; }
    public Guid? NextMatchId { get; set; }
    public bool IsParticipantASlotInNextMatch { get; set; } // True = slot A, False = slot B

    [ConcurrencyCheck]
    public byte[] RowVersion { get; set; } = Guid.NewGuid().ToByteArray();

    // Navigation properties
    public Tournament Tournament { get; set; } = null!;
    public TournamentMember? ParticipantA { get; set; }
    public TournamentMember? ParticipantB { get; set; }
    public TournamentMember? Winner { get; set; }
    public Match? NextMatch { get; set; }
}
