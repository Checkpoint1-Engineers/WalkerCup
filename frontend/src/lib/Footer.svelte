<script lang="ts">
	import { resolve } from '$app/paths';
	const year = new Date().getFullYear();
</script>

<footer class="site-footer">
	<div class="footer-content">
		<div class="footer-brand">
			<h3 class="footer-logo">WORLD OF WALKER</h3>
			<p class="footer-tagline">The premiere competitive platform for the Walker community.</p>
		</div>

		<div class="footer-links">
			<div class="col">
				<h4>PLATFORM</h4>
				<a href={resolve('/')}>Home</a>
				<a href={resolve('/recruiting')}>Recruiting</a>
				<a href={resolve('/live')}>Live Ops</a>
			</div>
			<div class="col">
				<h4>COMMUNITY</h4>
				<a href="#">Discord</a>
				<a href="#">Twitter/X</a>
				<a href="#">Instagram</a>
			</div>
			<div class="col">
				<h4>LEGAL</h4>
				<a href="#">Privacy Policy</a>
				<a href="#">Terms of Service</a>
			</div>
		</div>
	</div>

	<div class="footer-bottom">
		<p>&copy; {year} WALKER GAMING. ALL RIGHTS RESERVED.</p>
		<p class="credits">SYSTEM STATUS: <span class="online">ONLINE</span></p>
	</div>
</footer>

<style>
	.site-footer {
		background: #020b14;
		border-top: 1px solid rgba(255, 255, 255, 0.05);
		padding: 4rem 2rem 2rem;
		margin-top: auto;
		font-family: var(--wow-font-body);
	}

	.footer-content {
		max-width: 1400px;
		margin: 0 auto;
		display: flex;
		flex-wrap: wrap;
		gap: 4rem;
		justify-content: space-between;
		margin-bottom: 4rem;
	}

	.footer-brand {
		max-width: 300px;
	}
	.footer-logo {
		font-family: var(--wow-font-display);
		color: #fff;
		font-size: 1.5rem;
		letter-spacing: 2px;
		margin-bottom: 1rem;
	}
	.footer-tagline {
		color: var(--wow-text-body);
		font-size: 0.9rem;
		line-height: 1.6;
	}

	.footer-links {
		display: flex;
		gap: 3rem;
		flex-wrap: wrap;
	}
	.col {
		display: flex;
		flex-direction: column;
		gap: 0.5rem;
	}
	.col h4 {
		color: var(--wow-cyan);
		font-size: 0.8rem;
		letter-spacing: 1px;
		margin-bottom: 0.5rem;
	}
	.col a {
		color: var(--wow-text-muted);
		font-size: 0.9rem;
		transition: color 0.2s;
	}
	.col a:hover {
		color: #fff;
	}

	.footer-bottom {
		max-width: 1400px;
		margin: 0 auto;
		border-top: 1px solid rgba(255, 255, 255, 0.05);
		padding-top: 2rem;
		display: flex;
		justify-content: space-between;
		color: var(--wow-text-muted);
		font-size: 0.75rem;
		letter-spacing: 1px;
	}

	.online {
		color: var(--wow-cyan);
		text-shadow: 0 0 5px var(--wow-cyan);
	}
</style>
