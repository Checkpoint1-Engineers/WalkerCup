# Walker Tournament Platform

A tournament management system for competitive gaming. Backend runs on .NET 9, frontend is built with SvelteKit.

## What it does

WalkerCup handles the full tournament lifecycle—creating events, managing player registration, generating brackets, tracking matches, and crowning winners. The backend exposes a REST API, and the frontend provides a dark-themed UI with some visual flair.

## Tech stack

**Backend**
- .NET 9 Web API
- Entity Framework Core with SQLite
- JWT authentication
- BCrypt password hashing
- Serilog for logging

**Frontend**
- SvelteKit with Svelte 5
- TypeScript
- Vitest + Playwright for testing
- Vite for builds

## Getting started

### Backend

```bash
cd backend/src/WalkerTournament.Api

# Set up JWT secret (required)
dotnet user-secrets init
dotnet user-secrets set "Jwt:SecretKey" "your-secret-key-at-least-32-characters"

# Run migrations
dotnet ef database update

# Start the server
dotnet run
```

API runs at `https://localhost:5001`. Swagger docs at `/swagger`.

### Frontend

```bash
cd frontend

pnpm install
pnpm dev
```

Runs at `http://localhost:5173`.

If the API is somewhere other than `localhost:5001`, update the base URL in `src/lib/api.ts`.

## Project layout

```
├── backend/
│   └── src/WalkerTournament.Api/
│       ├── Controllers/    # API endpoints
│       ├── Data/           # EF Core context + migrations
│       ├── Models/         # Domain entities
│       ├── DTOs/           # Request/response types
│       └── Services/       # Business logic
│
├── frontend/
│   └── src/
│       ├── routes/         # SvelteKit pages
│       │   ├── +layout.svelte
│       │   ├── +page.svelte
│       │   ├── [slug]/     # Tournament detail page
│       │   ├── live/
│       │   ├── archives/
│       │   └── recruiting/
│       └── lib/
│           ├── components/ # Reusable UI bits
│           ├── landing/    # Homepage sections
│           └── api.ts      # API client
│
└── docs/                   # API documentation
```

## Default login

A superadmin account is seeded on first run:

- **Email:** `superadmin@walker.com`
- **Password:** `SuperAdmin123!`

Change this before deploying anywhere public.

## Running tests

```bash
# Backend
cd backend/tests
dotnet test

# Frontend
cd frontend
pnpm test          # unit + e2e
pnpm test:unit     # just vitest
pnpm test:e2e      # just playwright
```

## Configuration

### Backend (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=walkertournament.db"
  },
  "Jwt": {
    "Issuer": "WalkerTournamentAPI",
    "Audience": "WalkerTournamentClients",
    "ExpiryMinutes": 60
  }
}
```

JWT secret goes in user secrets, not the config file.

### Frontend

API base URL is set in `src/lib/api.ts`. CORS on the backend is configured for `http://localhost:5173` by default.

## Deployment

**Backend:** Build with `dotnet publish -c Release`, deploy the output to your host of choice. Set the JWT secret via environment variables or a secrets manager.

**Frontend:** Run `pnpm build`, deploy the `.svelte-kit` output to any static host (Vercel, Netlify, etc.) or use an adapter for node/cloudflare/etc.

## Troubleshooting

**"JWT secret not configured"** — Run the `dotnet user-secrets set` command shown above.

**"Unable to connect to database"** — Make sure you ran `dotnet ef database update`.

**"CORS errors"** — Check that the backend's CORS config includes your frontend URL.

**"Cannot connect to API"** — Verify the backend is running and the URL in `api.ts` is correct.

## License

MIT

---

Built by Checkpoint1 Engineers
