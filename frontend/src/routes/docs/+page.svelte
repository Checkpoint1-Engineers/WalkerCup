<script lang="ts">
</script>

<svelte:head>
	<title>System Overview | Walker API Docs</title>
</svelte:head>

<h1>System Overview</h1>

<p>
	The Walker Tournament API is the central nervous system for tournament management. It orchestrates
	the lifecycle of combat events, from initial registration to the final championship match.
</p>

<div class="info-panel">
	<h3>Mission Objective</h3>
	<p>
		To provide a robust, secure, and real-time backend for managing high-stakes Walker Bracket
		Tournaments. The system ensures data integrity via optimistic concurrency and delivers automated
		bracket progression.
	</p>
</div>

<h2>Core Workflows</h2>

<h3>The Tournament Loop</h3>

<ol>
	<li>
		<strong>Draft</strong> — Admin creates a tournament. Configures XP rewards and deadlines.
	</li>
	<li>
		<strong>Registration</strong> — Public walkers join via the open endpoint.
	</li>
	<li>
		<strong>Lock & Draw</strong> — Admin locks the tournament (closing registration) and triggers the
		bracket generation engine.
	</li>
	<li>
		<strong>Combat</strong> — Admin resolves matches by declaring winners. The system automatically promotes
		winners to the next round.
	</li>
	<li>
		<strong>Victory</strong> — The final match determines the champion, awarding XP and closing the event.
	</li>
</ol>

<h2>Quick Start</h2>

<div class="info-panel">
	<h3>Default Login</h3>
	<p>A superadmin account is seeded on first run:</p>
	<ul>
		<li><strong>Email:</strong> <code>superadmin@walker.com</code></li>
		<li><strong>Password:</strong> <code>SuperAdmin123!</code></li>
	</ul>
	<p><em>Change this before deploying anywhere public.</em></p>
</div>
