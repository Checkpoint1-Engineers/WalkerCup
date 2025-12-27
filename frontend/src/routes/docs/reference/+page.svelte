<script lang="ts">
</script>

<svelte:head>
	<title>API Reference | Walker API Docs</title>
</svelte:head>

<h1>API Reference</h1>

<p>All endpoints. Protected endpoints require a Bearer Token.</p>

<h2>Authentication</h2>

<div class="info-panel">
	<p><span class="badge post">POST</span> <code>/api/auth/login</code></p>
	<p>Authenticates user credentials.</p>
	<p><strong>Body:</strong> <code>&#123; "email": "...", "password": "..." &#125;</code></p>
	<p><strong>Returns:</strong> <code>&#123; "token": "jwt...", "expiration": "..." &#125;</code></p>
</div>

<h2>Tournaments</h2>

<div class="info-panel">
	<p><span class="badge post">POST</span> <code>/api/tournaments</code> <em>(Admin)</em></p>
	<p>Create a new tournament.</p>
	<p>
		<strong>Body:</strong>
		<code>&#123; "name": "...", "joinDeadline": "iso-date", "xpPerWin": 100 &#125;</code>
	</p>
</div>

<div class="info-panel">
	<p><span class="badge get">GET</span> <code>/api/tournaments</code> <em>(Public)</em></p>
	<p>List all tournaments.</p>
</div>

<div class="info-panel">
	<p>
		<span class="badge get">GET</span> <code>/api/tournaments/&#123;id&#125;</code>
		<em>(Public)</em>
	</p>
	<p>Get tournament details including members and matches.</p>
</div>

<div class="info-panel">
	<p>
		<span class="badge post">POST</span> <code>/api/tournaments/&#123;id&#125;/open</code>
		<em>(Admin)</em>
	</p>
	<p>Open tournament for registration.</p>
</div>

<div class="info-panel">
	<p>
		<span class="badge post">POST</span> <code>/api/tournaments/&#123;id&#125;/join</code>
		<em>(Public, Rate Limited)</em>
	</p>
	<p>Register a walker for the tournament.</p>
	<p><strong>Body:</strong> <code>&#123; "walkerId": 123, "walkerName": "Name" &#125;</code></p>
</div>

<div class="info-panel">
	<p>
		<span class="badge post">POST</span> <code>/api/tournaments/&#123;id&#125;/extend</code>
		<em>(Admin)</em>
	</p>
	<p>Extend the registration deadline.</p>
	<p><strong>Body:</strong> <code>&#123; "newDeadline": "iso-date" &#125;</code></p>
</div>

<div class="info-panel">
	<p>
		<span class="badge post">POST</span> <code>/api/tournaments/&#123;id&#125;/lock</code>
		<em>(Admin)</em>
	</p>
	<p>Lock tournament, closing registration.</p>
</div>

<div class="info-panel">
	<p>
		<span class="badge post">POST</span> <code>/api/tournaments/&#123;id&#125;/draw</code>
		<em>(Admin)</em>
	</p>
	<p>Generate the tournament bracket.</p>
</div>

<h2>Matches</h2>

<div class="info-panel">
	<p>
		<span class="badge post">POST</span> <code>/api/matches/&#123;id&#125;/winner</code>
		<em>(Admin)</em>
	</p>
	<p>Set the winner of a match. Triggers auto-advancement to the next round.</p>
	<p><strong>Body:</strong> <code>&#123; "winnerId": "guid" &#125;</code></p>
</div>

<h2>Health</h2>

<div class="info-panel">
	<p><span class="badge get">GET</span> <code>/api/health</code> <em>(Public)</em></p>
	<p>Health check endpoint.</p>
</div>

<h2>Status Codes</h2>

<ul>
	<li><code>200</code> — Success</li>
	<li><code>201</code> — Created</li>
	<li><code>400</code> — Bad Request (validation error)</li>
	<li><code>401</code> — Unauthorized (missing/invalid token)</li>
	<li><code>403</code> — Forbidden (insufficient permissions)</li>
	<li><code>404</code> — Not Found</li>
	<li><code>409</code> — Conflict (concurrency error)</li>
	<li><code>429</code> — Too Many Requests (rate limited)</li>
</ul>

<h2>Rate Limits</h2>

<ul>
	<li><code>/api/auth/login</code> — 5 requests per minute</li>
	<li><code>/api/tournaments/&#123;id&#125;/join</code> — 10 requests per minute</li>
</ul>
