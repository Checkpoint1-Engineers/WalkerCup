<script lang="ts">
	import TournamentGrid from '$lib/TournamentGrid.svelte';

	export let data;
</script>

<header class="content-header">
	<div>
		<h2 class="section-title">OPEN MISSIONS</h2>
		<p class="section-sub">The journey begins here. Prove your worth in open qualifiers.</p>
	</div>
</header>

{#if data.tournaments.length > 0}
	<TournamentGrid tournaments={data.tournaments} />
{:else}
	<div class="empty-state">NO DATA FOUND IN THIS SECTOR.</div>
{/if}
