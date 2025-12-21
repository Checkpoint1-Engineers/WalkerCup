<script lang="ts">
	import TournamentDetails from '$lib/TournamentDetails.svelte';
	export let data;
</script>

{#if data.tournament}
	<TournamentDetails tournament={{ ...data.tournament, title: data.tournament.name }} />
{:else}
	<div class="empty-state">TOURNAMENT NOT FOUND</div>
{/if}
