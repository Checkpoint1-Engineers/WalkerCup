<script>
    export let matches = [];

    // Group matches by round
    $: rounds = matches.reduce((acc, match) => {
        const r = match.roundNumber;
        if (!acc[r]) acc[r] = [];
        acc[r].push(match);
        return acc;
    }, {});

    $: sortedRoundKeys = Object.keys(rounds).sort(
        (a, b) => Number(a) - Number(b),
    );

    function getParticipantName(p) {
        return p?.walkerName || "TBD";
    }

    function isWinner(participant, winner) {
        return participant && winner && participant.id === winner.id;
    }
</script>

<div class="bracket-viewport">
    <div class="bracket-scroll">
        {#each sortedRoundKeys as roundNum, i}
            <div class="round-col">
                <div class="round-header">ROUND {roundNum}</div>
                <div class="matches-list">
                    {#each rounds[roundNum] as match (match.id)}
                        <div class="match-container">
                            <div
                                class="match-card"
                                class:has-winner={match.winner}
                            >
                                <div
                                    class="participant top"
                                    class:winner={isWinner(
                                        match.participantA,
                                        match.winner,
                                    )}
                                >
                                    <span class="name"
                                        >{getParticipantName(
                                            match.participantA,
                                        )}</span
                                    >
                                    {#if isWinner(match.participantA, match.winner)}
                                        <span class="win-tag">WIN</span>
                                    {/if}
                                </div>
                                <div class="vs-divider"></div>
                                <div
                                    class="participant bottom"
                                    class:winner={isWinner(
                                        match.participantB,
                                        match.winner,
                                    )}
                                >
                                    <span class="name"
                                        >{getParticipantName(
                                            match.participantB,
                                        )}</span
                                    >
                                    {#if isWinner(match.participantB, match.winner)}
                                        <span class="win-tag">WIN</span>
                                    {/if}
                                </div>
                            </div>

                            <!-- Connector Lines (Visual Only) -->
                            {#if i < sortedRoundKeys.length - 1}
                                <div class="connector-right"></div>
                            {/if}
                        </div>
                    {/each}
                </div>
            </div>
        {/each}
    </div>
</div>

<style>
    .bracket-viewport {
        width: 100%;
        overflow-x: auto;
        padding: 2rem 0;
        background: rgba(0, 0, 0, 0.2);
        border: 1px solid rgba(255, 255, 255, 0.05);
        border-radius: 8px;
    }

    .bracket-scroll {
        display: flex;
        gap: 4rem;
        padding: 0 2rem;
        min-width: max-content;
    }

    .round-col {
        display: flex;
        flex-direction: column;
        width: 220px;
    }

    .round-header {
        text-align: center;
        font-family: var(--wow-font-display);
        color: var(--wow-cyan);
        font-size: 0.8rem;
        letter-spacing: 3px;
        margin-bottom: 2rem;
        opacity: 0.6;
    }

    .matches-list {
        display: flex;
        flex-direction: column;
        justify-content: space-around;
        flex-grow: 1;
        gap: 2rem;
    }

    .match-container {
        position: relative;
        display: flex;
        align-items: center;
    }

    .match-card {
        width: 100%;
        background: #050a10;
        border: 1px solid rgba(255, 255, 255, 0.1);
        border-radius: 4px;
        position: relative;
        z-index: 2;
        transition:
            border-color 0.3s,
            box-shadow 0.3s;
    }

    .match-card.has-winner {
        border-color: rgba(0, 243, 255, 0.2);
    }

    .participant {
        padding: 0.8rem 1rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-family: monospace;
        font-size: 0.85rem;
        color: #888;
        transition: all 0.3s;
    }

    .participant.winner {
        color: #fff;
        background: rgba(0, 243, 255, 0.05);
    }

    .name {
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
    }

    .win-tag {
        font-size: 0.6rem;
        background: var(--wow-cyan);
        color: #000;
        padding: 1px 4px;
        font-weight: bold;
        border-radius: 2px;
        box-shadow: 0 0 10px var(--wow-cyan);
    }

    .vs-divider {
        height: 1px;
        background: rgba(255, 255, 255, 0.05);
    }

    /* CONNECTOR LINES */
    .connector-right {
        position: absolute;
        right: -4rem;
        top: 50%;
        width: 4rem;
        height: 2px;
        background: rgba(255, 255, 255, 0.1);
        z-index: 1;
    }

    /* Scrollbar Styling */
    .bracket-viewport::-webkit-scrollbar {
        height: 6px;
    }
    .bracket-viewport::-webkit-scrollbar-track {
        background: rgba(0, 0, 0, 0.2);
    }
    .bracket-viewport::-webkit-scrollbar-thumb {
        background: var(--wow-cyan);
        border-radius: 3px;
        opacity: 0.3;
    }
</style>
