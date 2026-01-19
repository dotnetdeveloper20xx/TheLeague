# Authentication and Security

How users are identified and protected in The League.

---

## Authentication Overview

The League uses **JWT (JSON Web Tokens)** for stateless authentication:

```
┌─────────────┐    1. POST /api/auth/login     ┌─────────────┐
│   Angular   │  ─────────────────────────────►│   API       │
│   Client    │    { email, password }         │             │
└─────────────┘                                └──────┬──────┘
                                                      │
                                               2. Validate credentials
                                               3. Generate JWT + Refresh Token
                                                      │
┌─────────────┐    4. { token, refreshToken }  ┌──────▼──────┐
│   Angular   │  ◄─────────────────────────────│   API       │
│   Client    │                                │             │
└──────┬──────┘                                └─────────────┘
       │
       │ 5. Store tokens in localStorage
       │
       │ 6. Attach token to all API requests
       │    Authorization: Bearer <token>
       ▼
┌─────────────┐                                ┌─────────────┐
│   Angular   │    7. Request with token       │   API       │
│   Client    │  ─────────────────────────────►│  Validates  │
└─────────────┘                                │   & serves  │
                                               └─────────────┘
```

---

## User Roles

```csharp
// TheLeague.Core/Enums/Enums.cs

public enum UserRole
{
    SuperAdmin = 0,   // Platform administrator
    ClubManager = 1,  // Club committee member
    Member = 2,       // Regular club member
    Coach = 3,        // Coach/instructor
    Staff = 4         // Support staff
}
```

### Role Permissions

| Role | Can Access | Description |
|------|------------|-------------|
| **SuperAdmin** | `/admin/*` | Manage all clubs, system config |
| **ClubManager** | `/club/*` | Manage their club's data |
| **Member** | `/portal/*` | Self-service portal only |
| **Coach** | `/club/*` (limited) | Session management |
| **Staff** | `/club/*` (limited) | Read-only views |

---

## Login Flow

### 1. User Submits Credentials

```typescript
// Angular: auth.service.ts
login(email: string, password: string): Observable<AuthResponse> {
  return this.api.post<ApiResponse<AuthResponse>>('auth/login', { email, password })
    .pipe(
      map(response => {
        if (response.success && response.data) {
          localStorage.setItem('token', response.data.token);
          localStorage.setItem('refreshToken', response.data.refreshToken);
          localStorage.setItem('user', JSON.stringify(response.data.user));
          this.currentUserSubject.next(response.data.user);
        }
        return response.data!;
      })
    );
}
```

### 2. API Validates & Generates Token

```csharp
// TheLeague.Api/Services/AuthService.cs

public async Task<AuthResponse> LoginAsync(LoginDto dto)
{
    var user = await _userManager.FindByEmailAsync(dto.Email);
    if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
    {
        throw new UnauthorizedAccessException("Invalid email or password");
    }

    var token = GenerateJwtToken(user);
    var refreshToken = GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
    await _userManager.UpdateAsync(user);

    return new AuthResponse
    {
        Token = token,
        RefreshToken = refreshToken,
        User = MapToUserDto(user)
    };
}
```

### 3. JWT Token Structure

The token contains these claims:

```csharp
private string GenerateJwtToken(ApplicationUser user)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        new Claim("role", user.Role.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    // Add club context if user belongs to a club
    if (user.ClubId.HasValue)
    {
        claims.Add(new Claim("clubId", user.ClubId.Value.ToString()));
    }

    // Add member ID if user is a member
    if (user.MemberId.HasValue)
    {
        claims.Add(new Claim("memberId", user.MemberId.Value.ToString()));
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

### Decoded Token Example

```json
{
  "sub": "ff880734-5c1f-4736-ac20-225b27fb8b66",
  "email": "chairman@teddingtoncc.com",
  "name": "Richard Pemberton",
  "role": "ClubManager",
  "clubId": "f9b913a6-6796-4c5a-bbce-b7519bab9323",
  "jti": "c4683088-793f-474b-bb27-09555649241b",
  "exp": 1768819460,
  "iss": "TheLeague",
  "aud": "TheLeagueApp"
}
```

---

## Token Refresh

Access tokens expire after 15 minutes. Refresh tokens last 7 days.

```typescript
// Angular: auth.interceptor.ts

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('auth/refresh')) {
        // Try to refresh the token
        return authService.refreshToken().pipe(
          switchMap(() => {
            // Retry original request with new token
            const newReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${authService.getToken()}`
              }
            });
            return next(newReq);
          }),
          catchError(() => {
            authService.logout();
            return throwError(() => error);
          })
        );
      }
      return throwError(() => error);
    })
  );
};
```

---

## Authorization

### Backend: Controller Attributes

```csharp
// Require authentication
[Authorize]
public class MembersController : BaseApiController { }

// Require specific role
[Authorize(Roles = "SuperAdmin")]
public class AdminController : BaseApiController { }

// Require multiple roles (OR)
[Authorize(Roles = "SuperAdmin,ClubManager")]
public class ClubsController : BaseApiController { }

// Allow anonymous access
[AllowAnonymous]
[HttpPost("login")]
public async Task<IActionResult> Login(LoginDto dto) { }
```

### Frontend: Route Guards

```typescript
// app.routes.ts

{
  path: 'admin',
  component: AdminLayoutComponent,
  canActivate: [authGuard, superAdminGuard],  // Must be SuperAdmin
  children: [...]
},
{
  path: 'club',
  component: AdminLayoutComponent,
  canActivate: [authGuard, clubManagerGuard],  // ClubManager OR SuperAdmin
  children: [...]
},
{
  path: 'portal',
  component: PortalLayoutComponent,
  canActivate: [authGuard, memberGuard],  // Must be Member
  children: [...]
}
```

### Frontend: Conditional UI

```typescript
// In component
export class NavComponent {
  authService = inject(AuthService);

  get isAdmin(): boolean {
    return this.authService.hasRole('SuperAdmin');
  }

  get isClubManager(): boolean {
    return this.authService.hasRole('ClubManager') || this.isAdmin;
  }
}
```

```html
<!-- In template -->
@if (isAdmin) {
  <a routerLink="/admin">Admin Dashboard</a>
}

@if (isClubManager) {
  <a routerLink="/club/settings">Club Settings</a>
}
```

---

## Multi-Tenancy Security

### How Tenant Isolation Works

1. User logs in → JWT contains `clubId` claim
2. `TenantMiddleware` extracts `clubId` from token
3. `TenantService` provides `ClubId` to services
4. All queries filter by `ClubId`

```csharp
// TheLeague.Api/Middleware/TenantMiddleware.cs

public class TenantMiddleware
{
    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        var clubIdClaim = context.User.FindFirst("clubId");
        if (clubIdClaim != null && Guid.TryParse(clubIdClaim.Value, out var clubId))
        {
            tenantService.SetCurrentTenant(clubId);
        }

        await _next(context);
    }
}

// TheLeague.Infrastructure/Data/TenantService.cs

public class TenantService : ITenantService
{
    private Guid? _currentClubId;

    public Guid? CurrentClubId => _currentClubId;

    public void SetCurrentTenant(Guid clubId)
    {
        _currentClubId = clubId;
    }
}
```

### Service Layer Enforcement

```csharp
public async Task<MemberDetailDto> GetMemberByIdAsync(Guid clubId, Guid memberId)
{
    var member = await _context.Members
        .Where(m => m.ClubId == clubId && m.Id == memberId)  // ALWAYS filter by ClubId
        .FirstOrDefaultAsync();

    if (member == null)
        throw new KeyNotFoundException("Member not found");

    return MapToDto(member);
}
```

---

## Password Security

### Password Requirements

Configured in `Program.cs`:

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
```

### Password Reset Flow

```
1. User clicks "Forgot Password"
2. POST /api/auth/forgot-password { email }
3. API generates reset token, sends email (via EmailProvider)
4. User clicks email link with token
5. POST /api/auth/reset-password { token, newPassword }
6. API validates token, updates password
```

---

## CORS Configuration

```csharp
// TheLeague.Api/Program.cs

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

app.UseCors("Development");
```

---

## Security Headers

Recommended headers for production:

```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});
```

---

## Secrets Management

### Development

Secrets stored in `appsettings.json` (not committed for production):

```json
{
  "Jwt": {
    "Secret": "development-only-secret-key-min-32-chars"
  }
}
```

### Production

Use environment variables or Azure Key Vault:

```bash
# Environment variables
ConnectionStrings__DefaultConnection=Server=...
Jwt__Secret=production-secret-min-32-chars
```

---

## Quick Reference

### Token Claims

| Claim | Description |
|-------|-------------|
| `sub` | User ID |
| `email` | User email |
| `name` | Full name |
| `role` | UserRole enum value |
| `clubId` | Associated club (tenant) |
| `memberId` | Member record ID (if member) |
| `exp` | Token expiration time |

### Guard Usage

```typescript
// Require any authentication
canActivate: [authGuard]

// Require SuperAdmin
canActivate: [authGuard, superAdminGuard]

// Require ClubManager or SuperAdmin
canActivate: [authGuard, clubManagerGuard]

// Custom role check
canActivate: [authGuard, roleGuard(['Coach', 'ClubManager'])]
```

### Auth Service Methods

```typescript
authService.login(email, password)  // Log in
authService.logout()                 // Log out
authService.isLoggedIn              // Check if logged in
authService.currentUser             // Get current user
authService.hasRole('ClubManager')  // Check role
authService.getToken()              // Get JWT token
authService.refreshToken()          // Refresh token
```

---

## Next Steps

→ [08_KEY_FEATURES_WALKTHROUGH.md](./08_KEY_FEATURES_WALKTHROUGH.md) - Trace features end-to-end
