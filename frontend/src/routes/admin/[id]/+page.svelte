<script>
    import { page } from "$app/stores";
    import { onMount } from "svelte";
    import { api } from "$lib/api";
    import CyberButton from "$lib/components/CyberButton.svelte";

    let tournament = null;
    let loading = true;
    let matches = [];
    let id = $page.params.id;

    onMount(async () => {
        await loadData();
    });

    async function loadData() {
        loading = true;
        try {
            tournament = await api.tournaments.get(id);
            matches = tournament.matches || [];
            // Sort by round, then order
            matches.sort(
                (a, b) =>
                    a.roundNumber - b.roundNumber ||
                    a.matchOrder - b.matchOrder,
            );
        } catch (e) {
            console.error(e);
            alert("LOAD FAILED: " + e.message);
        } finally {
            loading = false;
        }
    }

    async function setWinner(match, participant) {
        if (
            !confirm(
                `DECLARE ${participant.walkerName} WINNER OF MATCH #${match.matchOrder}?`,
            )
        )
            return;
        try {
            await api.matches.setWinner(match.id, participant.id);
            await loadData(); // Reload to see updates
        } catch (e) {
            alert("OVERRIDE FAILED: " + e.message);
        }
    }
</script>

<div class="control-room">
    <div class="header">
        <a href="/admin" class="back-link">← COMMAND</a>
        <h1>OPERATION: {tournament?.name || "LOADING..."}</h1>
    </div>

    {#if loading}
        <div class="loading">ESTABLISHING UPLINK...</div>
    {:else if tournament}
        <div class="matches-grid">
            {#if matches.length === 0}
                <div class="no-data">
                    NO ACTIVE COMBAT DATA (BRACKET NOT DRAWN)
                </div>
            {/if}

            {#each matches as m}
                <div class="match-card {m.winner ? 'resolved' : 'active'}">
                    <div class="match-header">
                        ROUND {m.roundNumber} - M{m.matchOrder}
                    </div>

                    <div class="combatants">
                        <div
                            class="fighter {m.winner?.id === m.participantA?.id
                                ? 'winner'
                                : ''}"
                        >
                            <div class="name">
                                {m.participantA?.walkerName || "TBD"}
                            </div>
                            {#if m.participantA}
                                <button
                                    class="win-btn"
                                    on:click={() =>
                                        setWinner(m, m.participantA)}
                                    disabled={!!m.winner}
                                >
                                    {m.winner?.id === m.participantA.id
                                        ? "VICTOR"
                                        : "SET WIN"}
                                </button>
                            {/if}
                        </div>
                        <div class="vs">VS</div>
                        <div
                            class="fighter {m.winner?.id === m.participantB?.id
                                ? 'winner'
                                : ''}"
                        >
                            <div class="name">
                                {m.participantB?.walkerName || "TBD"}
                            </div>
                            {#if m.participantB}
                                <button
                                    class="win-btn"
                                    on:click={() =>
                                        setWinner(m, m.participantB)}
                                    disabled={!!m.winner}
                                >
                                    {m.winner?.id === m.participantB.id
                                        ? "VICTOR"
                                        : "SET WIN"}
                                </button>
                            {/if}
                        </div>
                    </div>
                </div>
            {/each}
        </div>
    {/if}
</div>

<style>
    .control-room {
        max-width: 1200px;
        margin: 0 auto;
    }

    .header {
        margin-bottom: 2rem;
        border-bottom: 1px solid rgba(0, 243, 255, 0.2);
        padding-bottom: 1rem;
    }

    .back-link {
        color: var(--wow-cyan);
        text-decoration: none;
        font-family: var(--wow-font-display);
        display: block;
        margin-bottom: 0.5rem;
    }

    h1 {
        font-family: var(--wow-font-display);
        color: #fff;
        margin: 0;
        font-size: 2rem;
        letter-spacing: 2px;
    }

    .matches-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
        gap: 1.5rem;
    }

    .match-card {
        background: rgba(0, 20, 30, 0.6);
        border: 1px solid rgba(255, 255, 255, 0.1);
        padding: 1rem;
    }

    .match-card.active {
        border-color: var(--wow-cyan);
        box-shadow: 0 0 15px rgba(0, 243, 255, 0.1);
    }

    .match-header {
        font-family: monospace;
        color: rgba(255, 255, 255, 0.5);
        font-size: 0.8rem;
        margin-bottom: 1rem;
        text-align: center;
    }

    .combatants {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .fighter {
        flex: 1;
        text-align: center;
        padding: 0.5rem;
        border: 1px solid transparent;
        transition: all 0.2s;
    }

    .fighter.winner {
        border-color: #00ff41;
        background: rgba(0, 255, 65, 0.1);
    }

    .name {
        font-weight: bold;
        margin-bottom: 0.5rem;
        color: #fff;
    }

    .vs {
        margin: 0 1rem;
        font-family: var(--wow-font-display);
        color: rgba(255, 255, 255, 0.3);
    }

    .win-btn {
        background: transparent;
        border: 1px solid rgba(255, 255, 255, 0.3);
        color: rgba(255, 255, 255, 0.7);
        font-size: 0.7rem;
        padding: 2px 6px;
        cursor: pointer;
    }
    .win-btn:hover:not(:disabled) {
        border-color: var(--wow-cyan);
        color: var(--wow-cyan);
    }
    .win-btn:disabled {
        border: none;
        color: #00ff41;
        cursor: default;
    }

    .no-data {
        grid-column: 1 / -1;
        text-align: center;
        padding: 3rem;
        color: rgba(255, 255, 255, 0.3);
        border: 1px dashed rgba(255, 255, 255, 0.1);
    }
</style>
