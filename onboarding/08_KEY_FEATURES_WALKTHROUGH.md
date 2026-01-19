# Key Features Walkthrough

Trace core features end-to-end to understand how the pieces connect.

---

## Feature 1: Member Registration

Creating a new club member - from form to database.

### User Journey

1. Club Manager logs in
2. Navigates to Members → Add Member
3. Fills out member form
4. Clicks Save
5. Member appears in list

### Code Flow

```
┌─────────────────┐
│ 1. Angular Form │  the-league-client/src/app/features/club/
│    Component    │  members/member-form/member-form.component.ts
└────────┬────────┘
         │ Form data submitted
         ▼
┌─────────────────┐
│ 2. Member       │  the-league-client/src/app/core/services/
│    Service      │  member.service.ts
└────────┬────────┘
         │ POST /api/members
         ▼
┌─────────────────┐
│ 3. Members      │  TheLeague.Api/Controllers/
│    Controller   │  MembersController.cs
└────────┬────────┘
         │ CreateMember(dto)
         ▼
┌─────────────────┐
│ 4. Member       │  TheLeague.Api/Services/
│    Service      │  MemberService.cs
└────────┬────────┘
         │ _context.Members.Add()
         ▼
┌─────────────────┐
│ 5. Database     │  TheLeague.Infrastructure/Data/
│    Context      │  ApplicationDbContext.cs
└────────┬────────┘
         │ SaveChangesAsync()
         ▼
┌─────────────────┐
│ 6. SQL Server   │
│    INSERT       │
└─────────────────┘
```

### Frontend: Member Form Component

```typescript
// the-league-client/src/app/features/club/members/member-form/member-form.component.ts

@Component({
  selector: 'app-member-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink]
})
export class MemberFormComponent implements OnInit {
  memberForm!: FormGroup;
  isEditMode = false;
  memberId?: string;

  constructor(
    private fb: FormBuilder,
    private memberService: MemberService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.initForm();

    // Check if editing existing member
    this.memberId = this.route.snapshot.params['id'];
    if (this.memberId) {
      this.isEditMode = true;
      this.loadMember();
    }
  }

  private initForm() {
    this.memberForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      dateOfBirth: [''],
      addressLine1: [''],
      city: [''],
      postCode: ['']
    });
  }

  onSubmit() {
    if (this.memberForm.invalid) return;

    const memberData = this.memberForm.value;

    if (this.isEditMode) {
      this.memberService.updateMember(this.memberId!, memberData).subscribe({
        next: () => this.router.navigate(['/club/members']),
        error: (err) => console.error('Update failed', err)
      });
    } else {
      this.memberService.createMember(memberData).subscribe({
        next: () => this.router.navigate(['/club/members']),
        error: (err) => console.error('Create failed', err)
      });
    }
  }
}
```

### Frontend: Member Service

```typescript
// the-league-client/src/app/core/services/member.service.ts

@Injectable({ providedIn: 'root' })
export class MemberService {
  constructor(private api: ApiService) {}

  createMember(member: CreateMemberDto): Observable<MemberDetailDto> {
    return this.api.post<MemberDetailDto>('members', member);
  }

  updateMember(id: string, member: UpdateMemberDto): Observable<MemberDetailDto> {
    return this.api.put<MemberDetailDto>(`members/${id}`, member);
  }

  getMember(id: string): Observable<MemberDetailDto> {
    return this.api.get<MemberDetailDto>(`members/${id}`);
  }

  getMembers(page = 1, pageSize = 20, search?: string): Observable<PagedResult<MemberListDto>> {
    let params = `?page=${page}&pageSize=${pageSize}`;
    if (search) params += `&search=${encodeURIComponent(search)}`;
    return this.api.get<PagedResult<MemberListDto>>(`members${params}`);
  }
}
```

### Backend: Members Controller

```csharp
// TheLeague.Api/Controllers/MembersController.cs

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : BaseApiController
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpPost]
    public async Task<ActionResult<MemberDetailDto>> CreateMember(CreateMemberDto dto)
    {
        var clubId = GetClubId();  // From JWT claims
        var member = await _memberService.CreateMemberAsync(clubId, dto);
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDetailDto>> GetMember(Guid id)
    {
        var clubId = GetClubId();
        var member = await _memberService.GetMemberByIdAsync(clubId, id);
        return Ok(member);
    }
}
```

### Backend: Member Service

```csharp
// TheLeague.Api/Services/MemberService.cs

public class MemberService : IMemberService
{
    private readonly ApplicationDbContext _context;

    public async Task<MemberDetailDto> CreateMemberAsync(Guid clubId, CreateMemberDto dto)
    {
        // Validate email uniqueness within club
        var existingMember = await _context.Members
            .AnyAsync(m => m.ClubId == clubId && m.Email == dto.Email);

        if (existingMember)
            throw new ArgumentException("A member with this email already exists");

        // Create entity
        var member = new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,  // Tenant isolation
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            AddressLine1 = dto.AddressLine1,
            City = dto.City,
            PostCode = dto.PostCode,
            Status = MemberStatus.Pending,
            JoinedDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        return MapToDetailDto(member);
    }
}
```

---

## Feature 2: Session Booking

Members book spots in training sessions.

### User Journey

1. Member logs into portal
2. Views available sessions
3. Clicks "Book" on a session
4. Receives confirmation
5. Session attendance count updates

### Code Flow

```
┌─────────────────┐
│ 1. Portal       │  the-league-client/src/app/features/portal/
│    Sessions     │  sessions/sessions.component.ts
└────────┬────────┘
         │ Click "Book Session"
         ▼
┌─────────────────┐
│ 2. Session      │  the-league-client/src/app/core/services/
│    Service      │  session.service.ts
└────────┬────────┘
         │ POST /api/sessions/{id}/book
         ▼
┌─────────────────┐
│ 3. Sessions     │  TheLeague.Api/Controllers/
│    Controller   │  SessionsController.cs
└────────┬────────┘
         │ BookSession(id)
         ▼
┌─────────────────┐
│ 4. Session      │  TheLeague.Api/Services/
│    Service      │  SessionService.cs
└────────┬────────┘
         │ Validates capacity
         │ Creates SessionBooking
         │ Updates CurrentBookings
         ▼
┌─────────────────┐
│ 5. Database     │
│    Transaction  │
└─────────────────┘
```

### Frontend: Session List with Booking

```typescript
// the-league-client/src/app/features/portal/sessions/sessions.component.ts

@Component({
  selector: 'app-portal-sessions',
  standalone: true,
  template: `
    <div class="session-list">
      @for (session of sessions; track session.id) {
        <div class="session-card">
          <h3>{{ session.title }}</h3>
          <p>{{ session.startTime | date:'medium' }}</p>
          <p>{{ session.currentBookings }}/{{ session.capacity }} booked</p>

          @if (session.isBooked) {
            <button (click)="cancelBooking(session.id)" class="btn-secondary">
              Cancel Booking
            </button>
          } @else if (session.currentBookings < session.capacity) {
            <button (click)="bookSession(session.id)" class="btn-primary">
              Book Session
            </button>
          } @else {
            <span class="badge-full">Full</span>
          }
        </div>
      }
    </div>
  `
})
export class PortalSessionsComponent implements OnInit {
  sessions: SessionDto[] = [];

  constructor(private sessionService: SessionService) {}

  ngOnInit() {
    this.loadSessions();
  }

  loadSessions() {
    this.sessionService.getUpcomingSessions().subscribe({
      next: (sessions) => this.sessions = sessions,
      error: (err) => console.error('Failed to load sessions', err)
    });
  }

  bookSession(sessionId: string) {
    this.sessionService.bookSession(sessionId).subscribe({
      next: () => {
        this.loadSessions();  // Refresh to show updated booking
      },
      error: (err) => console.error('Booking failed', err)
    });
  }

  cancelBooking(sessionId: string) {
    this.sessionService.cancelBooking(sessionId).subscribe({
      next: () => this.loadSessions(),
      error: (err) => console.error('Cancellation failed', err)
    });
  }
}
```

### Backend: Session Booking Logic

```csharp
// TheLeague.Api/Services/SessionService.cs

public class SessionService : ISessionService
{
    public async Task<SessionBookingDto> BookSessionAsync(Guid clubId, Guid sessionId, Guid memberId)
    {
        // Use transaction for consistency
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Get session with lock
            var session = await _context.Sessions
                .FirstOrDefaultAsync(s => s.Id == sessionId && s.ClubId == clubId);

            if (session == null)
                throw new KeyNotFoundException("Session not found");

            if (session.IsCancelled)
                throw new InvalidOperationException("Cannot book a cancelled session");

            // Check capacity
            if (session.CurrentBookings >= session.Capacity)
                throw new InvalidOperationException("Session is full");

            // Check if already booked
            var existingBooking = await _context.SessionBookings
                .AnyAsync(b => b.SessionId == sessionId && b.MemberId == memberId && !b.IsCancelled);

            if (existingBooking)
                throw new InvalidOperationException("Already booked for this session");

            // Create booking
            var booking = new SessionBooking
            {
                Id = Guid.NewGuid(),
                SessionId = sessionId,
                MemberId = memberId,
                BookedAt = DateTime.UtcNow,
                IsCancelled = false
            };

            _context.SessionBookings.Add(booking);

            // Update session count
            session.CurrentBookings++;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return MapToDto(booking);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```

---

## Feature 3: Payment Recording

Recording membership fees or session payments.

### User Journey

1. Club Manager navigates to Payments
2. Clicks "Record Payment"
3. Selects member, amount, method
4. Saves payment
5. Payment appears in list and member's history

### Code Flow

```
┌─────────────────┐
│ 1. Payment Form │  the-league-client/src/app/features/club/
│    Component    │  payments/payment-form.component.ts
└────────┬────────┘
         │ Submit payment data
         ▼
┌─────────────────┐
│ 2. Payment      │  the-league-client/src/app/core/services/
│    Service      │  payment.service.ts
└────────┬────────┘
         │ POST /api/payments
         ▼
┌─────────────────┐
│ 3. Payments     │  TheLeague.Api/Controllers/
│    Controller   │  PaymentsController.cs
└────────┬────────┘
         │ RecordPayment(dto)
         ▼
┌─────────────────┐
│ 4. Payment      │  TheLeague.Api/Services/
│    Service      │  PaymentService.cs
└────────┬────────┘
         │ Creates Payment record
         │ Optionally updates Membership
         ▼
┌─────────────────┐
│ 5. Database     │
│    INSERT       │
└─────────────────┘
```

### Backend: Payment Service

```csharp
// TheLeague.Api/Services/PaymentService.cs

public class PaymentService : IPaymentService
{
    public async Task<PaymentDto> RecordPaymentAsync(Guid clubId, CreatePaymentDto dto)
    {
        // Validate member exists
        var member = await _context.Members
            .FirstOrDefaultAsync(m => m.Id == dto.MemberId && m.ClubId == clubId);

        if (member == null)
            throw new KeyNotFoundException("Member not found");

        // Create payment record
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = dto.MemberId,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod,
            PaymentType = dto.PaymentType,
            Description = dto.Description,
            PaymentDate = dto.PaymentDate ?? DateTime.UtcNow,
            Status = PaymentStatus.Completed,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        // If this is a membership payment, update membership status
        if (dto.MembershipId.HasValue)
        {
            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.Id == dto.MembershipId.Value);

            if (membership != null)
            {
                membership.IsPaid = true;
                membership.PaymentId = payment.Id;
            }
        }

        await _context.SaveChangesAsync();

        return MapToDto(payment);
    }

    public async Task<PagedResult<PaymentListDto>> GetPaymentsAsync(
        Guid clubId, int page, int pageSize, string? search)
    {
        var query = _context.Payments
            .Include(p => p.Member)
            .Where(p => p.ClubId == clubId);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p =>
                p.Member.FirstName.Contains(search) ||
                p.Member.LastName.Contains(search) ||
                p.Description.Contains(search));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.PaymentDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PaymentListDto
            {
                Id = p.Id,
                MemberName = p.Member.FirstName + " " + p.Member.LastName,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod.ToString(),
                PaymentDate = p.PaymentDate,
                Status = p.Status.ToString()
            })
            .ToListAsync();

        return new PagedResult<PaymentListDto>(items, total, page, pageSize);
    }
}
```

---

## Feature 4: Dashboard Statistics

Real-time statistics on the club dashboard.

### Data Flow

```
┌─────────────────┐
│ Dashboard       │  the-league-client/src/app/features/club/
│ Component       │  dashboard/dashboard.component.ts
└────────┬────────┘
         │ ngOnInit()
         ▼
┌─────────────────┐
│ Dashboard       │  the-league-client/src/app/core/services/
│ Service         │  dashboard.service.ts
└────────┬────────┘
         │ GET /api/club/dashboard
         ▼
┌─────────────────┐
│ Club            │  TheLeague.Api/Controllers/
│ Controller      │  ClubController.cs
└────────┬────────┘
         │ GetDashboard()
         ▼
┌─────────────────┐
│ Dashboard       │  TheLeague.Api/Services/
│ Service         │  DashboardService.cs
└────────┬────────┘
         │ Aggregation queries
         ▼
┌─────────────────┐
│ Multiple        │
│ COUNT queries   │
└─────────────────┘
```

### Backend: Dashboard Aggregation

```csharp
// TheLeague.Api/Services/DashboardService.cs

public class DashboardService : IDashboardService
{
    public async Task<ClubDashboardDto> GetClubDashboardAsync(Guid clubId)
    {
        // Run queries in parallel for performance
        var membersTask = _context.Members
            .CountAsync(m => m.ClubId == clubId);

        var activeMembersTask = _context.Members
            .CountAsync(m => m.ClubId == clubId && m.Status == MemberStatus.Active);

        var upcomingSessionsTask = _context.Sessions
            .CountAsync(s => s.ClubId == clubId && s.StartTime > DateTime.UtcNow && !s.IsCancelled);

        var upcomingEventsTask = _context.Events
            .CountAsync(e => e.ClubId == clubId && e.StartDate > DateTime.UtcNow);

        var recentPaymentsTask = _context.Payments
            .Where(p => p.ClubId == clubId && p.PaymentDate >= DateTime.UtcNow.AddDays(-30))
            .SumAsync(p => p.Amount);

        // Await all
        await Task.WhenAll(
            membersTask, activeMembersTask, upcomingSessionsTask,
            upcomingEventsTask, recentPaymentsTask
        );

        return new ClubDashboardDto
        {
            TotalMembers = await membersTask,
            ActiveMembers = await activeMembersTask,
            UpcomingSessions = await upcomingSessionsTask,
            UpcomingEvents = await upcomingEventsTask,
            RevenueThisMonth = await recentPaymentsTask,

            // Recent activity
            RecentMembers = await GetRecentMembersAsync(clubId, 5),
            UpcomingSessionsList = await GetUpcomingSessionsAsync(clubId, 5)
        };
    }
}
```

---

## Feature 5: Multi-Tenant Data Isolation

How the same code serves different clubs securely.

### Flow

```
┌─────────────────┐
│ Club A Manager  │  JWT: { clubId: "club-a-id", role: "ClubManager" }
│ logs in         │
└────────┬────────┘
         │
         ▼
┌─────────────────────────────────────────────────────────────────┐
│                      TenantMiddleware                            │
│  1. Extract "clubId" claim from JWT                              │
│  2. Call TenantService.SetCurrentTenant(clubId)                  │
└────────┬────────────────────────────────────────────────────────┘
         │
         ▼
┌─────────────────────────────────────────────────────────────────┐
│                      MembersController                           │
│  var clubId = GetClubId();  // Returns "club-a-id"               │
│  var members = await _service.GetMembersAsync(clubId, ...);     │
└────────┬────────────────────────────────────────────────────────┘
         │
         ▼
┌─────────────────────────────────────────────────────────────────┐
│                      MemberService                               │
│  _context.Members.Where(m => m.ClubId == clubId)                │
│  // Only returns Club A's members                                │
└─────────────────────────────────────────────────────────────────┘
```

### Why This Works

1. **JWT Contains ClubId**: When a user logs in, their club association is encoded in the token
2. **Every Request Includes Token**: The Angular app sends the JWT with every API call
3. **Middleware Extracts Context**: TenantMiddleware reads the clubId from claims
4. **Controllers Use GetClubId()**: Base controller method retrieves the current tenant
5. **Services Always Filter**: Every query includes `WHERE ClubId = @clubId`

### Security Guarantee

```csharp
// A Club B manager CANNOT access Club A data because:

// 1. Their JWT contains clubId = "club-b-id"
// 2. GetClubId() returns "club-b-id"
// 3. Query filters by "club-b-id"
// 4. Club A data is never returned

// Even if they guess Club A's member ID:
GET /api/members/{club-a-member-id}

// The service checks BOTH conditions:
var member = await _context.Members
    .FirstOrDefaultAsync(m => m.Id == id && m.ClubId == clubId);
    //                                       ^^^^^^^^^^^^^^^^
    // clubId is "club-b-id", so this returns null
    // Result: 404 Not Found
```

---

## Quick Reference: Common Modifications

### Add a Field to Member

1. Add property to `TheLeague.Core/Entities/Member.cs`
2. Create migration: `dotnet ef migrations add AddMemberField`
3. Update DTOs in `TheLeague.Api/DTOs/Members/`
4. Update service mapping in `MemberService.cs`
5. Add field to Angular form component
6. Update TypeScript interfaces

### Add a New Entity

1. Create entity in `TheLeague.Core/Entities/`
2. Add DbSet to `ApplicationDbContext`
3. Create migration
4. Create DTOs (Create, Update, List, Detail)
5. Create service interface and implementation
6. Register service in `Program.cs`
7. Create controller
8. Create Angular service and components

### Add a New API Endpoint

1. Add method to service interface
2. Implement in service class
3. Add controller action
4. Test via Swagger
5. Add Angular service method
6. Call from component

---

## Next Steps

→ [09_SUPPORT_KNOWLEDGE.md](./09_SUPPORT_KNOWLEDGE.md) - Troubleshooting and debugging

