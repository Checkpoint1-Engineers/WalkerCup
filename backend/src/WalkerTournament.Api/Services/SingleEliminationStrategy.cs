using WalkerTournament.Api.Entities;

namespace WalkerTournament.Api.Services;

public class SingleEliminationStrategy : IBracketStrategy
{
    public IReadOnlyList<Match> GenerateBracket(Guid tournamentId, IReadOnlyList<TournamentMember> participants)
    {
        if (participants.Count < 2)
        {
            throw new InvalidOperationException("Need at least 2 participants for a tournament");
        }

        var matches = new List<Match>();
        var shuffledParticipants = participants.OrderBy(_ => Guid.NewGuid()).ToList();
        
        // Calculate number of rounds needed
        int participantCount = shuffledParticipants.Count;
        int totalRounds = (int)Math.Ceiling(Math.Log2(participantCount));
        int bracketSize = (int)Math.Pow(2, totalRounds);
        int byesNeeded = bracketSize - participantCount;

        // Create all match slots for all rounds
        var matchesByRound = new Dictionary<int, List<Match>>();
        
        // Generate matches from final round backwards (easier to link)
        for (int round = totalRounds; round >= 1; round--)
        {
            int matchesInRound = (int)Math.Pow(2, totalRounds - round);
            matchesByRound[round] = new List<Match>();
            
            for (int i = 0; i < matchesInRound; i++)
            {
                var match = new Match
                {
                    Id = Guid.NewGuid(),
                    TournamentId = tournamentId,
                    RoundNumber = round,
                    MatchOrder = i
                };
                matchesByRound[round].Add(match);
                matches.Add(match);
            }
        }

        // Link matches to next round matches
        for (int round = 1; round < totalRounds; round++)
        {
            var currentRoundMatches = matchesByRound[round];
            var nextRoundMatches = matchesByRound[round + 1];
            
            for (int i = 0; i < currentRoundMatches.Count; i++)
            {
                var match = currentRoundMatches[i];
                match.NextMatchId = nextRoundMatches[i / 2].Id;
                match.IsParticipantASlotInNextMatch = (i % 2 == 0);
            }
        }

        // Assign participants to first round
        var firstRoundMatches = matchesByRound[1];
        int participantIndex = 0;
        
        for (int i = 0; i < firstRoundMatches.Count; i++)
        {
            var match = firstRoundMatches[i];
            
            if (i < byesNeeded)
            {
                // This match is a bye - one participant advances automatically
                if (participantIndex < shuffledParticipants.Count)
                {
                    match.ParticipantAId = shuffledParticipants[participantIndex].Id;
                    match.ParticipantBId = null; // Bye
                    match.WinnerId = shuffledParticipants[participantIndex].Id; // Auto-advance
                    participantIndex++;
                }
            }
            else
            {
                // Normal match with two participants
                if (participantIndex < shuffledParticipants.Count)
                {
                    match.ParticipantAId = shuffledParticipants[participantIndex].Id;
                    participantIndex++;
                }
                if (participantIndex < shuffledParticipants.Count)
                {
                    match.ParticipantBId = shuffledParticipants[participantIndex].Id;
                    participantIndex++;
                }
            }
        }

        // Advance bye winners to next round
        foreach (var match in firstRoundMatches.Where(m => m.WinnerId != null && m.NextMatchId != null))
        {
            var nextMatch = matches.First(m => m.Id == match.NextMatchId);
            if (match.IsParticipantASlotInNextMatch)
            {
                nextMatch.ParticipantAId = match.WinnerId;
            }
            else
            {
                nextMatch.ParticipantBId = match.WinnerId;
            }
        }

        return matches;
    }
}
