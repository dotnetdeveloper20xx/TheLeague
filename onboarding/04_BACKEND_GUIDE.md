# Backend Guide

Deep dive into the ASP.NET Core API layer.

---

## Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core 8.0 |
| Language | C# 12 |
| ORM | Entity Framework Core 8 |
| Authentication | JWT Bearer + ASP.NET Identity |
| API Docs | Swagger/OpenAPI |
| DI Container | Built-in Microsoft DI |

---

## Project Structure

```
TheLeague.Api/
├── Controllers/                    # HTTP endpoints
│   ├── AuthController.cs           # Authentication endpoints
│   ├── BaseApiController.cs        # Common controller base
│   ├── MembersController.cs        # Member CRUD
│   ├── SessionsController.cs       # Session/booking management
│   ├── EventsController.cs         # Event management
│   ├── PaymentsController.cs       # Payment recording
│   ├── ClubController.cs           # Club-specific operations
│   ├── ClubsController.cs          # Admin club management
│   ├── AdminController.cs          # System administration
│   └── ...                         # More controllers
│
├── Services/                       # Business logic
│   ├── AuthService.cs              # Authentication logic
│   ├── MemberService.cs            # Member operations
│   ├── SessionService.cs           # Session booking logic
│   ├── PaymentService.cs           # Payment processing
│   ├── ClubService.cs              # Club operations
│   └── ...                         # More services
│
├── DTOs/                           # Data transfer objects
│   ├── Auth/                       # Login, Register DTOs
│   ├── Members/                    # Member request/response
│   ├── Sessions/                   # Session DTOs
│   └── ...                         # More DTOs
│
├── Middleware/                     # Pipeline components
│   ├── ExceptionHandlingMiddleware.cs
│   └── TenantMiddleware.cs
│
├── Providers/                      # External integrations
│   ├── Payment/                    # Stripe, PayPal, Mock
│   └── Email/                      # SendGrid, Mock
│
├── Program.cs                      # Application startup
├── appsettings.json                # Configuration
└── appsettings.Development.json    # Dev overrides
```

---

## Request Lifecycle

Every HTTP request flows through this pipeline:

```csharp
// Configured in Program.cs

app.UseExceptionHandling();      // 1. Catch exceptions, return proper HTTP status
app.UseAuthentication();          // 2. Validate JWT token
app.UseAuthorization();           // 3. Check [Authorize] attributes
// TenantMiddleware runs via DI  // 4. Extract ClubId from JWT
app.MapControllers();             // 5. Route to controller action
```

---

## Controllers

### Base Controller

All controllers inherit from `BaseApiController`:

```csharp
// TheLeague.Api/Controllers/BaseApiController.cs

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    // Get current user's club ID from JWT claims
    protected Guid GetClubId()
    {
        var claim = User.FindFirst("clubId");
        return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
    }

    // Get current user's ID
    protected string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
    }

    // Get current user's member ID (if they're a member)
    protected Guid? GetMemberId()
    {
        var claim = User.FindFirst("memberId");
        return claim != null ? Guid.Parse(claim.Value) : null;
    }
}
```

### Controller Pattern

Controllers are thin - they delegate to services:

```csharp
// Example: MembersController.cs

[ApiController]
[Route("api/[controller]")]
[Authorize]  // All endpoints require authentication
public class MembersController : BaseApiController
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<MemberListDto>>> GetMembers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null)
    {
        var clubId = GetClubId();
        var result = await _memberService.GetMembersAsync(clubId, page, pageSize, search);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDetailDto>> GetMember(Guid id)
    {
        var clubId = GetClubId();
        var member = await _memberService.GetMemberByIdAsync(clubId, id);
        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDetailDto>> CreateMember(CreateMemberDto dto)
    {
        var clubId = GetClubId();
        var member = await _memberService.CreateMemberAsync(clubId, dto);
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
    }
}
```

---

## API Endpoint Inventory

### Authentication (`/api/auth`)

| Method | Route | Purpose | Auth |
|--------|-------|---------|------|
| POST | `/login` | User login | No |
| POST | `/register` | User registration | No |
| POST | `/refresh-token` | Refresh JWT | No |
| POST | `/forgot-password` | Request password reset | No |
| POST | `/reset-password` | Complete password reset | No |
| POST | `/change-password` | Change password | Yes |
| GET | `/me` | Get current user info | Yes |

### Members (`/api/members`)

| Method | Route | Purpose | Auth |
|--------|-------|---------|------|
| GET | `/` | List members (paginated) | Yes |
| GET | `/{id}` | Get member details | Yes |
| POST | `/` | Create member | Yes |
| PUT | `/{id}` | Update member | Yes |
| DELETE | `/{id}` | Delete member | Yes |
| GET | `/{id}/family` | Get family members | Yes |
| POST | `/{id}/family` | Add family member | Yes |

### Sessions (`/api/sessions`)

| Method | Route | Purpose | Auth |
|--------|-------|---------|------|
| GET | `/` | List sessions | Yes |
| GET | `/{id}` | Get session details | Yes |
| POST | `/` | Create session | Yes |
| PUT | `/{id}` | Update session | Yes |
| DELETE | `/{id}` | Delete session | Yes |
| POST | `/{id}/book` | Book a session | Yes |
| DELETE | `/{id}/book` | Cancel booking | Yes |
| GET | `/{id}/attendees` | List attendees | Yes |

### Events (`/api/events`)

| Method | Route | Purpose | Auth |
|--------|-------|---------|------|
| GET | `/` | List events | Yes |
| GET | `/{id}` | Get event details | Yes |
| POST | `/` | Create event | Yes |
| PUT | `/{id}` | Update event | Yes |
| DELETE | `/{id}` | Delete event | Yes |

### Payments (`/api/payments`)

| Method | Route | Purpose | Auth |
|--------|-------|---------|------|
| GET | `/` | List payments | Yes |
| GET | `/{id}` | Get payment details | Yes |
| POST | `/` | Record payment | Yes |
| POST | `/{id}/refund` | Process refund | Yes |

### Admin (`/api/admin`)

| Method | Route | Purpose | Auth |
|--------|-------|---------|------|
| GET | `/clubs` | List all clubs | SuperAdmin |
| POST | `/clubs` | Create club | SuperAdmin |
| GET | `/users` | List all users | SuperAdmin |
| GET | `/dashboard` | System dashboard | SuperAdmin |

---

## Services

Services contain business logic and data access.

### Service Interface Pattern

```csharp
// Interface defines contract
public interface IMemberService
{
    Task<PagedResult<MemberListDto>> GetMembersAsync(
        Guid clubId, int page, int pageSize, string? search);
    Task<MemberDetailDto> GetMemberByIdAsync(Guid clubId, Guid id);
    Task<MemberDetailDto> CreateMemberAsync(Guid clubId, CreateMemberDto dto);
    Task<MemberDetailDto> UpdateMemberAsync(Guid clubId, Guid id, UpdateMemberDto dto);
    Task DeleteMemberAsync(Guid clubId, Guid id);
}

// Implementation
public class MemberService : IMemberService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public MemberService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<PagedResult<MemberListDto>> GetMembersAsync(
        Guid clubId, int page, int pageSize, string? search)
    {
        var query = _context.Members
            .Where(m => m.ClubId == clubId);  // Tenant filter

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(m =>
                m.FirstName.Contains(search) ||
                m.LastName.Contains(search) ||
                m.Email.Contains(search));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(m => m.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MemberListDto { /* mapping */ })
            .ToListAsync();

        return new PagedResult<MemberListDto>(items, total, page, pageSize);
    }
}
```

### Key Services

| Service | Responsibility |
|---------|----------------|
| `AuthService` | Login, token generation, password reset |
| `MemberService` | Member CRUD, family members |
| `SessionService` | Sessions, bookings, attendance |
| `EventService` | Events, RSVPs, tickets |
| `PaymentService` | Payment recording, refunds |
| `ClubService` | Club settings, dashboard data |
| `InvoiceService` | Invoice generation |
| `ReportService` | Report data aggregation |

---

## DTOs

DTOs (Data Transfer Objects) define API request/response shapes.

### Naming Convention

- `CreateXxxDto` - For POST requests
- `UpdateXxxDto` - For PUT requests
- `XxxListDto` - For list responses (minimal data)
- `XxxDetailDto` - For detail responses (full data)

### Example

```csharp
// TheLeague.Api/DTOs/Members/CreateMemberDto.cs
public record CreateMemberDto(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    DateTime? DateOfBirth,
    string? Address
);

// TheLeague.Api/DTOs/Members/MemberListDto.cs
public record MemberListDto(
    Guid Id,
    string FullName,
    string Email,
    string? Phone,
    MemberStatus Status,
    DateTime JoinedDate,
    string? MembershipType
);
```

---

## API Response Pattern

All responses follow a consistent structure:

```csharp
public record ApiResponse<T>(
    bool Success,
    string? Message,
    T? Data
);

// Usage in controller
return Ok(new ApiResponse<MemberDetailDto>(true, null, member));
```

For paginated lists:

```csharp
public record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
```

---

## Error Handling

The `ExceptionHandlingMiddleware` converts exceptions to HTTP responses:

```csharp
// TheLeague.Api/Middleware/ExceptionHandlingMiddleware.cs

public class ExceptionHandlingMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = new ApiResponse<object>(false, exception.Message, null);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(response);
    }
}
```

---

## Dependency Injection

Services are registered in `Program.cs`:

```csharp
// TheLeague.Api/Program.cs

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
// ... more services

// Multi-tenancy
builder.Services.AddScoped<ITenantService, TenantService>();

// External providers (singleton factories)
builder.Services.AddSingleton<IPaymentProviderFactory, PaymentProviderFactory>();
builder.Services.AddSingleton<IEmailProviderFactory, EmailProviderFactory>();
```

---

## Configuration

### appsettings.json Structure

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=TheLeagueDb;..."
  },
  "Jwt": {
    "Secret": "your-secret-key-min-32-chars",
    "Issuer": "TheLeague",
    "Audience": "TheLeagueApp",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "PaymentProvider": {
    "Type": "Mock",  // Mock, Stripe, PayPal
    "Stripe": {
      "SecretKey": "sk_test_...",
      "PublishableKey": "pk_test_..."
    }
  },
  "EmailProvider": {
    "Type": "Mock",  // Mock, SendGrid
    "SendGrid": {
      "ApiKey": "SG...."
    }
  },
  "Kestrel": {
    "Endpoints": {
      "Http": { "Url": "http://localhost:7000" }
    }
  }
}
```

---

## Quick Reference

### Adding a New Endpoint

1. Create/update DTO in `DTOs/` folder
2. Add method to service interface
3. Implement in service class
4. Add action to controller
5. Test via Swagger

### Adding a New Service

1. Create interface `IXxxService` in `Services/`
2. Create implementation `XxxService`
3. Register in `Program.cs`: `builder.Services.AddScoped<IXxxService, XxxService>()`
4. Inject into controllers via constructor

### Common Patterns

```csharp
// Get tenant-scoped data
var clubId = GetClubId();
var data = await _context.Members.Where(m => m.ClubId == clubId).ToListAsync();

// Return not found
if (entity == null)
    throw new KeyNotFoundException($"Member {id} not found");

// Return validation error
if (string.IsNullOrEmpty(dto.Email))
    throw new ArgumentException("Email is required");

// Paginated response
return Ok(new PagedResult<T>(items, total, page, pageSize));
```

---

## Next Steps

→ [05_DATABASE_GUIDE.md](./05_DATABASE_GUIDE.md) - Understand the data model
