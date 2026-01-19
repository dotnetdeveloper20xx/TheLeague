# Development Workflow

How we develop, test, and ship code.

---

## Git Workflow

### Branch Strategy

```
main (or master)
  │
  ├── feature/add-member-notes
  │
  ├── feature/session-waitlist
  │
  ├── bugfix/login-redirect
  │
  └── hotfix/payment-calculation
```

### Branch Naming

| Type | Pattern | Example |
|------|---------|---------|
| Feature | `feature/<short-description>` | `feature/add-member-notes` |
| Bug Fix | `bugfix/<issue-description>` | `bugfix/login-redirect` |
| Hotfix | `hotfix/<critical-issue>` | `hotfix/payment-calculation` |
| Refactor | `refactor/<area>` | `refactor/member-service` |

### Development Flow

```
1. Create branch from main
   git checkout main
   git pull
   git checkout -b feature/my-feature

2. Make changes (commit often)
   git add .
   git commit -m "Add member notes field"

3. Push branch
   git push -u origin feature/my-feature

4. Create Pull Request
   - Use PR template
   - Request review

5. Address review feedback
   git add .
   git commit -m "Address review: improve validation"
   git push

6. Merge after approval
   - Squash and merge preferred
   - Delete branch after merge
```

---

## Making Changes

### Backend Changes

1. **Create feature branch**
   ```bash
   git checkout -b feature/add-member-notes
   ```

2. **Make code changes**
   - Update entities if needed
   - Update DTOs
   - Update services
   - Update controllers

3. **Create migration (if schema changed)**
   ```bash
   cd TheLeague.Infrastructure
   dotnet ef migrations add AddMemberNotes -s ../TheLeague.Api
   ```

4. **Test locally**
   ```bash
   cd TheLeague.Api
   dotnet run
   # Test via Swagger or Postman
   ```

5. **Run tests**
   ```bash
   cd TheLeague.Tests
   dotnet test
   ```

6. **Commit and push**
   ```bash
   git add .
   git commit -m "Add member notes field

   - Added Notes property to Member entity
   - Created migration
   - Updated DTOs and service"
   git push -u origin feature/add-member-notes
   ```

### Frontend Changes

1. **Create feature branch** (if not already)

2. **Make code changes**
   - Update components
   - Update services
   - Update templates

3. **Test locally**
   ```bash
   cd the-league-client
   npm start
   # Test in browser
   ```

4. **Run linter**
   ```bash
   npm run lint
   ```

5. **Build to check for errors**
   ```bash
   npm run build
   ```

6. **Run E2E tests**
   ```bash
   npm run e2e
   ```

7. **Commit and push**

---

## Commit Guidelines

### Message Format

```
<type>: <short summary>

<optional body with more detail>

<optional footer>
```

### Types

| Type | Use For |
|------|---------|
| `feat` | New feature |
| `fix` | Bug fix |
| `refactor` | Code change that doesn't add feature or fix bug |
| `docs` | Documentation only |
| `style` | Formatting, missing semicolons, etc. |
| `test` | Adding or updating tests |
| `chore` | Maintenance tasks, dependencies |

### Examples

```bash
# Simple change
git commit -m "feat: add member notes field"

# With body
git commit -m "fix: correct payment calculation

The total was including cancelled bookings.
Now filters out cancelled items before summing."

# Breaking change
git commit -m "refactor!: rename Member.Phone to Member.PhoneNumber

BREAKING CHANGE: API consumers need to update their DTOs"
```

---

## Pull Request Process

### Creating a PR

1. **Push your branch**
   ```bash
   git push -u origin feature/my-feature
   ```

2. **Open GitHub/GitLab**
   - Navigate to repository
   - Click "Create Pull Request"

3. **Fill out PR template**
   ```markdown
   ## Summary
   Brief description of changes

   ## Changes
   - Added X
   - Updated Y
   - Fixed Z

   ## Testing
   - [ ] Tested locally
   - [ ] Added/updated tests
   - [ ] Migrations run successfully
   - [ ] No new warnings

   ## Screenshots (if UI changes)
   Before: [screenshot]
   After: [screenshot]
   ```

4. **Request reviewers**

### Review Checklist

**Reviewers should check:**

- [ ] Code follows project patterns
- [ ] Changes are focused (not mixing concerns)
- [ ] No commented-out code
- [ ] DTOs are used (not exposing entities)
- [ ] ClubId filtering is correct (multi-tenancy)
- [ ] Error handling is appropriate
- [ ] No security issues introduced
- [ ] Tests cover new functionality

### After Approval

1. **Squash and merge** (preferred)
   - Keeps history clean
   - One commit per feature

2. **Delete the branch**
   - GitHub can do this automatically

---

## Testing

### Backend Tests

```bash
cd TheLeague.Tests
dotnet test
```

**Test Types:**

| Type | Purpose | Location |
|------|---------|----------|
| Unit Tests | Test services in isolation | `TheLeague.Tests/Services/` |
| Integration Tests | Test API endpoints | `TheLeague.Tests/Controllers/` |

**Writing a Unit Test:**

```csharp
public class MemberServiceTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly MemberService _service;

    public MemberServiceTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _service = new MemberService(_mockContext.Object);
    }

    [Fact]
    public async Task CreateMember_WithValidData_ReturnsNewMember()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var dto = new CreateMemberDto("John", "Doe", "john@example.com", null, null, null);

        // Act
        var result = await _service.CreateMemberAsync(clubId, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
    }
}
```

### Frontend Tests

**Unit Tests (Jasmine):**

```bash
cd the-league-client
npm test
```

**E2E Tests (Playwright):**

```bash
cd the-league-client
npm run e2e
```

**E2E Test Example:**

```typescript
// e2e/auth.spec.ts
import { test, expect } from '@playwright/test';

test('login with valid credentials', async ({ page }) => {
  await page.goto('/login');

  await page.fill('input[formControlName="email"]', 'admin@theleague.com');
  await page.fill('input[formControlName="password"]', 'Admin123!');
  await page.click('button[type="submit"]');

  await expect(page).toHaveURL('/admin/dashboard');
  await expect(page.locator('h1')).toContainText('Dashboard');
});
```

---

## Running Locally

### Quick Start

**Terminal 1 - Backend:**
```bash
cd TheLeague.Api
dotnet run
# API at http://localhost:7000
# Swagger at http://localhost:7000/swagger
```

**Terminal 2 - Frontend:**
```bash
cd the-league-client
npm start
# App at http://localhost:4200
```

### With Hot Reload

**Backend (watch mode):**
```bash
cd TheLeague.Api
dotnet watch run
```

**Frontend (default):**
```bash
npm start  # Hot reload enabled by default
```

---

## Database Management

### View Current Schema

```bash
cd TheLeague.Infrastructure
dotnet ef migrations list -s ../TheLeague.Api
```

### Create Migration

```bash
dotnet ef migrations add <MigrationName> -s ../TheLeague.Api
```

### Apply Migrations

```bash
dotnet ef database update -s ../TheLeague.Api
```

### Reset Database

```bash
dotnet ef database drop -s ../TheLeague.Api --force
dotnet ef database update -s ../TheLeague.Api
# Restart API to trigger seeding
```

### Generate SQL Script (for production)

```bash
dotnet ef migrations script --idempotent -o migration.sql -s ../TheLeague.Api
```

---

## Code Quality

### Backend

- Follow C# naming conventions
- Use `async/await` for all database operations
- Always filter by `ClubId` for tenant isolation
- Use DTOs for API input/output
- Keep controllers thin, services fat

### Frontend

- Use standalone components
- Prefer signals over BehaviorSubject for new code
- Use reactive forms for complex forms
- Keep components focused (single responsibility)
- Extract shared logic to services

### General

- No commented-out code in commits
- No `console.log` in production code
- No hard-coded credentials
- No business logic in controllers
- Keep functions small and focused

---

## Environment Configuration

### Backend (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TheLeagueDb;..."
  },
  "Jwt": {
    "Secret": "your-secret-key"
  },
  "PaymentProvider": {
    "Type": "Mock"  // Mock, Stripe, PayPal
  },
  "EmailProvider": {
    "Type": "Mock"  // Mock, SendGrid
  }
}
```

### Frontend (environment.ts)

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:7000/api'
};
```

---

## Useful Commands Reference

### Git

```bash
# Create and switch to branch
git checkout -b feature/name

# Update branch with main
git checkout main
git pull
git checkout feature/name
git rebase main

# Undo last commit (keep changes)
git reset --soft HEAD~1

# Discard all local changes
git checkout -- .
git clean -fd
```

### .NET

```bash
# Build
dotnet build

# Run
dotnet run

# Run with watch
dotnet watch run

# Run tests
dotnet test

# Add package
dotnet add package <PackageName>
```

### npm

```bash
# Install dependencies
npm install

# Start dev server
npm start

# Build for production
npm run build

# Run tests
npm test

# Run E2E tests
npm run e2e

# Check for outdated packages
npm outdated

# Update packages
npm update
```

### EF Core

```bash
# List migrations
dotnet ef migrations list -s ../TheLeague.Api

# Add migration
dotnet ef migrations add <Name> -s ../TheLeague.Api

# Update database
dotnet ef database update -s ../TheLeague.Api

# Drop database
dotnet ef database drop -s ../TheLeague.Api --force

# Generate SQL script
dotnet ef migrations script --idempotent -o script.sql -s ../TheLeague.Api
```

---

## You're Ready!

Congratulations! You've completed the onboarding documentation.

### What You Should Now Know

- [ ] Project vision and business domain
- [ ] How to set up local development environment
- [ ] System architecture and how components connect
- [ ] Backend API structure and patterns
- [ ] Database schema and EF Core usage
- [ ] Frontend Angular architecture
- [ ] Authentication and security model
- [ ] How key features work end-to-end
- [ ] How to troubleshoot common issues
- [ ] Development workflow and best practices

### First Tasks to Try

1. **Login as different users** - Verify multi-tenant isolation
2. **Create a new member** - Full CRUD operation
3. **Book a session** - Member portal functionality
4. **Add a new field** - Simple schema + UI change
5. **Write a test** - Understand test patterns

### Keep Learning

- Explore code you're curious about
- Ask questions when stuck
- Document discoveries for others
- Improve these docs when you find gaps

**Welcome to the team!**

