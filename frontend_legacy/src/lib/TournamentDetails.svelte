<script>
    import { onMount } from "svelte";
    import { api } from "./api.js";
    import { navigate } from "./router.js";

    export let tournament;

    let activeTab = "BRIEF"; // BRIEF | BRACKET | ROSTER
    let details = null;
    let loading = true;

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

    function goBack() {
        navigate("/recruiting"); // Default fallback
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
                        Get ready for the ultimate showdown. This tournament
                        pits the best Walkers against each other. Prepare your
                        loadouts and sync your drones.
                    </p>

                    <div class="stats-hud">
                        <div class="hud-item entry">
                            <span class="hud-label">ENTRY FEE</span>
                            <span class="hud-val"
                                >{details?.entryFee || 0} WC</span
                            >
                        </div>
                        <div class="hud-item prize">
                            <span class="hud-label">PRIZE POOL</span>
                            <span class="hud-val gold"
                                >{details?.prizePool || 0} WC</span
                            >
                        </div>
                        <div class="hud-item players">
                            <span class="hud-label">OPERATIVES</span>
                            <span class="hud-val"
                                >{details?.maxParticipants || 16} MAX</span
                            >
                        </div>
                    </div>
                </div>

                <div class="action-area">
                    <button class="join-btn">
                        <span class="btn-text">JOIN MISSION</span>
                        <div class="btn-glitch"></div>
                    </button>
                    <p class="terms">
                        By joining, you accept the Walker Protocol.
                    </p>
                </div>
            </div>
        {:else if activeTab === "BRACKET"}
            <div class="panel">
                <h3 class="panel-header">TOURNAMENT BRACKET</h3>
                <div class="bracket-placeholder">
                    NO ACTIVE BRACKET DATA FOUND.
                </div>
            </div>
        {:else if activeTab === "ROSTER"}
            <div class="panel">
                <h3 class="panel-header">REGISTERED WALKERS</h3>
                <ul class="roster-list">
                    <li>Alan Walker #1</li>
                    <li>Walker #2492</li>
                    <li>Walker #9999</li>
                </ul>
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
        gap: 2rem;
        margin-bottom: 3rem;
        background: rgba(2, 11, 20, 0.6);
        border: 1px solid rgba(0, 243, 255, 0.1);
        padding: 2.5rem;
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
</style>
