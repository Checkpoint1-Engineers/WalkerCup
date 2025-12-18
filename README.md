# Walker Tournament Platform 🎮

A comprehensive tournament management platform for competitive gaming, featuring robust backend APIs and an interactive Svelte frontend.

## 📋 Overview

The Walker Tournament Platform (WalkerCup) is a full-stack application designed to manage high-stakes gaming tournaments. It orchestrates the complete lifecycle of combat events, from player registration to bracket generation, match resolution, and championship determination. The system ensures data integrity through optimistic concurrency control and delivers real-time tournament progression.

## ✨ Features

### Tournament Management
- **Complete Lifecycle Management**: Draft creation, registration, bracket generation, and match resolution
- **Automated Bracket System**: Automatic bracket generation and winner progression
- **Multiple Formats**: Support for 1v1, 5v5, and custom tournament formats
- **XP & Rewards**: Configurable experience points and reward distribution
- **Real-time Updates**: Live tournament status and match progression




### Security & Authentication
- **JWT Authentication**: Secure token-based authentication
- **Password Management**: BCrypt hashing with reset functionality
- **Role-Based Authorization**: Granular permission control
- **User Secrets**: Secure configuration management

### Interactive Frontend
- **Modern UI/UX**: Sleek, responsive design with glassmorphism effects
- **Interactive Animations**: Micro-interactions and smooth transitions
- **Dark Mode**: Premium dark theme aesthetic

## 🛠️ Technology Stack

### Backend
- **.NET 9.0**: Modern ASP.NET Core Web API
- **Entity Framework Core**: ORM with SQLite database
- **JWT Authentication**: Secure token-based auth
- **BCrypt.Net**: Password hashing
- **Serilog**: Structured logging
- **Swagger/OpenAPI**: API documentation

### Frontend
- **Svelte 5**: Reactive UI framework
- **Vite**: Fast build tool and dev server
- **Vanilla CSS**: Custom styling with modern design patterns
- **SPA Router**: Client-side routing

## 📦 Prerequisites

- **.NET 9.0 SDK** or later
- **Node.js** 18+ and npm
- **Git** for version control
- **SQLite** (included with .NET)

## 🚀 Getting Started

### Backend Setup

1. **Navigate to the backend directory**
   ```bash
   cd backend/src/WalkerTournament.Api
   ```

2. **Configure JWT Secret**
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "Jwt:SecretKey" "your-super-secret-key-minimum-32-characters-long"
   ```

3. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the Backend**
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` (or `http://localhost:5000`)

5. **Access API Documentation**
   - Swagger UI: `https://localhost:5001/swagger`

### Frontend Setup

1. **Navigate to the frontend directory**
   ```bash
   cd frontend
   ```

2. **Install Dependencies**
   ```bash
   npm install
   ```

3. **Configure API Endpoint**
   
   Edit `src/lib/api.js` to set the correct backend URL:
   ```javascript
   const API_BASE_URL = 'https://localhost:5001';
   ```

4. **Run the Development Server**
   ```bash
   npm run dev
   ```

   The frontend will be available at `http://localhost:5173`

## ⚙️ Configuration

### Backend Configuration (`appsettings.json`)

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

**Important**: The `Jwt:SecretKey` is stored in user secrets, not in `appsettings.json`, for security.

### Frontend Configuration (`src/lib/api.js`)

Update the `API_BASE_URL` constant to match your backend URL:
- Development: `https://localhost:5001`
- Production: Your deployed API URL

## 📁 Project Structure

```
Walker Tournament/
├── backend/
│   ├── src/
│   │   └── WalkerTournament.Api/
│   │       ├── Controllers/       # API endpoints
│   │       ├── Data/              # DbContext and migrations
│   │       ├── Models/            # Entity models
│   │       ├── DTOs/              # Data transfer objects
│   │       ├── Services/          # Business logic
│   │       └── Program.cs         # Application entry point
│   ├── tests/                     # Unit and integration tests
│   └── WalkerTournament.sln       # Solution file
├── frontend/
│   ├── src/
│   │   ├── lib/
│   │   │   ├── api.js            # API client
│   │   │   └── router.js         # SPA routing
│   │   ├── components/           # Svelte components
│   │   ├── pages/                # Page components
│   │   ├── App.svelte            # Root component
│   │   └── main.js               # Entry point
│   ├── public/                   # Static assets
│   └── package.json              # Dependencies
├── docs/                         # API documentation website
└── README.md                     # This file
```

## 🔐 Default Credentials

The system seeds a SuperAdmin account on first run:

- **Email**: `superadmin@walker.com`
- **Password**: `SuperAdmin123!`

**Important**: Change these credentials immediately in production!

## 🎯 Core Workflows

### 1. Tournament Creation & Management
1. **Draft**: Admin creates a tournament and configures settings (format, XP rewards, deadlines)
2. **Registration**: Players join via the open registration endpoint
3. **Lock & Draw**: Admin locks registration and triggers bracket generation
4. **Combat**: Admin resolves matches by declaring winners; system auto-promotes winners
5. **Victory**: Final match determines the champion, awarding XP and closing the event



## 🧪 Testing

### Backend Tests
```bash
cd backend/tests
dotnet test
```

### Frontend Testing
```bash
cd frontend
npm run build  # Verify build succeeds
```

## 🌐 API Documentation

Comprehensive API documentation is available in the `/docs` folder. Once the backend is running, access:

- **Interactive Docs**: View `/docs/index.html` in a browser
- **Swagger UI**: `https://localhost:5001/swagger`

## 🔧 Troubleshooting

### Backend Issues

**Problem**: "Unable to connect to database"
- **Solution**: Ensure migrations are applied: `dotnet ef database update`

**Problem**: "JWT secret not configured"
- **Solution**: Set user secret: `dotnet user-secrets set "Jwt:SecretKey" "your-key"`

**Problem**: "Port already in use"
- **Solution**: Change port in `Properties/launchSettings.json`

### Frontend Issues

**Problem**: "Cannot connect to API"
- **Solution**: Verify backend is running and `API_BASE_URL` in `api.js` is correct

**Problem**: "CORS errors"
- **Solution**: Backend CORS is configured for `http://localhost:5173` (Vite default)

**Problem**: "npm install fails"
- **Solution**: Ensure Node.js 18+ is installed, delete `node_modules` and try again

## 🚀 Production Deployment

### Backend
1. Set production connection string in `appsettings.Production.json`
2. Configure JWT secret via environment variable or secure secret management
3. Build: `dotnet build -c Release`
4. Publish: `dotnet publish -c Release -o ./publish`
5. Deploy to your hosting platform (Azure, AWS, etc.)

### Frontend
1. Update `API_BASE_URL` in `src/lib/api.js` to production API URL
2. Build: `npm run build`
3. Deploy `dist/` folder to static hosting (Netlify, Vercel, Azure Static Web Apps, etc.)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Team

Developed by **Checkpoint1 Engineers**

## 📧 Support

For issues, questions, or contributions, please open an issue on GitHub.

---

**⚡ Walker Tournament Platform** - Where Champions Are Made
