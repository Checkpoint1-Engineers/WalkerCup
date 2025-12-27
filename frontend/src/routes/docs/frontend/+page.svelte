<script lang="ts">
	const loginCode = `const login = async (email: string, password: string) => {
  const res = await fetch('http://localhost:5270/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });

  const data = await res.json();
  if (data.token) {
    localStorage.setItem('authToken', data.token);
  }
};`;

	const fetchCode = `const secureFetch = async (endpoint: string, options: RequestInit = {}) => {
  const token = localStorage.getItem('authToken');

  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...options.headers as Record<string, string>
  };

  if (token) {
    headers['Authorization'] = \`Bearer \${token}\`;
  }

  const response = await fetch(\`http://localhost:5270\${endpoint}\`, {
    ...options,
    headers
  });

  if (response.status === 401) {
    window.location.href = '/login';
  }

  return response;
};`;

	const typesCode = `interface Tournament {
  id: string;
  name: string;
  description?: string;
  status: number; // 0=Draft, 1=Open, 2=Locked, 3=InProgress, 4=Completed
  joinDeadline: string;
  xpPerWin: number;
  maxParticipants?: number;
  imageUrl?: string;
  memberCount: number;
}

interface Match {
  id: string;
  roundNumber: number;
  matchOrder: number;
  participantA?: TournamentMember;
  participantB?: TournamentMember;
  winner?: TournamentMember;
  nextMatchId?: string;
}

interface TournamentMember {
  id: string;
  walkerId: number;
  walkerName: string;
  joinedAt: string;
  eliminatedAt?: string;
  xpEarned: number;
}`;
</script>

<svelte:head>
	<title>Frontend Integration | Walker API Docs</title>
</svelte:head>

<h1>Frontend Integration Guide</h1>

<p>This section details how to connect your SvelteKit frontend to the Walker Tournament API.</p>

<h2>Authentication</h2>

<p>
	The API uses <strong>JWT (JSON Web Tokens)</strong>. The frontend must handle token storage and
	header injection.
</p>

<h3>Step 1: Login</h3>

<p>Send a POST request with email/password to receive a token.</p>

<pre><code>{loginCode}</code></pre>

<h3>Step 2: Authenticated Requests</h3>

<p>Create a wrapper for <code>fetch</code> that automatically injects the token.</p>

<pre><code>{fetchCode}</code></pre>

<h2>TypeScript Interfaces</h2>

<p>Use these definitions to type your frontend state.</p>

<pre><code>{typesCode}</code></pre>
