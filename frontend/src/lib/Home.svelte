<script lang="ts">
	import { onMount } from 'svelte';
	import { resolveRoute } from '$app/paths';
	import Mission from './landing/Mission.svelte';
	import Features from './landing/Features.svelte';
	import FAQ from './landing/FAQ.svelte';
	import Background from './components/Background.svelte';
	import CyberBackground from './components/CyberBackground.svelte';

	const sections = [
		{
			title: 'RECRUITING',
			description: 'Identify elite operatives. Assess candidates for Protocol compatibility.',
			icon: 'M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z M12 8v4 M12 16h.01',
			image: '/recruiting_prism.png',
			path: '/recruiting' as const,
			color: 'var(--wow-cyan)',
			bg: 'linear-gradient(45deg, #0b1218, #1a2633)'
		},
		{
			title: 'LIVE OPS',
			description: 'Monitor real-time conflict zones. Deploy directives to field agents.',
			icon: 'M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 17.93c-3.95-.49-7-3.85-7-7.93 0-.62.08-1.21.21-1.79L9 15v1c0 1.1.9 2 2 2v1.93zm6.9-2.54c-.26-.81-1-1.39-1.9-1.39h-1v-3c0-.55-.45-1-1-1H8v-2h2c.55 0 1-.45 1-1V7h2c1.1 0 2-.9 2-2v-.41c2.93 1.19 5 4.06 5 7.41 0 2.08-1.07 3.9-2.2 5.06z',
			image: '/live_ops_prism.png',
			path: '/live' as const,
			color: '#ff3e3e',
			bg: 'linear-gradient(45deg, #2a0a0a, #4a1a1a)'
		},
		{
			title: 'ARCHIVES',
			description: 'Access classified historical data. Analyze recovery logs and artifact intel.',
			icon: 'M4 19.5A2.5 2.5 0 0 1 6.5 17H20M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z',
			image: '/archives_prism.png',
			path: '/archives' as const,
			color: '#a0b6c5',
			bg: 'linear-gradient(45deg, #0f171f, #232d36)'
		}
	];

	let visible = false;
	onMount(() => {
		setTimeout(() => (visible = true), 100);
	});

	// Text Scramble Effect
	const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()';
	function scramble(node: HTMLElement) {
		const text = node.innerText;
		const original = text;
		let iterations = 0;
		let interval: ReturnType<typeof setInterval> | null = null;

		node.onmouseenter = () => {
			if (interval) clearInterval(interval);
			iterations = 0;

			interval = setInterval(() => {
				node.innerText = text
					.split('')
					.map((char: string, index: number) => {
						if (index < iterations) return original[index];
						return chars[Math.floor(Math.random() * chars.length)];
					})
					.join('');

				if (iterations >= original.length && interval) clearInterval(interval);
				iterations += 1 / 2;
			}, 30);
		};
	}
</script>

<div class="landing-container" class:visible>
	<!-- FIXED DRONE BACKGROUND (Visually sits behind everything) -->
	<div class="fixed-hero-bg">
		<Background mode="HOME" />
	</div>

	<!-- HERO SECTION (Transparent, scrolls over fixed bg) -->
	<section class="hero full-height">
		<div class="hero-content">
			<h1 class="hero-title">
				JOIN THE <span class="highlight">RESISTANCE</span>
			</h1>
			<p class="hero-sub">The Protocol Initialized. Ascend the Ranks.</p>

			<a href={resolve('/recruiting')} class="cta-button">
				<span class="cta-text">ENTER PROTOCOL</span>
				<div class="liquid"></div>
			</a>

			<div class="hero-decoration"></div>
		</div>

		<div class="scroll-indicator">
			<div class="mouse">
				<div class="wheel"></div>
			</div>
			<div class="arrow"></div>
		</div>
	</section>

	<!-- CONTENT LAYER (Solid BG, scrolls over drone) -->
	<div class="content-layer">
		<!-- Global background for non-hero sections -->
		<CyberBackground />

		<!-- NAVIGATION GRID (As features/modules) -->
		<section class="modules-section">
			<div class="modules-header">
				<h3 class="section-label">OPERATIONAL MODULES</h3>
				<div class="line"></div>
			</div>

			<div class="paths-grid">
				{#each sections as section (section.path)}
					<a
						href={resolve(section.path)}
						class="path-card"
						style="--accent: {section.color}; --bg-grad: {section.bg};"
					>
						<!-- Background Image -->
						<img src={section.image} alt={section.title} class="card-bg" loading="lazy" />
						<div class="card-overlay"></div>

						<!-- Content -->
						<div class="card-content">
							<!-- Scramble Effect Applied Here -->
							<h2 class="path-title" use:scramble>
								<!-- Icon Inline -->
								<svg
									class="title-icon"
									viewBox="0 0 24 24"
									fill="none"
									stroke="currentColor"
									stroke-width="2"
								>
									<path d={section.icon} />
								</svg>
								{section.title}
							</h2>
							<p class="path-desc">{section.description}</p>
						</div>

						<!-- Interactive Elements -->
						<div class="card-hud"></div>
					</a>
				{/each}
			</div>
		</section>

		<!-- NEW MARKETING SECTIONS -->
		<Mission />
		<Features />
		<FAQ />
	</div>
</div>
