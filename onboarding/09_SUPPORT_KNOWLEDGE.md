# Support Knowledge

Troubleshooting guide and common issues.

---

## Quick Diagnostics

### Is Everything Running?

```bash
# Check API is responding
curl http://localhost:7000/swagger
# Should return HTML

# Check Angular dev server
curl http://localhost:4200
# Should return HTML

# Check database connection (from TheLeague.Api folder)
dotnet ef database update --verbose
# Look for "Applying migration" or "Already up to date"
```

### Health Check Endpoints

| Endpoint | Purpose |
|----------|---------|
| `GET /swagger` | API is running, routes are registered |
| `POST /api/auth/login` | Auth system working |
| `GET /api/auth/me` | JWT validation working |

---

## Common Issues

### Issue: API Won't Start

**Symptom:** `dotnet run` fails or hangs

**Possible Causes:**

1. **Port already in use**
   ```
   System.IO.IOException: Failed to bind to address http://localhost:7000
   ```
   **Fix:** Kill the process using the port
   ```bash
   # Windows
   netstat -ano | findstr :7000
   taskkill /PID <pid> /F

   # Or change port in appsettings.json
   ```

2. **Database connection failed**
   ```
   Microsoft.Data.SqlClient.SqlException: Cannot open database "TheLeagueDb"
   ```
   **Fix:** Ensure LocalDB is running
   ```bash
   sqllocaldb info mssqllocaldb
   sqllocaldb start mssqllocaldb
   ```

3. **Missing migrations**
   ```
   The model for context 'ApplicationDbContext' has pending changes
   ```
   **Fix:** Apply migrations
   ```bash
   cd TheLeague.Infrastructure
   dotnet ef database update -s ../TheLeague.Api
   ```

---

### Issue: Angular Won't Start

**Symptom:** `npm start` fails

**Possible Causes:**

1. **Missing node_modules**
   ```
   Error: Cannot find module '@angular/core'
   ```
   **Fix:**
   ```bash
   cd the-league-client
   npm install
   ```

2. **Port 4200 in use**
   ```
   Port 4200 is already in use
   ```
   **Fix:**
   ```bash
   # Windows
   netstat -ano | findstr :4200
   taskkill /PID <pid> /F

   # Or use different port
   npm start -- --port 4300
   ```

3. **Node version mismatch**
   ```
   Node.js version v16.x is not supported
   ```
   **Fix:** Use Node 18+
   ```bash
   node --version  # Should be 18+
   ```

---

### Issue: Login Fails

**Symptom:** "Invalid email or password" error

**Debugging Steps:**

1. **Check user exists in database**
   ```sql
   SELECT * FROM AspNetUsers WHERE Email = 'your@email.com'
   ```

2. **Verify password hash exists**
   - Password hash should be non-null in AspNetUsers table

3. **Check seed data ran**
   - If database was just created, ensure seeding completed
   - Check API console output for seeding messages

4. **Try known demo credentials**
   ```
   SuperAdmin: admin@theleague.com / Admin123!
   Club Manager: chairman@teddingtoncc.com / Chairman123!
   ```

---

### Issue: API Returns 401 Unauthorized

**Symptom:** Every API call returns 401

**Debugging Steps:**

1. **Check token exists in localStorage**
   ```javascript
   // Browser console
   localStorage.getItem('token')
   ```

2. **Check token is being sent**
   - Open browser DevTools → Network tab
   - Find a failed request
   - Check Headers → Request Headers → Authorization
   - Should see `Bearer <token>`

3. **Check token hasn't expired**
   - Decode token at jwt.io
   - Check `exp` claim (Unix timestamp)
   - If expired, refresh or re-login

4. **Check interceptor is registered**
   ```typescript
   // app.config.ts should have:
   provideHttpClient(withInterceptors([authInterceptor]))
   ```

---

### Issue: API Returns 403 Forbidden

**Symptom:** Authenticated but action denied

**Cause:** User's role doesn't match required role

**Debugging Steps:**

1. **Check user's role in token**
   - Decode JWT at jwt.io
   - Look for `role` claim

2. **Check endpoint's required role**
   ```csharp
   [Authorize(Roles = "SuperAdmin")]  // Only SuperAdmin
   [Authorize(Roles = "SuperAdmin,ClubManager")]  // Either role
   ```

3. **Common role requirements**
   - `/admin/*` - SuperAdmin only
   - `/club/*` - ClubManager or SuperAdmin
   - `/portal/*` - Member (any authenticated member)

---

### Issue: Data Not Showing (Tenant Isolation)

**Symptom:** List is empty but data exists in database

**Cause:** User's clubId doesn't match data's clubId

**Debugging Steps:**

1. **Check user's clubId in token**
   - Decode JWT, find `clubId` claim

2. **Check data's clubId in database**
   ```sql
   SELECT ClubId FROM Members WHERE Id = 'the-id'
   ```

3. **Ensure they match**
   - If mismatched, user literally cannot see that data
   - This is correct behavior (security)

---

### Issue: CORS Error

**Symptom:** Browser console shows CORS policy error

```
Access to XMLHttpRequest at 'http://localhost:7000/api/...'
from origin 'http://localhost:4200' has been blocked by CORS policy
```

**Fix:** Check CORS configuration in `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Must match exactly
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// IMPORTANT: UseCors must be called before MapControllers
app.UseCors("Development");
app.MapControllers();
```

---

### Issue: Migration Fails

**Symptom:** `dotnet ef migrations add` or `database update` fails

**Common Errors:**

1. **Model has pending changes**
   ```
   Unable to create a 'DbContext' of type 'ApplicationDbContext'
   ```
   **Fix:** Ensure you're in correct directory
   ```bash
   cd TheLeague.Infrastructure
   dotnet ef migrations add Name -s ../TheLeague.Api
   ```

2. **Foreign key constraint**
   ```
   FOREIGN KEY constraint failed
   ```
   **Fix:** Check for orphaned data or drop database to reset
   ```bash
   dotnet ef database drop -s ../TheLeague.Api --force
   dotnet ef database update -s ../TheLeague.Api
   ```

3. **Duplicate migration**
   **Fix:** Delete the duplicate from `Migrations/` folder and retry

---

## Debugging Techniques

### Backend Debugging

1. **Set breakpoints in Visual Studio/VS Code**
   - Click left margin in controller or service
   - Run with F5 (Debug mode)

2. **Check EF Core SQL queries**
   ```csharp
   // In Program.cs
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(connectionString)
              .LogTo(Console.WriteLine, LogLevel.Information));  // Add this
   ```

3. **Use exception middleware logging**
   - All exceptions are caught by `ExceptionHandlingMiddleware`
   - Add logging there for detailed error info

### Frontend Debugging

1. **Browser DevTools (F12)**
   - Console: JavaScript errors
   - Network: API calls and responses
   - Application: localStorage (tokens)

2. **Angular DevTools extension**
   - Inspect component tree
   - View component state
   - Profile change detection

3. **Add console.log strategically**
   ```typescript
   this.api.get('members').subscribe({
     next: (data) => console.log('Members:', data),
     error: (err) => console.error('Error:', err)
   });
   ```

---

## Log Locations

| Component | Log Location |
|-----------|--------------|
| API | Console output (when running `dotnet run`) |
| Angular | Browser console (F12) |
| SQL Server | SQL Server Management Studio → Management → SQL Server Logs |
| EF Core | Console when SQL logging enabled |

---

## Resetting to Clean State

### Full Reset (Nuclear Option)

```bash
# 1. Stop all running processes

# 2. Drop database
cd TheLeague.Infrastructure
dotnet ef database drop -s ../TheLeague.Api --force

# 3. Clean Angular build
cd ../the-league-client
rm -rf node_modules dist .angular

# 4. Reinstall
npm install

# 5. Apply migrations (creates fresh database)
cd ../TheLeague.Infrastructure
dotnet ef database update -s ../TheLeague.Api

# 6. Start API (will seed data)
cd ../TheLeague.Api
dotnet run

# 7. Start Angular
cd ../the-league-client
npm start
```

### Partial Reset (Just Data)

```bash
cd TheLeague.Infrastructure
dotnet ef database drop -s ../TheLeague.Api --force
dotnet ef database update -s ../TheLeague.Api

# Restart API to trigger seeding
cd ../TheLeague.Api
dotnet run
```

---

## Performance Issues

### Slow API Responses

1. **Check for N+1 queries**
   - Enable SQL logging
   - Look for repeated similar queries
   - Fix with `.Include()` or projection

2. **Add missing indexes**
   - ClubId columns should be indexed
   - Frequently filtered columns need indexes

3. **Check pagination**
   - Never load all records
   - Always use `Skip().Take()`

### Slow Angular App

1. **Check bundle size**
   ```bash
   npm run build -- --stats-json
   npx webpack-bundle-analyzer dist/the-league-client/stats.json
   ```

2. **Lazy load feature modules**
   - Already configured in routes
   - Verify with Network tab (chunks load on navigation)

3. **Optimize change detection**
   - Use `OnPush` change detection
   - Use `trackBy` in `@for` loops

---

## Getting Help

### Before Asking

1. Check this troubleshooting guide
2. Search existing issues/discussions
3. Reproduce with minimal steps
4. Note exact error messages

### When Asking

Include:
- What you were trying to do
- What you expected to happen
- What actually happened
- Error messages (exact text)
- Steps to reproduce
- Relevant code snippets

### Resources

- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core)
- [Angular Docs](https://angular.dev)
- [Tailwind CSS Docs](https://tailwindcss.com/docs)

---

## Next Steps

→ [10_DEVELOPMENT_WORKFLOW.md](./10_DEVELOPMENT_WORKFLOW.md) - Git workflow and development process

