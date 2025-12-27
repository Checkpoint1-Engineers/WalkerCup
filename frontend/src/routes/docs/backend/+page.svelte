<script lang="ts">
</script>

<svelte:head>
	<title>Backend Architecture | Walker API Docs</title>
</svelte:head>

<h1>Backend Architecture</h1>

<p>For engineers extending the API logic.</p>

<h2>Architecture Patterns</h2>

<div class="info-panel">
	<h3>Clean Architecture</h3>
	<p>Core Domain Entities are isolated from External Frameworks (API, DB).</p>
</div>

<div class="info-panel">
	<h3>Repository Pattern</h3>
	<p><code>MatchRepository</code> handles complex EF Core graph queries.</p>
</div>

<div class="info-panel">
	<h3>Strategy Pattern</h3>
	<p><code>SingleEliminationStrategy</code> encapsulates the bracket creation math.</p>
</div>

<h2>Database Schema</h2>

<p>
	The system uses a self-referencing relationship on the <code>Matches</code> table to build the bracket
	tree.
</p>

<ul>
	<li>
		<strong>Matches Table</strong> — Contains <code>NextMatchId</code> FK pointing to another Match.
	</li>
	<li>
		<strong>Concurrency</strong> — Uses <code>RowVersion</code> (Concurrency Token) to prevent "Lost Update"
		problems when two admins try to update a match simultaneously.
	</li>
</ul>

<h2>Bracket Generation Algorithm</h2>

<p>The <code>DrawAsync</code> method in <code>TournamentService</code> performs the following:</p>

<ol>
	<li>Shuffles registered participants randomly.</li>
	<li>Calculates the nearest power of 2 (e.g., if 6 players, nearest is 8).</li>
	<li>Identifies "Byes" (players who skip Round 1).</li>
	<li>Generates Round 1 matches.</li>
	<li>Recursively generates subsequent rounds (Winners of R1 → Match in R2).</li>
	<li>Saves the entire graph transactionally.</li>
</ol>

<h2>Key Services</h2>

<ul>
	<li><code>TournamentService</code> — Create, open, lock, draw, manage tournaments</li>
	<li><code>MatchService</code> — Set winners, handle auto-advancement</li>
	<li><code>AuthService</code> — JWT token generation, password verification</li>
	<li><code>AuditLogService</code> — Track all admin actions</li>
</ul>
