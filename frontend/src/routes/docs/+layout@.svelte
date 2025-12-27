<script lang="ts">
	import '../../app.css';
	import '../docs.css';
	import { page } from '$app/stores';

	let { children } = $props();

	const navGroups = [
		{
			title: 'Mission Briefing',
			items: [{ label: 'System Overview', path: '/docs' }]
		},
		{
			title: 'Frontend Operations',
			items: [{ label: 'Integration Guide', path: '/docs/frontend' }]
		},
		{
			title: 'Backend Intelligence',
			items: [{ label: 'Architecture Deep Dive', path: '/docs/backend' }]
		},
		{
			title: 'Reference',
			items: [{ label: 'API Endpoints', path: '/docs/reference' }]
		}
	];
</script>

<div class="docs-layout">
	<aside class="docs-sidebar">
		<div class="sidebar-header">
			<a href="/" class="logo">WALKER API</a>
			<div class="version">DOCS V1.0</div>
		</div>

		<nav class="nav-scroller">
			{#each navGroups as group (group.title)}
				<div class="nav-group">
					<div class="nav-title">{group.title}</div>
					{#each group.items as item (item.path)}
						<a href={item.path} class="nav-item" class:active={$page.url.pathname === item.path}>
							{item.label}
						</a>
					{/each}
				</div>
			{/each}
		</nav>

		<div class="sidebar-footer">
			<a href="/" class="back-link">← Back to App</a>
		</div>
	</aside>

	<main class="docs-content">
		<div class="docs-container">
			{@render children()}
		</div>
	</main>
</div>

<style>
	.docs-layout {
		display: flex;
		min-height: 100vh;
		background: var(--wow-bg);
	}

	.docs-sidebar {
		width: 280px;
		background: rgba(5, 7, 10, 0.95);
		border-right: 1px solid rgba(255, 255, 255, 0.08);
		display: flex;
		flex-direction: column;
		flex-shrink: 0;
		position: sticky;
		top: 0;
		height: 100vh;
		overflow-y: auto;
	}

	.sidebar-header {
		padding: 1.5rem;
		border-bottom: 1px solid rgba(255, 255, 255, 0.08);
	}

	.logo {
		font-family: var(--wow-font-display);
		font-size: 1.25rem;
		font-weight: 800;
		color: var(--wow-cyan);
		letter-spacing: 0.05em;
		text-transform: uppercase;
		text-decoration: none;
		display: block;
	}

	.logo:hover {
		text-shadow: 0 0 10px var(--wow-cyan);
	}

	.version {
		font-size: 0.75rem;
		color: var(--wow-text-muted);
		margin-top: 4px;
		font-family: monospace;
	}

	.nav-scroller {
		flex: 1;
		overflow-y: auto;
		padding: 1.5rem 0;
	}

	.nav-group {
		margin-bottom: 2rem;
	}

	.nav-title {
		padding: 0 1.5rem;
		font-size: 0.7rem;
		text-transform: uppercase;
		letter-spacing: 0.1em;
		color: var(--wow-text-muted);
		font-weight: 600;
		margin-bottom: 0.75rem;
		font-family: var(--wow-font-display);
	}

	.nav-item {
		display: block;
		padding: 0.5rem 1.5rem;
		color: var(--wow-text-body);
		text-decoration: none;
		font-size: 0.9rem;
		border-left: 2px solid transparent;
		transition: all 0.2s;
	}

	.nav-item:hover,
	.nav-item.active {
		background: rgba(0, 243, 255, 0.05);
		color: var(--wow-cyan);
		border-left-color: var(--wow-cyan);
	}

	.sidebar-footer {
		padding: 1.5rem;
		border-top: 1px solid rgba(255, 255, 255, 0.08);
	}

	.back-link {
		font-size: 0.85rem;
		color: var(--wow-text-muted);
		text-decoration: none;
		transition: color 0.2s;
	}

	.back-link:hover {
		color: var(--wow-cyan);
	}

	.docs-content {
		flex: 1;
		overflow-y: auto;
		padding: 3rem 4rem;
	}

	.docs-container {
		max-width: 800px;
		margin: 0 auto;
	}

	@media (max-width: 768px) {
		.docs-layout {
			flex-direction: column;
		}

		.docs-sidebar {
			width: 100%;
			height: auto;
			position: relative;
		}

		.docs-content {
			padding: 2rem 1.5rem;
		}
	}
</style>
