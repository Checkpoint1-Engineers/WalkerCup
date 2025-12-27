<script>
	import { onMount } from 'svelte';

	let visible = false;
	let section;

	onMount(() => {
		const observer = new IntersectionObserver(
			(entries) => {
				if (entries[0].isIntersecting) {
					visible = true;
				}
			},
			{ threshold: 0.3 }
		);
		if (section) observer.observe(section);

		return () => observer.disconnect();
	});
</script>

<section class="mission-section" bind:this={section} class:visible>
	<div class="content">
		<div class="text-col">
			<h2 class="title">
				THE WORLD HAS <span class="highlight">FALLEN</span>.
			</h2>
			<h2 class="title indent">RISE UP.</h2>
			<p class="desc">
				The old world is gone. The corporations have taken everything. But in the shadows, waiting
				for the signal, are the <strong>Walkers</strong>. We are the glitch in their perfect system.
				The anomaly they cannot patch.
			</p>
			<p class="desc">
				Your skills are your weapon. Your code is your ammunition. Join the resistance and reclaim
				the digital frontier.
			</p>
		</div>
		<div class="visual-col">
			<div class="holo-box">
				<div class="scan-line"></div>
				<div class="data-row">TARGET: SYSTEM_CORE</div>
				<div class="data-row">STATUS: VULNERABLE</div>
				<div class="data-row">PROBABILITY: 99.9%</div>
			</div>
		</div>
	</div>
</section>

<style>
	.mission-section {
		padding: 8rem 2rem;
		position: relative;
		max-width: 1400px;
		margin: 0 auto;
		opacity: 0;
		transform: translateY(50px);
		transition:
			opacity 1s ease,
			transform 1s ease;
	}
	.mission-section.visible {
		opacity: 1;
		transform: translateY(0);
	}

	.content {
		display: grid;
		grid-template-columns: 1.5fr 1fr;
		gap: 4rem;
		align-items: center;
	}

	.title {
		font-family: var(--wow-font-display, sans-serif);
		font-size: 4rem;
		line-height: 0.9;
		margin: 0;
		color: #fff;
		text-transform: uppercase;
		font-weight: 900;
	}
	.title.indent {
		margin-left: 4rem;
		color: rgba(255, 255, 255, 0.5);
	}
	.highlight {
		color: var(--wow-cyan, #00f3ff);
		text-shadow: 0 0 20px rgba(0, 243, 255, 0.5);
	}

	.desc {
		font-size: 1.2rem;
		color: #a0a0a0;
		margin-top: 2rem;
		line-height: 1.6;
		max-width: 600px;
		border-left: 2px solid var(--wow-cyan);
		padding-left: 1.5rem;
	}
	.desc strong {
		color: #fff;
	}

	/* Holo Visual */
	.holo-box {
		border: 1px solid rgba(0, 243, 255, 0.3);
		padding: 2rem;
		background: rgba(0, 10, 20, 0.6);
		backdrop-filter: blur(5px);
		position: relative;
		overflow: hidden;
		font-family: monospace;
		color: var(--wow-cyan);
	}
	.holo-box::before {
		content: '';
		position: absolute;
		top: -10px;
		left: -10px;
		width: 20px;
		height: 20px;
		border-top: 2px solid var(--wow-cyan);
		border-left: 2px solid var(--wow-cyan);
	}
	.holo-box::after {
		content: '';
		position: absolute;
		bottom: -10px;
		right: -10px;
		width: 20px;
		height: 20px;
		border-bottom: 2px solid var(--wow-cyan);
		border-right: 2px solid var(--wow-cyan);
	}

	.data-row {
		margin-bottom: 0.5rem;
		opacity: 0.8;
		letter-spacing: 1px;
	}

	.scan-line {
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 2px;
		background: var(--wow-cyan);
		box-shadow: 0 0 10px var(--wow-cyan);
		animation: scan 3s linear infinite;
		opacity: 0.5;
	}

	@keyframes scan {
		0% {
			top: 0;
			opacity: 0;
		}
		10% {
			opacity: 1;
		}
		90% {
			opacity: 1;
		}
		100% {
			top: 100%;
			opacity: 0;
		}
	}

	@media (max-width: 768px) {
		.content {
			grid-template-columns: 1fr;
		}
		.title {
			font-size: 2.5rem;
		}
		.title.indent {
			margin-left: 0;
		}
	}
</style>
