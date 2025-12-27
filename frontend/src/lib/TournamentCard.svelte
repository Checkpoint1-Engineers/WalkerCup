<script lang="ts">
	import { spring } from 'svelte/motion';

	export let id: string = '0000';
	export let title: string = '';
	export let imageUrl: string = '';
	export let status: number = 0;
	export let joinDeadline: string = '';

	// Format Date
	$: deadlineDate = new Date(joinDeadline).toLocaleDateString('en-US', {
		month: 'short',
		day: 'numeric'
	});

	// Generate Lore Data
	$: serialId = `OP-${(id || 'XXXX').toString().slice(0, 4).toUpperCase()}`;
	$: priorityLevel = status === 3 ? 'CRITICAL' : 'STANDARD';
	const region = 'GLOBAL';

	// Status Logic (reactive)
	$: statusText =
		status === 1 ? 'RECRUITING' : status === 3 ? 'LIVE OPS' : status === 4 ? 'ARCHIVED' : 'OPEN';
	$: accentColor =
		status === 1
			? 'var(--wow-cyan)'
			: status === 3
				? '#ff3e3e'
				: status === 4
					? '#a0b6c5'
					: 'var(--wow-cyan)';

	// 3D TILT LOGIC
	let rx = spring(0, { stiffness: 0.1, damping: 0.5 });
	let ry = spring(0, { stiffness: 0.1, damping: 0.5 });

	function handleMouseMove(e: MouseEvent) {
		const rect = (e.currentTarget as HTMLElement).getBoundingClientRect();
		const x = e.clientX - rect.left;
		const y = e.clientY - rect.top;

		const xPct = x / rect.width - 0.5;
		const yPct = y / rect.height - 0.5;

		ry.set(xPct * 15);
		rx.set(yPct * -15);
	}

	function handleMouseLeave() {
		rx.set(0);
		ry.set(0);
	}
</script>

<a
	href={'/' + encodeURIComponent(title)}
	class="game-card"
	onmousemove={handleMouseMove}
	onmouseleave={handleMouseLeave}
	style="--accent: {accentColor}; --rx: {$rx}deg; --ry: {$ry}deg"
>
	<!-- Border Flow -->
	<div class="border-gradient"></div>

	<!-- Inner Plate -->
	<div class="card-inner">
		<!-- Visual Texture overlay -->
		<div class="texture-overlay"></div>

		<!-- Image Content -->
		<div class="card-media">
			{#if imageUrl}
				<img src={imageUrl} alt={title} />
			{:else}
				<div class="placeholder"></div>
			{/if}
			<div class="media-overlay"></div>

			<div class="status-badge">
				<span class="status-dot"></span>
				{statusText}
			</div>

			<div class="serial-tag">{serialId}</div>
		</div>

		<!-- Text Content -->
		<div class="card-content">
			<h3 class="card-title">{title}</h3>

			<!-- Tech Specs Grid -->
			<div class="tech-grid">
				<div class="tech-item">
					<span class="tech-label">PRIORITY</span>
					<span class="tech-val" class:alert={status === 3}>{priorityLevel}</span>
				</div>
				<div class="tech-item">
					<span class="tech-label">REGION</span>
					<span class="tech-val">{region}</span>
				</div>
				<div class="tech-item">
					<span class="tech-label">SQUAD</span>
					<span class="tech-val">4-MAN</span>
				</div>
				<div class="tech-item">
					<span class="tech-label">ACCESS</span>
					<span class="tech-val">CLEARED</span>
				</div>
			</div>

			<div class="divider"></div>

			<div class="meta-row">
				<div class="meta-item">
					<span class="label">DEADLINE</span>
					<span class="value">{deadlineDate}</span>
				</div>
				<div class="action-btn">
					INITIALIZE <span class="arrow">›</span>
				</div>
			</div>
		</div>
	</div>
</a>
