<script>
	import { onMount } from "svelte";
	import { navigating } from "$app/stores";
	import { api } from "$lib/api.js";
	import "../app.css";
	import Header from "$lib/Header.svelte";
	import Footer from "$lib/Footer.svelte";
	import CyberCard from "$lib/CyberCard.svelte";
	import Loading from "$lib/components/Loading.svelte";
	import { tournaments, loading } from "$lib/stores/tournaments.js";

	let isBooting = true;
	let pageTransitioning = false;

	// Watch for navigation changes
	$: if ($navigating) {
		pageTransitioning = true;
	} else {
		setTimeout(() => (pageTransitioning = false), 300);
	}

	onMount(async () => {
		// Initial boot delay
		setTimeout(() => (isBooting = false), 2000);

		try {
			const list = await api.tournaments.list();
			tournaments.set(list);
		} catch (e) {
			console.error("Failed to load tournaments", e);
		} finally {
			loading.set(false);
		}
	});
</script>

<div class="dashboard-layout">
	<Header />

	<CyberCard>
		<main class="main-content">
			{#if isBooting || $loading || pageTransitioning}
				<Loading />
			{:else}
				<div class="page-content">
					<slot />
				</div>
			{/if}
		</main>
	</CyberCard>

	<Footer />
</div>

<style>
	.dashboard-layout {
		min-height: 100vh;
		display: flex;
		flex-direction: column;
		width: 100%;
	}

	.main-content {
		/* Height = Auto matches content */
		min-height: auto;
		max-width: 1600px;
		margin: 0 auto;
		padding: 0 1rem 1rem;
		width: 100%;
		box-sizing: border-box;
		position: relative;
		flex: 1;
		display: flex;
		flex-direction: column;
	}

	.page-content {
		animation: fadeIn 0.4s ease-out;
		flex: 1;
		display: flex;
		flex-direction: column;
	}

	@keyframes fadeIn {
		from {
			opacity: 0;
			transform: translateY(10px);
		}
		to {
			opacity: 1;
			transform: translateY(0);
		}
	}

	@media (max-width: 768px) {
		.main-content {
			min-height: calc(100vh - 60px);
			padding: 0 0.5rem 1rem;
		}
	}
</style>
