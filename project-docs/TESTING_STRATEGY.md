# The League - Testing Strategy Document

## Overview

This document outlines the comprehensive testing strategy for The League platform. The strategy covers unit testing, integration testing, end-to-end testing, and quality assurance processes.

---

## Testing Pyramid

```
                    /\
                   /  \
                  / E2E \        <- Few, expensive, slow
                 /______\
                /        \
               / Integration \   <- Some, moderate cost
              /______________\
             /                \
            /    Unit Tests    \ <- Many, cheap, fast
           /____________________\
```

### Test Distribution Goals

| Test Type | Coverage Target | Quantity |
|-----------|-----------------|----------|
| Unit Tests | 80% code coverage | Many (hundreds) |
| Integration Tests | Critical paths | Moderate (dozens) |
| E2E Tests | User journeys | Few (10-20) |

---

## Unit Testing

### Backend (C# / .NET)

**Framework:** xUnit

**Coverage Areas:**
- Service layer business logic
- Entity validation
- Utility/helper methods
- Authorization logic

#### Project Structure
```
TheLeague.Tests/
├── Unit/
│   ├── Services/
│   │   ├── AuthServiceTests.cs
│   │   ├── MemberServiceTests.cs
│   │   ├── MembershipServiceTests.cs
│   │   ├── PaymentServiceTests.cs
│   │   └── ...
│   ├── Providers/
│   │   ├── MockPaymentProviderTests.cs
│   │   └── MockEmailProviderTests.cs
│   └── Middleware/
│       └── TenantMiddlewareTests.cs
├── Integration/
│   └── ...
└── Helpers/
    └── TestDataFactory.cs
```

#### Example Unit Test

```csharp
// Unit/Services/MemberServiceTests.cs
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TheLeague.Api.Services;
using TheLeague.Infrastructure.Data;
using TheLeague.Core.Entities;

namespace TheLeague.Tests.Unit.Services;

public class MemberServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ITenantService> _tenantServiceMock;
    private readonly MemberService _sut;

    public MemberServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _tenantServiceMock = new Mock<ITenantService>();
        _tenantServiceMock.Setup(x => x.CurrentTenantId).Returns(Guid.NewGuid());

        _sut = new MemberService(_context, _tenantServiceMock.Object);
    }

    [Fact]
    public async Task CreateMember_WithValidData_ReturnsCreatedMember()
    {
        // Arrange
        var request = new CreateMemberRequest
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john@example.com"
        };

        // Act
        var result = await _sut.CreateMemberAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Smith");
        result.MemberNumber.Should().StartWith("MBR-");
    }

    [Fact]
    public async Task CreateMember_WithDuplicateEmail_ThrowsException()
    {
        // Arrange
        var clubId = _tenantServiceMock.Object.CurrentTenantId!.Value;
        _context.Members.Add(new Member
        {
            ClubId = clubId,
            FirstName = "Existing",
            LastName = "Member",
            Email = "existing@example.com"
        });
        await _context.SaveChangesAsync();

        var request = new CreateMemberRequest
        {
            Email = "existing@example.com"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.CreateMemberAsync(request)
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateMember_WithInvalidEmail_ThrowsValidationException(string email)
    {
        // Arrange
        var request = new CreateMemberRequest { Email = email };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _sut.CreateMemberAsync(request)
        );
    }
}
```

#### Test Data Factory

```csharp
// Helpers/TestDataFactory.cs
public static class TestDataFactory
{
    public static Club CreateClub(Guid? id = null)
    {
        return new Club
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Test Cricket Club",
            Slug = "test-cricket-club",
            ClubType = ClubType.Cricket,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Member CreateMember(Guid clubId, string email = null)
    {
        return new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = "Test",
            LastName = "Member",
            Email = email ?? $"test-{Guid.NewGuid()}@example.com",
            MemberNumber = $"MBR-{Random.Shared.Next(1000, 9999)}",
            Status = MemberStatus.Active,
            JoinedDate = DateTime.UtcNow
        };
    }

    public static MembershipType CreateMembershipType(Guid clubId)
    {
        return new MembershipType
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = "Senior Membership",
            BasePrice = 180.00m,
            IsActive = true
        };
    }
}
```

### Frontend (Angular / TypeScript)

**Framework:** Karma + Jasmine

**Coverage Areas:**
- Service methods
- Component logic
- Pipe transformations
- Guard functionality

#### Example Angular Unit Test

```typescript
// core/services/member.service.spec.ts
import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { MemberService } from './member.service';
import { Member } from '../models/member.model';

describe('MemberService', () => {
  let service: MemberService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        MemberService
      ]
    });

    service = TestBed.inject(MemberService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch members', () => {
    const mockMembers: Member[] = [
      { id: '1', firstName: 'John', lastName: 'Smith', email: 'john@test.com' }
    ];

    service.getMembers({ page: 1, pageSize: 20 }).subscribe(result => {
      expect(result.items.length).toBe(1);
      expect(result.items[0].firstName).toBe('John');
    });

    const req = httpMock.expectOne(request =>
      request.url.includes('/api/members')
    );
    expect(req.request.method).toBe('GET');
    req.flush({ success: true, data: { items: mockMembers, totalCount: 1 } });
  });

  it('should create member', () => {
    const newMember = { firstName: 'Jane', lastName: 'Doe', email: 'jane@test.com' };

    service.createMember(newMember).subscribe(result => {
      expect(result.firstName).toBe('Jane');
    });

    const req = httpMock.expectOne('/api/members');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newMember);
    req.flush({ success: true, data: { id: '2', ...newMember } });
  });
});
```

#### Component Testing

```typescript
// features/club/members/members-list.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MembersListComponent } from './members-list.component';
import { MemberService } from '@core/services/member.service';
import { of } from 'rxjs';

describe('MembersListComponent', () => {
  let component: MembersListComponent;
  let fixture: ComponentFixture<MembersListComponent>;
  let memberServiceSpy: jasmine.SpyObj<MemberService>;

  beforeEach(async () => {
    memberServiceSpy = jasmine.createSpyObj('MemberService', ['getMembers']);
    memberServiceSpy.getMembers.and.returnValue(of({
      items: [],
      totalCount: 0,
      page: 1,
      pageSize: 20
    }));

    await TestBed.configureTestingModule({
      imports: [MembersListComponent],
      providers: [
        { provide: MemberService, useValue: memberServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MembersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load members on init', () => {
    expect(memberServiceSpy.getMembers).toHaveBeenCalled();
  });

  it('should search members when search term changes', () => {
    component.searchTerm = 'John';
    component.onSearch();

    expect(memberServiceSpy.getMembers).toHaveBeenCalledWith(
      jasmine.objectContaining({ search: 'John' })
    );
  });
});
```

---

## Integration Testing

### API Integration Tests

Test API endpoints with a real database (in-memory or test database).

```csharp
// Integration/Controllers/MembersControllerTests.cs
public class MembersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public MembersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace with in-memory database
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetMembers_WithValidToken_ReturnsMembers()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/members");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetMembers_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/members");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateMember_WithValidData_ReturnsCreated()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/members", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<MemberDto>>();
        result.Success.Should().BeTrue();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var loginRequest = new { Email = "admin@test.com", Password = "TestPass123" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        return result.Data.AccessToken;
    }
}
```

---

## End-to-End Testing

### Framework: Playwright

E2E tests simulate real user interactions in a browser environment.

#### Playwright Configuration

```typescript
// playwright.config.ts
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: 'html',
  use: {
    baseURL: 'http://localhost:4200',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] },
    },
    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] },
    },
    {
      name: 'Mobile Chrome',
      use: { ...devices['Pixel 5'] },
    },
  ],
  webServer: {
    command: 'npm run start',
    url: 'http://localhost:4200',
    reuseExistingServer: !process.env.CI,
  },
});
```

#### E2E Test Examples

```typescript
// e2e/auth.spec.ts
import { test, expect } from '@playwright/test';

test.describe('Authentication', () => {
  test('should login successfully with valid credentials', async ({ page }) => {
    await page.goto('/auth/login');

    await page.fill('input[formControlName="email"]', 'manager@willowcreek.com');
    await page.fill('input[formControlName="password"]', 'Password123!');
    await page.click('button[type="submit"]');

    await expect(page).toHaveURL('/club/dashboard');
    await expect(page.locator('h1')).toContainText('Dashboard');
  });

  test('should show error with invalid credentials', async ({ page }) => {
    await page.goto('/auth/login');

    await page.fill('input[formControlName="email"]', 'invalid@test.com');
    await page.fill('input[formControlName="password"]', 'wrongpassword');
    await page.click('button[type="submit"]');

    await expect(page.locator('.text-red-500')).toBeVisible();
    await expect(page).toHaveURL('/auth/login');
  });

  test('should logout successfully', async ({ page }) => {
    // Login first
    await page.goto('/auth/login');
    await page.fill('input[formControlName="email"]', 'manager@willowcreek.com');
    await page.fill('input[formControlName="password"]', 'Password123!');
    await page.click('button[type="submit"]');

    await expect(page).toHaveURL('/club/dashboard');

    // Logout
    await page.click('text=Logout');

    await expect(page).toHaveURL('/auth/login');
  });
});
```

```typescript
// e2e/members.spec.ts
import { test, expect } from '@playwright/test';

test.describe('Member Management', () => {
  test.beforeEach(async ({ page }) => {
    // Login as club manager
    await page.goto('/auth/login');
    await page.fill('input[formControlName="email"]', 'manager@willowcreek.com');
    await page.fill('input[formControlName="password"]', 'Password123!');
    await page.click('button[type="submit"]');
    await expect(page).toHaveURL('/club/dashboard');
  });

  test('should display members list', async ({ page }) => {
    await page.goto('/club/members');

    await expect(page.locator('h1')).toContainText('Members');
    await expect(page.locator('table')).toBeVisible();
  });

  test('should create new member', async ({ page }) => {
    await page.goto('/club/members/new');

    await page.fill('input[formControlName="firstName"]', 'New');
    await page.fill('input[formControlName="lastName"]', 'Member');
    await page.fill('input[formControlName="email"]', `test-${Date.now()}@example.com`);
    await page.fill('input[formControlName="phone"]', '07700 900000');

    await page.click('button[type="submit"]');

    await expect(page).toHaveURL('/club/members');
    await expect(page.locator('.notification-success')).toBeVisible();
  });

  test('should search members', async ({ page }) => {
    await page.goto('/club/members');

    await page.fill('input[placeholder*="Search"]', 'John');
    await page.waitForResponse(response =>
      response.url().includes('/api/members') && response.status() === 200
    );

    // Verify filtered results
    const rows = page.locator('table tbody tr');
    await expect(rows.first()).toContainText('John');
  });
});
```

```typescript
// e2e/visual-regression.spec.ts
import { test, expect } from '@playwright/test';

test.describe('Visual Regression', () => {
  test('login page screenshot', async ({ page }) => {
    await page.goto('/auth/login');
    await expect(page).toHaveScreenshot('login-page.png');
  });

  test('dashboard screenshot', async ({ page }) => {
    // Login first
    await page.goto('/auth/login');
    await page.fill('input[formControlName="email"]', 'manager@willowcreek.com');
    await page.fill('input[formControlName="password"]', 'Password123!');
    await page.click('button[type="submit"]');

    await page.waitForURL('/club/dashboard');
    await expect(page).toHaveScreenshot('dashboard.png', {
      maxDiffPixelRatio: 0.05
    });
  });
});
```

---

## Performance Testing

### Load Testing with k6

```javascript
// load-tests/members-api.js
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 20 },  // Ramp up
    { duration: '1m', target: 20 },   // Stay at 20 users
    { duration: '30s', target: 0 },   // Ramp down
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'], // 95% of requests under 500ms
    http_req_failed: ['rate<0.01'],   // Error rate under 1%
  },
};

const BASE_URL = 'http://localhost:7000/api';
let authToken;

export function setup() {
  const loginRes = http.post(`${BASE_URL}/auth/login`, JSON.stringify({
    email: 'manager@willowcreek.com',
    password: 'Password123!'
  }), { headers: { 'Content-Type': 'application/json' } });

  return { token: JSON.parse(loginRes.body).data.accessToken };
}

export default function(data) {
  const headers = {
    'Authorization': `Bearer ${data.token}`,
    'Content-Type': 'application/json'
  };

  // Get members list
  const membersRes = http.get(`${BASE_URL}/members?page=1&pageSize=20`, { headers });
  check(membersRes, {
    'members status is 200': (r) => r.status === 200,
    'members response time OK': (r) => r.timings.duration < 500,
  });

  sleep(1);
}
```

---

## Security Testing

### OWASP Top 10 Checklist

| Vulnerability | Test Approach | Status |
|---------------|---------------|--------|
| Injection | Parameterized queries, input validation | Covered by EF Core |
| Broken Auth | Session tests, password policy tests | Manual review |
| Sensitive Data | HTTPS, encrypted secrets | Configuration review |
| XXE | Disabled in JSON API | N/A |
| Broken Access Control | Authorization tests per role | Integration tests |
| Security Misconfiguration | Configuration audit | Manual review |
| XSS | Angular sanitization tests | Framework default |
| Insecure Deserialization | Input validation | Manual review |
| Using Components with Vulnerabilities | Dependency scan | `npm audit`, `dotnet audit` |
| Insufficient Logging | Audit log tests | Integration tests |

### Security Test Examples

```csharp
[Fact]
public async Task MemberEndpoint_CrossTenantAccess_Forbidden()
{
    // Arrange - Login as Club A manager
    var tokenClubA = await GetAuthTokenForClub("club-a");
    _client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", tokenClubA);

    // Get member ID from Club B
    var memberIdClubB = "member-from-club-b-guid";

    // Act - Try to access Club B's member
    var response = await _client.GetAsync($"/api/members/{memberIdClubB}");

    // Assert - Should not find member (filtered by tenant)
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
```

---

## Test Automation Pipeline

### CI/CD Test Stages

```yaml
# .github/workflows/test.yml
name: Test Suite

on: [push, pull_request]

jobs:
  unit-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      - name: Run Unit Tests
        run: dotnet test --filter Category=Unit --collect:"XPlat Code Coverage"
      - name: Upload Coverage
        uses: codecov/codecov-action@v3

  integration-tests:
    runs-on: ubuntu-latest
    services:
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
    steps:
      - uses: actions/checkout@v3
      - name: Run Integration Tests
        run: dotnet test --filter Category=Integration

  e2e-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup Node
        uses: actions/setup-node@v3
      - name: Install Playwright
        run: npx playwright install --with-deps
      - name: Run E2E Tests
        run: npm run e2e
      - name: Upload Screenshots
        if: failure()
        uses: actions/upload-artifact@v3
        with:
          name: playwright-screenshots
          path: test-results/
```

---

## Test Coverage Goals

| Area | Current | Target |
|------|---------|--------|
| Backend Unit Tests | TBD | 80% |
| Frontend Unit Tests | TBD | 70% |
| Integration Tests | TBD | Critical paths |
| E2E Tests | TBD | Core user journeys |

---

## Running Tests

### Backend Commands

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific category
dotnet test --filter Category=Unit
dotnet test --filter Category=Integration
```

### Frontend Commands

```bash
# Run unit tests
npm test

# Run with coverage
npm test -- --code-coverage

# Run E2E tests
npm run e2e

# Run specific E2E test
npx playwright test e2e/auth.spec.ts
```

---

*Document Version: 1.0*
*Last Updated: Pre-Development Planning Phase*
