<script>
    import { onMount } from "svelte";
    import { api } from "./api.js";
    import { goto } from "$app/navigation";
    import BracketVisualizer from "./BracketVisualizer.svelte";
    import JoinTournamentModal from "$lib/components/JoinTournamentModal.svelte";

    export let tournament;

    let activeTab = "BRIEF"; // BRIEF | BRACKET | ROSTER
    let details = null;
    let loading = true;
    let showJoinModal = false;

    function handleJoinSuccess() {
        // Reload details to show new member
        loading = true;
        api.tournaments.get(tournament.id).then((data) => {
            details = data;
            loading = false;
        });
    }

    // derived
    $: bgImage = tournament?.imageUrl || "";
    $: statusText =
        tournament?.status === 1
            ? "RECRUITING"
            : tournament?.status === 3
              ? "LIVE"
              : "ARCHIVED";

    onMount(async () => {
        if (tournament) {
            try {
                details = await api.tournaments.get(tournament.id);
            } catch (e) {
                console.error(e);
            } finally {
                loading = false;
            }
        }
    });

    import { auth } from "$lib/stores/auth";

    function goBack() {
        goto("/recruiting"); // Default fallback
    }

    async function handleSetWinner(matchId, winnerId) {
        if (!confirm("CONFIRM WINNER SELECTION?")) return;
        try {
            await api.matches.setWinner(matchId, winnerId);
            // Refresh
            handleJoinSuccess(); // Reuses the reload logic
        } catch (e) {
            alert("SET WINNER FAILED: " + e.message);
        }
    }

    async function handleRemoveMember(walkerId) {
        if (!confirm("CONFIRM REMOVAL OF WALKER?")) return;
        try {
            await api.tournaments.removeMember(tournament.id, walkerId);
            handleJoinSuccess();
        } catch (e) {
            alert("REMOVAL FAILED: " + e.message);
        }
    }
</script>

<div class="details-container">
    <!-- HERO SECTION -->
    <div class="details-hero">
        <div class="hero-bg" style="background-image: url({bgImage})"></div>
        <div class="hero-overlay"></div>

        <div class="hero-content">
            <button class="back-btn" on:click={goBack}
                >&larr; ABORT MISSION</button
            >
            <div class="tags">
                <span class="status-tag">{statusText}</span>
                <span class="id-tag"
                    >OP-ID: {tournament.id.substring(0, 8)}</span
                >
            </div>
            <h1 class="hero-title">{tournament.title}</h1>
        </div>
    </div>

    <!-- TABS -->
    <div class="tabs-container">
        <div class="tabs">
            <button
                class:active={activeTab === "BRIEF"}
                on:click={() => (activeTab = "BRIEF")}>MISSION BRIEF</button
            >
            <button
                class:active={activeTab === "BRACKET"}
                on:click={() => (activeTab = "BRACKET")}>BRACKET INTEL</button
            >
            <button
                class:active={activeTab === "ROSTER"}
                on:click={() => (activeTab = "ROSTER")}>ACTIVE ROSTER</button
            >
        </div>
    </div>

    <!-- CONTENT -->
    <div class="content-area">
        {#if loading}
            <div class="loader-box">DECRYPTING INTEL...</div>
        {:else if activeTab === "BRIEF"}
            <div class="panel brief-panel">
                <div class="brief-content">
                    <h3 class="panel-header">OPERATION OVERVIEW</h3>
                    <p class="desc">
                        {details?.description ||
                            "NO MISSION BRIEFING PROVIDED FOR THIS OPERATION."}
                    </p>

                    <div class="stats-hud">
                        <div class="hud-item entry">
                            <span class="hud-label">XP PER WIN</span>
                            <span class="hud-val"
                                >{details?.xpPerWin || 0} XP</span
                            >
                        </div>
                        <div class="hud-item prize">
                            <span class="hud-label">SQUAD CAP</span>
                            <span class="hud-val gold"
                                >{details?.maxParticipants || 16}</span
                            >
                        </div>
                        <div class="hud-item players">
                            <span class="hud-label">ACTIVE ROSTER</span>
                            <span class="hud-val"
                                >{details?.memberCount || 0}</span
                            >
                        </div>
                    </div>
                </div>

                <div class="action-area">
                    {#if tournament.status === 1}
                        <button
                            class="join-btn"
                            on:click={() => (showJoinModal = true)}
                        >
                            <span class="btn-text">JOIN MISSION</span>
                            <div class="btn-glitch"></div>
                        </button>
                    {:else}
                        <button
                            class="join-btn"
                            disabled
                            style="opacity: 0.5; cursor: not-allowed;"
                        >
                            <span class="btn-text">RECRUITMENT CLOSED</span>
                        </button>
                    {/if}
                    <p class="terms">
                        By joining, you accept the Walker Protocol.
                    </p>
                </div>
            </div>

            <JoinTournamentModal
                isOpen={showJoinModal}
                tournamentId={tournament.id}
                tournamentName={tournament.title}
                on:close={() => (showJoinModal = false)}
                on:success={handleJoinSuccess}
            />
        {:else if activeTab === "BRACKET"}
            <div class="panel">
                <h3 class="panel-header">TOURNAMENT BRACKET</h3>
                {#if details?.matches && details.matches.length > 0}
                    <BracketVisualizer matches={details.matches} />

                    <!-- ADMIN ACTIONS FOR MATCHES -->
                    {#if $auth.user?.role === "TeamAlan"}
                        <div class="admin-match-controls">
                            <h4>ADMIN OVERRIDE PROTOCOLS</h4>
                            <div class="match-list">
                                {#each details.matches as m}
                                    <div class="match-item">
                                        <span
                                            >MATCH #{m.matchId.substring(0, 6)}: {m.player1Name ||
                                                "TBD"} vs {m.player2Name ||
                                                "TBD"}</span
                                        >
                                        {#if !m.winnerId && m.player1Id && m.player2Id}
                                            <div class="match-actions">
                                                <button
                                                    class="win-btn"
                                                    on:click={() =>
                                                        handleSetWinner(
                                                            m.matchId,
                                                            m.player1Id,
                                                        )}
                                                    >WIN: {m.player1Name}</button
                                                >
                                                <button
                                                    class="win-btn"
                                                    on:click={() =>
                                                        handleSetWinner(
                                                            m.matchId,
                                                            m.player2Id,
                                                        )}
                                                    >WIN: {m.player2Name}</button
                                                >
                                            </div>
                                        {:else if m.winnerId}
                                            <span class="winner-tag"
                                                >WINNER: {m.winnerId ===
                                                m.player1Id
                                                    ? m.player1Name
                                                    : m.player2Name}</span
                                            >
                                        {/if}
                                    </div>
                                {/each}
                            </div>
                        </div>
                    {/if}
                {:else}
                    <div class="bracket-placeholder">
                        NO ACTIVE BRACKET DATA FOUND.
                    </div>
                {/if}
            </div>
        {:else if activeTab === "ROSTER"}
            <div class="panel">
                <h3 class="panel-header">REGISTERED WALKERS</h3>
                {#if details?.members && details.members.length > 0}
                    <ul class="roster-list">
                        {#each details.members as member}
                            <li>
                                <span>
                                    {member.walkerName}
                                    <span class="walker-id"
                                        >#{member.walkerId}</span
                                    >
                                </span>
                                {#if $auth.user?.role === "TeamAlan" && (tournament.status === 0 || tournament.status === 1)}
                                    <button
                                        class="remove-btn"
                                        on:click={() =>
                                            handleRemoveMember(member.walkerId)}
                                        >REMOVE</button
                                    >
                                {/if}
                            </li>
                        {/each}
                    </ul>
                {:else}
                    <div class="bracket-placeholder">
                        NO WALKERS REGISTERED.
                    </div>
                {/if}
            </div>
        {/if}
    </div>
</div>

<style>
    .details-container {
        color: #fff;
        animation: fadeIn 0.5s ease-out;
    }
    @keyframes fadeIn {
        from {
            opacity: 0;
        }
        to {
            opacity: 1;
        }
    }

    /* HERO */
    .details-hero {
        position: relative;
        height: 400px;
        display: flex;
        align-items: flex-end;
        padding: 0 3rem 3rem;
        margin-bottom: 2rem;
    }
    .hero-bg {
        position: absolute;
        inset: 0;
        background-size: cover;
        background-position: center;
        opacity: 0.6;
        mask-image: linear-gradient(to bottom, black 50%, transparent 100%);
    }
    .hero-overlay {
        position: absolute;
        inset: 0;
        background: linear-gradient(to top, var(--wow-bg) 0%, transparent 100%);
    }
    .hero-content {
        position: relative;
        z-index: 2;
        width: 100%;
        max-width: 1200px;
        margin: 0 auto;
    }

    .back-btn {
        background: none;
        border: none;
        color: var(--wow-text-muted);
        cursor: pointer;
        padding: 0;
        margin-bottom: 1rem;
        font-family: monospace;
        letter-spacing: 2px;
        transition: color 0.3s;
    }
    .back-btn:hover {
        color: var(--wow-cyan);
    }

    .tags {
        display: flex;
        gap: 1rem;
        margin-bottom: 1rem;
    }
    .status-tag {
        background: var(--wow-cyan);
        color: #000;
        padding: 2px 8px;
        font-weight: bold;
        border-radius: 2px;
        font-size: 0.8rem;
    }
    .id-tag {
        color: var(--wow-text-muted);
        font-family: monospace;
    }

    .hero-title {
        font-size: 3.5rem;
        margin: 0;
        line-height: 1;
        text-transform: uppercase;
        text-shadow: 0 0 20px rgba(0, 0, 0, 0.5);
    }

    /* TABS */
    .tabs-container {
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        margin-bottom: 3rem;
        padding: 0 3rem;
    }
    .tabs {
        max-width: 1200px;
        margin: 0 auto;
        display: flex;
        gap: 2rem;
    }
    .tabs button {
        background: none;
        border: none;
        color: var(--wow-text-muted);
        padding: 1rem 0;
        font-size: 1rem;
        letter-spacing: 2px;
        cursor: pointer;
        position: relative;
    }
    .tabs button.active {
        color: #fff;
    }
    .tabs button.active::after {
        content: "";
        position: absolute;
        bottom: -1px;
        left: 0;
        width: 100%;
        height: 2px;
        background: var(--wow-cyan);
        box-shadow: 0 0 10px var(--wow-cyan);
    }

    /* CONTENT */
    .content-area {
        max-width: 1200px;
        margin: 0 auto;
        padding: 0 3rem 3rem;
    }
    .panel-header {
        font-size: 1.5rem;
        color: var(--wow-cyan);
        margin-bottom: 1rem;
        font-family: var(--wow-font-display);
        letter-spacing: 2px;
    }
    .desc {
        color: var(--wow-text-body);
        max-width: 800px;
        line-height: 1.8;
        margin-bottom: 3rem;
        font-size: 1.1rem;
    }

    /* STATS HUD - GLASS TERMINAL STYLE */
    .stats-hud {
        display: flex;
        gap: var(--space-5);
        margin-bottom: var(--space-6);
        background: var(--wow-secondary);
        border: 1px solid rgba(0, 243, 255, 0.1);
        padding: var(--space-5);
        border-radius: 12px;
        backdrop-filter: blur(12px);
        box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.37);
        justify-content: space-around;
        text-align: center;
    }
    .hud-item {
        display: flex;
        flex-direction: column;
        gap: 0.8rem;
        flex: 1;
        position: relative;
    }
    .hud-item:not(:last-child)::after {
        content: "";
        position: absolute;
        right: -1rem;
        top: 20%;
        height: 60%;
        width: 1px;
        background: rgba(255, 255, 255, 0.1);
    }

    .hud-label {
        font-family: var(--wow-font-display);
        font-size: 0.9rem;
        color: var(--wow-cyan);
        letter-spacing: 2px;
        opacity: 0.8;
    }
    .hud-val {
        font-size: 2.5rem;
        font-weight: 900;
        color: #fff;
        line-height: 1;
    }
    .hud-val.gold {
        color: #ffd700;
        text-shadow: 0 0 20px rgba(255, 215, 0, 0.3);
    }

    /* ACTION AREA */
    .action-area {
        margin-top: 2rem;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 1.5rem;
        border-top: 1px solid rgba(255, 255, 255, 0.05);
        padding-top: 3rem;
    }
    .join-btn {
        position: relative;
        background: transparent;
        color: var(--wow-cyan);
        border: 1px solid var(--wow-cyan);
        padding: 1.2rem 4rem;
        font-size: 1.4rem;
        font-weight: 900;
        letter-spacing: 3px;
        cursor: pointer;
        overflow: hidden;
        clip-path: polygon(
            15px 0,
            100% 0,
            100% calc(100% - 15px),
            calc(100% - 15px) 100%,
            0 100%,
            0 15px
        );
        transition: all 0.3s;
        box-shadow: 0 0 15px rgba(0, 243, 255, 0.1);
    }
    .join-btn:hover {
        background: var(--wow-cyan);
        color: #000;
        box-shadow: 0 0 40px rgba(0, 243, 255, 0.4);
        transform: scale(1.05);
    }

    .terms {
        font-size: 0.9rem;
        color: var(--wow-text-muted);
        opacity: 0.6;
    }

    /* ROSTER */
    .roster-list {
        list-style: none;
        padding: 0;
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: 1rem;
    }
    .roster-list li {
        background: rgba(255, 255, 255, 0.03);
        border: 1px solid rgba(255, 255, 255, 0.05);
        padding: var(--space-3);
        border-radius: 4px;
        display: flex;
        justify-content: space-between;
        align-items: center;
        transition: all var(--wow-transition-base) var(--wow-ease-out);
    }
    .roster-list li:hover {
        background: rgba(0, 243, 255, 0.05);
        border-color: rgba(0, 243, 255, 0.2);
        transform: translateY(-2px);
    }
    .walker-id {
        font-family: monospace;
        color: var(--wow-cyan);
        font-size: 0.8rem;
        opacity: 0.7;
    }

    .loader-box {
        text-align: center;
        padding: 4rem;
        color: var(--wow-cyan);
        letter-spacing: 4px;
        font-family: var(--wow-font-display);
        animation: pulse 1s infinite alternate;
    }
    @keyframes pulse {
        from {
            opacity: 0.5;
        }
        to {
            opacity: 1;
        }
    }

    /* ADMIN CONTROLS */
    .admin-match-controls {
        margin-top: 2rem;
        padding-top: 2rem;
        border-top: 1px dashed rgba(255, 0, 0, 0.3);
    }
    .admin-match-controls h4 {
        color: #ff3e3e;
        font-family: var(--wow-font-display);
        letter-spacing: 2px;
        margin-bottom: 1rem;
    }
    .match-item {
        background: rgba(255, 0, 0, 0.05);
        border: 1px solid rgba(255, 0, 0, 0.2);
        padding: 1rem;
        margin-bottom: 0.5rem;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .win-btn {
        background: rgba(255, 0, 0, 0.2);
        border: 1px solid #ff3e3e;
        color: #ff3e3e;
        padding: 0.3rem 0.8rem;
        margin-left: 0.5rem;
        cursor: pointer;
        font-size: 0.8rem;
    }
    .win-btn:hover {
        background: #ff3e3e;
        color: #000;
    }
    .winner-tag {
        color: #00ff41;
        font-weight: bold;
    }

    .remove-btn {
        background: transparent;
        border: 1px solid #ff3e3e;
        color: #ff3e3e;
        font-size: 0.7rem;
        cursor: pointer;
        padding: 0.2rem 0.5rem;
        font-family: var(--wow-font-display);
    }
    .remove-btn:hover {
        background: rgba(255, 62, 62, 0.1);
    }
</style>
