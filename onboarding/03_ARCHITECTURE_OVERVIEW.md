# Architecture Overview

This guide gives you the 10,000-foot view before diving into code.

---

## Architecture Style

The League follows a **Clean Architecture** pattern with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────────┐
│                         Clients                                  │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐ │
│  │ Angular SPA     │  │ Mobile (future) │  │ External APIs   │ │
│  │ (the-league-    │  │                 │  │                 │ │
│  │  client)        │  │                 │  │                 │ │
│  └────────┬────────┘  └────────┬────────┘  └────────┬────────┘ │
└───────────┼─────────────────────┼─────────────────────┼─────────┘
            │                     │                     │
            └─────────────────────┼─────────────────────┘
                                  │ HTTP/REST
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                      TheLeague.Api                               │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ Controllers (API Endpoints)                                  ││
│  │ - AuthController, MembersController, SessionsController...  ││
│  └─────────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ Middleware Pipeline                                          ││
│  │ - ExceptionHandling → Authentication → Tenant → Routing     ││
│  └─────────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ Services (Business Logic)                                    ││
│  │ - MemberService, PaymentService, SessionService...          ││
│  └─────────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ DTOs (Data Transfer Objects)                                 ││
│  │ - Request/Response models for API                           ││
│  └─────────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ Providers (External Integrations)                            ││
│  │ - StripePaymentProvider, SendGridEmailProvider              ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                     TheLeague.Core                               │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ Entities (Domain Models)                                     ││
│  │ - Member, Club, Session, Payment, Invoice...                ││
│  └─────────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ Enums                                                        ││
│  │ - UserRole, MemberStatus, PaymentStatus, SessionCategory... ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                  TheLeague.Infrastructure                        │
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ ApplicationDbContext (EF Core)                               ││
│  │ - 100+ DbSets, entity configurations, global query filters  ││
│  └─────────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ Migrations                                                   ││
│  │ - Database schema versioning                                ││
│  └─────────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────────┐│
│  │ TenantService                                                ││
│  │ - Multi-tenancy context management                          ││
│  └─────────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                      SQL Server                                  │
│                   (LocalDB / Azure SQL)                          │
└─────────────────────────────────────────────────────────────────┘
```

---

## Solution Structure

```
LeagueMembershipManagementPortal/
├── TheLeague.Api/                    # Web API (Entry Point)
│   ├── Controllers/                  # HTTP endpoints
│   ├── Services/                     # Business logic
│   ├── DTOs/                         # Request/Response models
│   ├── Middleware/                   # Pipeline components
│   ├── Providers/                    # External service integrations
│   └── Program.cs                    # Application startup
│
├── TheLeague.Core/                   # Domain Layer (No dependencies)
│   ├── Entities/                     # Database entities
│   └── Enums/                        # Enumeration types
│
├── TheLeague.Infrastructure/         # Data Access Layer
│   └── Data/
│       ├── ApplicationDbContext.cs   # EF Core context
│       ├── Migrations/               # Schema migrations
│       ├── TenantService.cs          # Multi-tenancy
│       └── DatabaseSeeder.cs         # Seed data
│
├── TheLeague.Tests/                  # Unit & Integration Tests
│
├── the-league-client/                # Angular Frontend
│   └── src/app/
│       ├── core/                     # Services, guards, interceptors
│       ├── features/                 # Feature modules
│       ├── shared/                   # Reusable components
│       └── layouts/                  # Page layouts
│
├── project-docs/                     # Planning documentation
├── onboarding/                       # You are here!
└── seedData.json                     # Demo data configuration
```

---

## Project Responsibilities

| Project | Responsibility | Dependencies |
|---------|----------------|--------------|
| **TheLeague.Api** | HTTP handling, DI setup, middleware, business services | Core, Infrastructure |
| **TheLeague.Core** | Domain entities, enums, business rules | None (pure domain) |
| **TheLeague.Infrastructure** | EF Core, database access, tenant management | Core |
| **TheLeague.Tests** | Unit and integration tests | Api, Core, Infrastructure |
| **the-league-client** | User interface, state management, API calls | None (separate app) |

---

## How Projects Communicate

```
┌─────────────────┐         ┌─────────────────┐
│  Angular SPA    │  HTTP   │  TheLeague.Api  │
│  (TypeScript)   │ ──────► │  (C#)           │
└─────────────────┘  REST   └────────┬────────┘
                                     │
                          Direct Reference
                                     │
                    ┌────────────────┼────────────────┐
                    ▼                ▼                ▼
           ┌──────────────┐  ┌──────────────┐  ┌──────────────┐
           │ TheLeague.   │  │ TheLeague.   │  │   External   │
           │ Core         │  │ Infrastructure│  │   Services   │
           │ (Entities)   │  │ (EF Core)    │  │   (Stripe,   │
           └──────────────┘  └──────────────┘  │   SendGrid)  │
                                     │         └──────────────┘
                                     │
                                     ▼
                              ┌──────────────┐
                              │  SQL Server  │
                              └──────────────┘
```

---

## Key Architectural Decisions

### 1. Multi-Tenant via ClubId Discriminator

**Why:** Shared database is simpler to manage than database-per-tenant.

**How:** Every tenant-scoped entity has a `ClubId` property. Services filter by the current tenant's ClubId.

```csharp
// In MemberService
var members = await _context.Members
    .Where(m => m.ClubId == currentClubId)  // Tenant isolation
    .ToListAsync();
```

### 2. JWT for Authentication

**Why:** Stateless authentication scales well, works across services.

**How:** Login returns a JWT with claims including `clubId`. Token is sent in Authorization header.

### 3. Clean Architecture (Services in API)

**Why:** Pragmatic approach - services live in API project for simplicity while maintaining separation.

**How:** Controllers are thin, services contain business logic, entities are pure.

### 4. Angular Standalone Components

**Why:** Simpler than NgModules, better tree-shaking, modern Angular pattern.

**How:** Components use `standalone: true` and import dependencies directly.

### 5. Provider Pattern for External Services

**Why:** Swappable implementations for payment (Stripe/PayPal/Mock) and email (SendGrid/Mock).

**How:** Factory pattern selects provider based on configuration.

---

## Request/Response Lifecycle

```
1. HTTP Request arrives
         │
         ▼
2. ExceptionHandlingMiddleware
   (Catches all errors, returns proper status codes)
         │
         ▼
3. Authentication Middleware
   (Validates JWT token)
         │
         ▼
4. TenantMiddleware
   (Extracts ClubId from JWT claims)
         │
         ▼
5. Routing
   (Matches to controller action)
         │
         ▼
6. Authorization
   ([Authorize] attributes check roles)
         │
         ▼
7. Controller Action
   (Receives request, calls service)
         │
         ▼
8. Service Layer
   (Business logic, data access via EF Core)
         │
         ▼
9. Database Query
   (Filtered by ClubId via TenantService)
         │
         ▼
10. Response DTO mapping
          │
          ▼
11. HTTP Response returned
```

---

## External Dependencies

```
┌─────────────────────────────────────────────────────────────────┐
│                      The League Platform                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────┐     ┌──────────┐     ┌──────────┐                │
│  │  Stripe  │     │ SendGrid │     │  PayPal  │                │
│  │ Payments │     │  Email   │     │ Payments │                │
│  └────┬─────┘     └────┬─────┘     └────┬─────┘                │
│       │                │                │                       │
│       └────────────────┼────────────────┘                       │
│                        │                                         │
│              ┌─────────▼─────────┐                              │
│              │  Provider Layer   │                              │
│              │ (Factory Pattern) │                              │
│              └───────────────────┘                              │
│                                                                  │
│  Current: Mock providers enabled for development                 │
│  Production: Configure real API keys in appsettings             │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Data Flow Example: Member Creation

```
┌─────────────┐    POST /api/members    ┌─────────────────┐
│   Angular   │ ─────────────────────► │ MembersController│
│   Form      │    { name, email, ...}  └────────┬────────┘
└─────────────┘                                  │
                                                 │ CreateAsync(dto)
                                                 ▼
                                        ┌─────────────────┐
                                        │  MemberService  │
                                        │                 │
                                        │ 1. Validate DTO │
                                        │ 2. Get ClubId   │
                                        │ 3. Create Entity│
                                        │ 4. Save to DB   │
                                        └────────┬────────┘
                                                 │
                                                 ▼
                                        ┌─────────────────┐
                                        │ DbContext       │
                                        │ Members.Add()   │
                                        │ SaveChanges()   │
                                        └────────┬────────┘
                                                 │
                                                 ▼
                                        ┌─────────────────┐
                                        │   SQL Server    │
                                        │ INSERT INTO     │
                                        │ Members...      │
                                        └─────────────────┘
```

---

## Quick Reference

### Key Files to Know

| Purpose | File Path |
|---------|-----------|
| API Entry Point | `TheLeague.Api/Program.cs` |
| All Controllers | `TheLeague.Api/Controllers/` |
| All Services | `TheLeague.Api/Services/` |
| All Entities | `TheLeague.Core/Entities/` |
| All Enums | `TheLeague.Core/Enums/Enums.cs` |
| DbContext | `TheLeague.Infrastructure/Data/ApplicationDbContext.cs` |
| Multi-Tenancy | `TheLeague.Infrastructure/Data/TenantService.cs` |
| Angular Routes | `the-league-client/src/app/app.routes.ts` |
| Angular Services | `the-league-client/src/app/core/services/` |

### Ports

| Service | Port | URL |
|---------|------|-----|
| Backend API | 7000 | http://localhost:7000 |
| Frontend | 4200 | http://localhost:4200 |
| Swagger | 7000 | http://localhost:7000/swagger |

---

## Next Steps

Ready for the deep dives:

→ [04_BACKEND_GUIDE.md](./04_BACKEND_GUIDE.md) - Explore the API layer in detail
