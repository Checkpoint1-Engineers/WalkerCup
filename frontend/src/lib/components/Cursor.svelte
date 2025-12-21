<script>
	import { onMount } from 'svelte';
	import { spring } from 'svelte/motion';

	let coords = spring(
		{ x: -100, y: -100 },
		{
			stiffness: 0.15,
			damping: 0.5
		}
	);

	let size = spring(30);
	let isHovering = false;
	let isVisible = false;

	onMount(() => {
		// Hide on mobile/touch
		if (window.matchMedia('(pointer: coarse)').matches) return;

		isVisible = true;

		const move = (e) => {
			coords.set({ x: e.clientX, y: e.clientY });
		};

		const mousedown = () => size.set(20);
		const mouseup = () => size.set(isHovering ? 60 : 30);

		const checkHover = (e) => {
			const target = e.target;
			if (
				target.tagName === 'A' ||
				target.tagName === 'BUTTON' ||
				target.closest('a') ||
				target.closest('button')
			) {
				isHovering = true;
				size.set(60);
			} else {
				isHovering = false;
				size.set(30);
			}
		};

		window.addEventListener('mousemove', move);
		window.addEventListener('mousedown', mousedown);
		window.addEventListener('mouseup', mouseup);
		window.addEventListener('mouseover', checkHover);

		return () => {
			window.removeEventListener('mousemove', move);
			window.removeEventListener('mousedown', mousedown);
			window.removeEventListener('mouseup', mouseup);
			window.removeEventListener('mouseover', checkHover);
		};
	});
</script>

{#if isVisible}
	<div
		class="cursor-reticle"
		class:hover={isHovering}
		style="transform: translate({$coords.x}px, {$coords.y}px); width: {$size}px; height: {$size}px;"
	>
		<div class="crosshair"></div>
		<div class="shards"></div>
	</div>
{/if}

<style>
	.cursor-reticle {
		position: fixed;
		top: 0;
		left: 0; /* JS sets x/y */
		pointer-events: none;
		z-index: 9999;
		border: 1px solid var(--wow-cyan);
		border-radius: 50%;

		/* Centering trick: translate -50% -50% */
		margin-top: 0;
		margin-left: 0;
		translate: -50% -50%;

		display: flex;
		align-items: center;
		justify-content: center;
		mix-blend-mode: exclusion;
		box-shadow: 0 0 10px rgba(0, 243, 255, 0.3);
		background: rgba(0, 243, 255, 0.05);
		transition:
			border-color 0.2s,
			width 0.2s,
			height 0.2s;
		will-change: transform, width, height;
	}

	.crosshair {
		width: 4px;
		height: 4px;
		background: var(--wow-cyan);
		border-radius: 50%;
	}

	.cursor-reticle.hover {
		border-color: #fff;
		background: rgba(255, 255, 255, 0.1);
		mix-blend-mode: normal;
	}
</style>
