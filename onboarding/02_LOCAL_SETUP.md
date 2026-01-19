# Local Development Setup

Let's get The League running on your machine. This should take about 30 minutes.

---

## Prerequisites

### Required Software

| Software | Version | Installation |
|----------|---------|--------------|
| .NET SDK | 8.0+ | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) |
| Node.js | 20 LTS | [Download](https://nodejs.org/) |
| SQL Server LocalDB | 2019+ | Included with Visual Studio, or [standalone](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) |
| Git | 2.40+ | [Download](https://git-scm.com/) |

### Recommended Tools

| Tool | Purpose |
|------|---------|
| Visual Studio 2022 | Backend development (best .NET experience) |
| VS Code | Frontend development (with Angular extensions) |
| Azure Data Studio | Database exploration |
| Postman | API testing |

### VS Code Extensions (if using VS Code)

- Angular Language Service
- Tailwind CSS IntelliSense
- C# Dev Kit
- REST Client

---

## Step 1: Clone the Repository

```bash
git clone https://github.com/[your-org]/LeagueMembershipManagementPortal.git
cd LeagueMembershipManagementPortal
```

---

## Step 2: Backend Setup

### 2.1 Restore NuGet Packages

```bash
dotnet restore
```

### 2.2 Verify Configuration

Check `TheLeague.Api/appsettings.json` - defaults should work for local development:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TheLeagueDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Secret": "TheLeagueSecretKey2024VeryLongAndSecureForJWTTokens!",
    "Issuer": "TheLeague",
    "Audience": "TheLeagueApp"
  }
}
```

**Note:** LocalDB is installed with Visual Studio. If you don't have it:
```bash
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
```

### 2.3 Apply Database Migrations

```bash
cd TheLeague.Infrastructure
dotnet ef database update -s ../TheLeague.Api
```

This creates the database and applies all migrations. The seeder runs automatically on first startup.

### 2.4 Run the Backend

```bash
cd ../TheLeague.Api
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:7000
```

### 2.5 Verify Backend is Running

Open in browser: http://localhost:7000/swagger

You should see the Swagger UI with all API endpoints.

---

## Step 3: Frontend Setup

Open a **new terminal** (keep backend running):

### 3.1 Install Dependencies

```bash
cd the-league-client
npm install
```

This takes a few minutes on first run.

### 3.2 Run the Frontend

```bash
npm start
```

You should see:
```
Local: http://localhost:4200/
```

### 3.3 Verify Frontend is Running

Open in browser: http://localhost:4200

You should see the login page.

---

## Step 4: Smoke Test

### Test Login

Use these credentials:

| Role | Email | Password |
|------|-------|----------|
| Super Admin | admin@theleague.com | Admin123! |
| Club Manager | chairman@teddingtoncc.com | Chairman123! |
| Member | james.anderson@email.com | Member123! |

### What to Check

1. **Super Admin Login** → Should see Admin Dashboard with club list
2. **Club Manager Login** → Should see Club Dashboard with member stats
3. **Member Login** → Should see Portal Dashboard with upcoming sessions

If all three work, you're good to go!

---

## Common Setup Issues

### Issue: "Cannot connect to LocalDB"

**Symptoms:** Database connection errors on startup

**Fix:**
```bash
# Check LocalDB is installed
sqllocaldb info

# Create instance if missing
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
```

### Issue: "Port 7000 already in use"

**Symptoms:** Backend won't start

**Fix:**
```bash
# Find process using port
netstat -ano | findstr :7000

# Kill it (replace PID)
taskkill /PID <PID> /F
```

Or change port in `appsettings.json`:
```json
"Kestrel": {
  "Endpoints": {
    "Http": { "Url": "http://localhost:7001" }
  }
}
```

Then update `the-league-client/src/environments/environment.ts`:
```typescript
apiUrl: 'http://localhost:7001/api'
```

### Issue: "npm install fails"

**Symptoms:** Node module installation errors

**Fix:**
```bash
# Clear npm cache
npm cache clean --force

# Delete node_modules and reinstall
rm -rf node_modules package-lock.json
npm install
```

### Issue: "CORS errors in browser console"

**Symptoms:** API calls fail with CORS errors

**Fix:** Make sure backend is running on port 7000. CORS is configured to allow `http://localhost:4200`.

### Issue: "Migration failed"

**Symptoms:** `dotnet ef database update` fails

**Fix:**
```bash
# Drop database and recreate
cd TheLeague.Infrastructure
dotnet ef database drop -s ../TheLeague.Api --force
dotnet ef database update -s ../TheLeague.Api
```

---

## Development Workflow

### Running Both Services

You need **two terminals**:

**Terminal 1 (Backend):**
```bash
cd TheLeague.Api
dotnet watch run  # Hot reload enabled
```

**Terminal 2 (Frontend):**
```bash
cd the-league-client
npm start  # Hot reload enabled by default
```

### Making Changes

- **Backend changes:** `dotnet watch` auto-recompiles
- **Frontend changes:** Angular CLI auto-refreshes browser
- **Database schema changes:** Create a new migration (see [05_DATABASE_GUIDE.md](./05_DATABASE_GUIDE.md))

---

## Useful Commands Cheat Sheet

```bash
# Backend
dotnet build                          # Build solution
dotnet run                            # Run API
dotnet watch run                      # Run with hot reload
dotnet test                           # Run unit tests

# Database
dotnet ef migrations add <Name> -s ../TheLeague.Api    # New migration
dotnet ef database update -s ../TheLeague.Api          # Apply migrations
dotnet ef database drop -s ../TheLeague.Api --force    # Reset database

# Frontend
npm start                             # Dev server
npm run build                         # Production build
npm test                              # Unit tests
npm run e2e                           # E2E tests (Playwright)
```

---

## IDE Setup Tips

### Visual Studio 2022

1. Open `LeagueMembershipManagementPortal.sln`
2. Set `TheLeague.Api` as startup project
3. Press F5 to run with debugger

### VS Code

1. Open folder in VS Code
2. Install recommended extensions when prompted
3. Use integrated terminal for commands
4. Use "Run and Debug" panel for debugging

---

## Next Steps

Now that you're running locally:

→ [03_ARCHITECTURE_OVERVIEW.md](./03_ARCHITECTURE_OVERVIEW.md) - Understand the system design
