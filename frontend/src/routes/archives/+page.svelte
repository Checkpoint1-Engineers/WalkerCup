<script>
    import { tournaments } from "$lib/stores/tournaments.js";
    import TournamentGrid from "$lib/TournamentGrid.svelte";

    // Status 4 = Archives/Past
    $: filtered = $tournaments.filter((t) => t.status === 4);
</script>

<header class="content-header">
    <div>
        <h2 class="section-title">MISSION ARCHIVES</h2>
        <p class="section-sub">
            History of the Walkers. Study past conflicts and results.
        </p>
    </div>
</header>

{#if filtered.length > 0}
    <TournamentGrid tournaments={filtered} />
{:else}
    <div class="empty-state">NO DATA FOUND IN THIS SECTOR.</div>
{/if}

<style>
    .content-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-end;
        margin-bottom: 2rem;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        padding-bottom: 1rem;
    }
    .section-title {
        font-family: var(--wow-font-display, sans-serif);
        font-size: 2.5rem;
        font-weight: 800;
        letter-spacing: 2px;
        color: #fff;
        margin: 0;
        text-transform: uppercase;
        line-height: 1;
        text-shadow: 0 0 20px rgba(0, 243, 255, 0.4);
    }
    .section-sub {
        font-family: "Consolas", "Monaco", monospace;
        color: var(--wow-cyan, #00f3ff);
        margin: 0.5rem 0 0;
        font-size: 0.9rem;
        opacity: 0.8;
        letter-spacing: 1px;
    }
    .empty-state {
        color: white;
        text-align: center;
        margin-top: 4rem;
        font-family: "Consolas", monospace;
    }
</style>
