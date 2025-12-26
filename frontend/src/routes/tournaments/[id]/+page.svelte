<script>
    import { page } from "$app/stores";
    import { onMount } from "svelte";
    import { api } from "$lib/api";
    import TournamentDetails from "$lib/TournamentDetails.svelte";
    import CyberButton from "$lib/components/CyberButton.svelte";

    let tournamentId = $page.params.id;
    let tournament = null;
    let loading = true;
    let error = null;

    onMount(async () => {
        try {
            tournament = await api.tournaments.get(tournamentId);
        } catch (e) {
            error = e.message;
        } finally {
            loading = false;
        }
    });

    function handleBack() {
        window.history.back();
    }
</script>

<div class="page-container">
    <div class="nav-header">
        <CyberButton
            label="< BACK TO ADMIN"
            variant="secondary"
            on:click={handleBack}
        />
    </div>

    {#if loading}
        <div class="loading">LOADING TOURNAMENT DATA...</div>
    {:else if error}
        <div class="error">ERROR: {error}</div>
    {:else if tournament}
        <!-- Pass the full object as 'tournament' prop -->
        <TournamentDetails {tournament} />
    {:else}
        <div class="not-found">TOURNAMENT NOT FOUND</div>
    {/if}
</div>

<style>
    .page-container {
        max-width: 1400px;
        margin: 0 auto;
        /* Padding removed to let hero image take full width if designed that way, 
           but page-container restricts width. 
           TournamentDetails has its own max-width containers. */
    }

    .nav-header {
        padding: 1rem 2rem;
    }

    .loading,
    .error,
    .not-found {
        text-align: center;
        font-family: var(--wow-font-display);
        font-size: 1.5rem;
        color: var(--wow-cyan);
        margin-top: 4rem;
    }

    .error {
        color: #ff0055;
    }
</style>
