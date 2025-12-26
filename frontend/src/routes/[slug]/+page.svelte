<script>
    import { page } from "$app/stores";
    import { tournaments } from "$lib/stores/tournaments.js";
    import TournamentDetails from "$lib/TournamentDetails.svelte";

    $: slug = $page.params.slug;

    $: tournament = $tournaments.find(
        (x) => x.name === decodeURIComponent(slug),
    );
</script>

{#if tournament}
    <TournamentDetails
        tournament={{
            ...tournament,
            title: tournament.name,
        }}
    />
{:else}
    <!-- Optional: Add a 404 or loading state here if desired -->
    <div class="not-found">TOURNAMENT NOT FOUND</div>
{/if}

<style>
    .not-found {
        color: white;
        text-align: center;
        margin-top: 5rem;
        font-size: 2rem;
        font-family: var(--wow-font-display, sans-serif);
    }
</style>
