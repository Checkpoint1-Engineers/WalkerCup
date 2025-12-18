<script>
    import TournamentCard from "./TournamentCard.svelte";
    export let tournaments = [];
</script>

<div class="cards-grid">
    {#each tournaments as t (t.id)}
        <TournamentCard
            {...t}
            title={t.name}
            imageUrl={t.imageUrl}
            status={t.status}
            joinDeadline={t.joinDeadline}
        />
    {/each}
</div>

<style>
    .cards-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
        gap: 2rem;
        padding-bottom: 4rem;
    }
</style>
