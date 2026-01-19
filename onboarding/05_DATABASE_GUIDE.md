# Database Guide

Everything about the data layer and Entity Framework Core.

---

## Technology Stack

| Component | Technology |
|-----------|------------|
| Database | SQL Server (LocalDB for dev, Azure SQL for prod) |
| ORM | Entity Framework Core 8 |
| Migrations | EF Core Code-First |
| Connection | Trusted Connection (Windows Auth for LocalDB) |

---

## Connection Setup

### Development (LocalDB)

```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TheLeagueDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Production (Azure SQL)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:yourserver.database.windows.net,1433;Database=TheLeagueDb;User ID=admin;Password=***;Encrypt=True;"
  }
}
```

---

## DbContext

The `ApplicationDbContext` is the gateway to the database:

```csharp
// TheLeague.Infrastructure/Data/ApplicationDbContext.cs

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // Core entities
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<FamilyMember> FamilyMembers { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<MembershipType> MembershipTypes { get; set; }

    // Activities
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionBooking> SessionBookings { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventTicket> EventTickets { get; set; }
    public DbSet<Venue> Venues { get; set; }

    // Financial
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Fee> Fees { get; set; }

    // ... 100+ more DbSets

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Entity configurations
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filters for soft delete (if used)
        // builder.Entity<Member>().HasQueryFilter(m => !m.IsDeleted);
    }
}
```

---

## Core Entity Overview

### Entity Relationship Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                           CLUB                                   │
│  (Tenant - all data belongs to a club)                          │
└──────────────────────────┬──────────────────────────────────────┘
                           │ 1:N
           ┌───────────────┼───────────────┬───────────────┐
           ▼               ▼               ▼               ▼
    ┌──────────┐    ┌──────────┐    ┌──────────┐    ┌──────────┐
    │  MEMBER  │    │ SESSION  │    │  EVENT   │    │  VENUE   │
    └────┬─────┘    └────┬─────┘    └────┬─────┘    └──────────┘
         │               │               │
    ┌────┴────┐     ┌────┴────┐     ┌────┴────┐
    ▼         ▼     ▼         ▼     ▼         ▼
┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐
│ FAMILY │ │MEMBER- │ │SESSION │ │ EVENT  │ │ EVENT  │
│ MEMBER │ │  SHIP  │ │BOOKING │ │ TICKET │ │  RSVP  │
└────────┘ └────┬───┘ └────────┘ └────────┘ └────────┘
                │
                ▼
          ┌──────────┐
          │MEMBERSHIP│
          │   TYPE   │
          └──────────┘
```

### Key Entities

#### Club (Tenant)

```csharp
public class Club
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }  // URL-friendly name
    public string? Description { get; set; }
    public ClubType ClubType { get; set; }  // Cricket, Football, Hockey...
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ClubSettings Settings { get; set; }
    public ICollection<Member> Members { get; set; }
    public ICollection<Session> Sessions { get; set; }
    public ICollection<Event> Events { get; set; }
}
```

#### Member

```csharp
public class Member
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }  // Tenant discriminator

    // Personal info
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }

    // Address
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }

    // Emergency contact
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }

    // Medical
    public string? MedicalConditions { get; set; }
    public string? Allergies { get; set; }

    // Status
    public MemberStatus Status { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Club Club { get; set; }
    public ApplicationUser? User { get; set; }
    public ICollection<FamilyMember> FamilyMembers { get; set; }
    public ICollection<Membership> Memberships { get; set; }
    public ICollection<SessionBooking> SessionBookings { get; set; }
}
```

#### Session

```csharp
public class Session
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }

    public string Title { get; set; }
    public string? Description { get; set; }
    public SessionCategory Category { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int Capacity { get; set; }
    public int CurrentBookings { get; set; }
    public decimal? SessionFee { get; set; }

    public bool IsCancelled { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public Club Club { get; set; }
    public Venue? Venue { get; set; }
    public ICollection<SessionBooking> Bookings { get; set; }
}
```

---

## Common Enums

```csharp
// TheLeague.Core/Enums/Enums.cs

public enum UserRole
{
    SuperAdmin,
    ClubManager,
    Member,
    Coach,
    Staff
}

public enum MemberStatus
{
    Pending,
    Active,
    Expired,
    Suspended,
    Cancelled
}

public enum ClubType
{
    Cricket,
    Football,
    Hockey,
    Rugby,
    Tennis,
    Golf,
    Swimming,
    Athletics,
    MultiSport,
    Other
}

public enum SessionCategory
{
    Training,
    Match,
    Fitness,
    Social,
    Coaching,
    Other
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded,
    PartiallyRefunded
}

public enum PaymentMethod
{
    Cash,
    BankTransfer,
    Card,
    Cheque,
    Online,
    DirectDebit,
    Other
}
```

---

## Migration Workflow

### View Current Migrations

```bash
cd TheLeague.Infrastructure
dotnet ef migrations list -s ../TheLeague.Api
```

### Create New Migration

```bash
dotnet ef migrations add <MigrationName> -s ../TheLeague.Api

# Example
dotnet ef migrations add AddMemberNotes -s ../TheLeague.Api
```

### Apply Migrations

```bash
dotnet ef database update -s ../TheLeague.Api
```

### Rollback Migration

```bash
# Rollback to specific migration
dotnet ef database update <PreviousMigrationName> -s ../TheLeague.Api

# Example: Rollback last migration
dotnet ef database update InitialCreate -s ../TheLeague.Api
```

### Generate SQL Script (for production)

```bash
dotnet ef migrations script --idempotent -o migration.sql -s ../TheLeague.Api
```

---

## Seeding

### How Seeding Works

The `DatabaseSeeder` runs on application startup if the database is empty:

```csharp
// TheLeague.Api/Services/DatabaseSeeder.cs

public class DatabaseSeeder
{
    public async Task SeedAsync()
    {
        // 1. Seed roles (SuperAdmin, ClubManager, Member)
        await SeedRolesAsync();

        // 2. Seed super admin user
        await SeedSuperAdminAsync();

        // 3. Load club data from seedData.json
        await LoadSeedDataAsync();

        // 4. For each club: seed members, sessions, events, etc.
        foreach (var club in _clubIdMap)
        {
            await SeedMembershipTypesAsync(club.Value, club.Key);
            await SeedVenuesAsync(club.Value, club.Key);
            await SeedMembersAsync(club.Value, club.Key);
            await SeedSessionsAsync(club.Value, club.Key);
            await SeedEventsAsync(club.Value, club.Key);
        }
    }
}
```

### Seed Data File

Demo data is configured in `seedData.json`:

```json
{
  "clubs": [
    {
      "id": "teddington-cc",
      "name": "Teddington Cricket Club",
      "clubType": "Cricket",
      "contactEmail": "secretary@teddingtoncc.com"
    },
    {
      "id": "highbury-fc",
      "name": "Highbury United FC",
      "clubType": "Football"
    }
  ],
  "members": [
    {
      "clubId": "teddington-cc",
      "firstName": "James",
      "lastName": "Anderson",
      "email": "james.anderson@email.com"
    }
  ]
}
```

### Reset Database with Fresh Seed

```bash
cd TheLeague.Infrastructure
dotnet ef database drop -s ../TheLeague.Api --force
dotnet ef database update -s ../TheLeague.Api
# Restart API to trigger seeding
```

---

## Data Access Patterns

### Tenant-Scoped Queries

Always filter by ClubId:

```csharp
// In service method
public async Task<List<MemberListDto>> GetMembersAsync(Guid clubId)
{
    return await _context.Members
        .Where(m => m.ClubId == clubId)  // ALWAYS filter by tenant
        .OrderBy(m => m.LastName)
        .Select(m => new MemberListDto { /* mapping */ })
        .ToListAsync();
}
```

### Including Related Data

Use `.Include()` for eager loading:

```csharp
var member = await _context.Members
    .Include(m => m.FamilyMembers)
    .Include(m => m.Memberships)
        .ThenInclude(ms => ms.MembershipType)
    .FirstOrDefaultAsync(m => m.Id == id && m.ClubId == clubId);
```

### Pagination

```csharp
var query = _context.Members.Where(m => m.ClubId == clubId);

var total = await query.CountAsync();
var items = await query
    .OrderBy(m => m.LastName)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

return new PagedResult<Member>(items, total, page, pageSize);
```

---

## Performance Considerations

### Use Select for DTOs

```csharp
// Good - only selects needed columns
var members = await _context.Members
    .Where(m => m.ClubId == clubId)
    .Select(m => new MemberListDto
    {
        Id = m.Id,
        FullName = m.FirstName + " " + m.LastName,
        Email = m.Email
    })
    .ToListAsync();

// Avoid - loads entire entity
var members = await _context.Members
    .Where(m => m.ClubId == clubId)
    .ToListAsync();  // Then mapping in memory
```

### Avoid N+1 Queries

```csharp
// Bad - N+1 problem
var members = await _context.Members.ToListAsync();
foreach (var member in members)
{
    var bookings = await _context.SessionBookings
        .Where(b => b.MemberId == member.Id)
        .ToListAsync();  // Query per member!
}

// Good - single query with Include
var members = await _context.Members
    .Include(m => m.SessionBookings)
    .ToListAsync();
```

### Index Key Columns

The `ClubId` column should be indexed on all tenant-scoped tables (handled via migrations).

---

## Quick Reference

### Common Queries

```csharp
// Get by ID with tenant check
var entity = await _context.Members
    .FirstOrDefaultAsync(m => m.Id == id && m.ClubId == clubId);

// Check existence
var exists = await _context.Members
    .AnyAsync(m => m.Email == email && m.ClubId == clubId);

// Count
var count = await _context.Members
    .CountAsync(m => m.ClubId == clubId && m.Status == MemberStatus.Active);

// Add new
_context.Members.Add(newMember);
await _context.SaveChangesAsync();

// Update
member.Status = MemberStatus.Active;
await _context.SaveChangesAsync();

// Delete
_context.Members.Remove(member);
await _context.SaveChangesAsync();
```

### Database Commands

```bash
# List migrations
dotnet ef migrations list -s ../TheLeague.Api

# Add migration
dotnet ef migrations add <Name> -s ../TheLeague.Api

# Apply migrations
dotnet ef database update -s ../TheLeague.Api

# Drop database
dotnet ef database drop -s ../TheLeague.Api --force

# Generate SQL
dotnet ef migrations script --idempotent -o script.sql -s ../TheLeague.Api
```

---

## Next Steps

→ [06_FRONTEND_GUIDE.md](./06_FRONTEND_GUIDE.md) - Explore the Angular client
