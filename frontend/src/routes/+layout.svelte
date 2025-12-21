<script lang="ts">
	import '../app.css';
	import Header from '$lib/Header.svelte';
	import Footer from '$lib/Footer.svelte';

	let { children } = $props();
</script>

<div class="dashboard-layout">
	<Header />
	<main class="main-content">
		{@render children()}
	</main>
	<Footer />
</div>
