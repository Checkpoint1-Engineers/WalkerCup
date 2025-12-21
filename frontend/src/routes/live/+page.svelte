<script lang="ts">
	import TournamentGrid from '$lib/TournamentGrid.svelte';
	export let data;
</script>

<header class="content-header">
	<div>
		<h2 class="section-title">LIVE OPERATIONS</h2>
		<p class="section-sub">Active warzones. Watch the elite compete for glory.</p>
	</div>
</header>

{#if data.tournaments.length > 0}
	<TournamentGrid tournaments={data.tournaments} />
{:else}
	<div class="empty-state">NO DATA FOUND IN THIS SECTOR.</div>
{/if}
