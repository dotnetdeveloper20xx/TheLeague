# The League - Complete Project Implementation Document

## Project Overview

**Project Name:** The League  
**Tagline:** Run Your Club Like a Business  
**Description:** A complete multi-tenant membership management system for sports clubs and community groups.

**Target Users:**
- Cricket Clubs
- Football Clubs
- Rugby Clubs
- Tennis Clubs
- Golf Clubs
- Community Groups
- Youth Sports Organizations

---

## Technology Stack

| Layer | Technology |
|-------|------------|
| Backend API | ASP.NET Core 8.0 Web API |
| Frontend | Angular 17+ (Standalone Components) |
| Database | Entity Framework Core with In-Memory Provider (SQL Server schema) |
| Authentication | ASP.NET Core Identity with JWT tokens |
| Payment Processing | Stripe & PayPal (configurable per club) |
| Email | SendGrid |
| File Storage | Local file system (Azure Blob Storage ready) |
| Charts/Reports | Chart.js via ng2-charts |
| CSS Framework | Tailwind CSS (Mobile-First Design) |

---

## Architecture Overview

### Multi-Tenancy Strategy
- **Shared Database with TenantId** - All tables include a `TenantId` (ClubId) column
- **Tenant Resolution** - Via subdomain (e.g., `cricketclub.theleague.com`) or header
- **Data Isolation** - Global query filters ensure clubs only see their own data

### User Roles
| Role | Description | Permissions |
|------|-------------|-------------|
| **Super Admin** | System administrator | Manage all clubs, system settings, view all data |
| **Club Manager** | Club administrator | Full control over their club's data, members, settings |
| **Member** | Regular club member | Self-service: profile, bookings, payments, events |

### Family Account Structure
- One primary account holder (adult)
- Linked family member profiles (children, spouse)
- Primary account manages all family payments
- Each family member can have individual bookings/attendance records

---

## Database Schema

### Core Entities

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              CORE ENTITIES                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌─────────────┐     ┌─────────────────┐     ┌──────────────────┐          │
│  │   Club      │────<│ MembershipType  │     │ ClubSettings     │          │
│  │  (Tenant)   │     └─────────────────┘     └──────────────────┘          │
│  └──────┬──────┘                                                            │
│         │                                                                    │
│         │ 1:N                                                                │
│         ▼                                                                    │
│  ┌─────────────┐     ┌─────────────────┐     ┌──────────────────┐          │
│  │   Member    │────<│ FamilyMember    │     │ MemberDocument   │          │
│  └──────┬──────┘     └─────────────────┘     └──────────────────┘          │
│         │                                                                    │
│         │ 1:N                                                                │
│         ▼                                                                    │
│  ┌─────────────┐     ┌─────────────────┐     ┌──────────────────┐          │
│  │ Membership  │────>│    Payment      │     │ PaymentReminder  │          │
│  └─────────────┘     └─────────────────┘     └──────────────────┘          │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                           BOOKING & EVENTS                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌─────────────┐     ┌─────────────────┐     ┌──────────────────┐          │
│  │  Session    │────<│ SessionBooking  │     │    Waitlist      │          │
│  │ (Training)  │     └─────────────────┘     └──────────────────┘          │
│  └─────────────┘                                                            │
│                                                                              │
│  ┌─────────────┐     ┌─────────────────┐     ┌──────────────────┐          │
│  │   Event     │────<│  EventTicket    │     │   EventRSVP      │          │
│  └─────────────┘     └─────────────────┘     └──────────────────┘          │
│                                                                              │
│  ┌─────────────┐     ┌─────────────────┐                                    │
│  │   Venue     │────<│ VenueBooking    │                                    │
│  └─────────────┘     └─────────────────┘                                    │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                           RECURRING BOOKINGS                                 │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌───────────────────┐     ┌─────────────────────────┐                      │
│  │ RecurringBooking  │────<│ RecurringBookingInstance│                      │
│  │ (e.g. Every Tue)  │     │  (Generated sessions)   │                      │
│  └───────────────────┘     └─────────────────────────┘                      │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Detailed Entity Definitions

#### Club (Tenant)
```csharp
public class Club
{
    public Guid Id { get; set; }
    public string Name { get; set; }                    // "Willow Creek Cricket Club"
    public string Slug { get; set; }                    // "willow-creek" (for subdomain)
    public string Description { get; set; }
    public string LogoUrl { get; set; }
    public string PrimaryColor { get; set; }            // "#1E40AF" (hex)
    public string SecondaryColor { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public ClubType ClubType { get; set; }              // Cricket, Football, Rugby, etc.
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RenewalDate { get; set; }          // Annual renewal season start
    
    // Payment Settings
    public PaymentProvider PreferredPaymentProvider { get; set; }  // Stripe or PayPal
    public string StripeAccountId { get; set; }
    public string PayPalClientId { get; set; }
    
    // SendGrid Settings
    public string SendGridApiKey { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    
    // Navigation
    public ICollection<Member> Members { get; set; }
    public ICollection<MembershipType> MembershipTypes { get; set; }
    public ICollection<Session> Sessions { get; set; }
    public ICollection<Event> Events { get; set; }
    public ICollection<Venue> Venues { get; set; }
    public ClubSettings Settings { get; set; }
}

public enum ClubType
{
    Cricket, Football, Rugby, Tennis, Golf, Hockey,
    Swimming, Athletics, MultiSport, CommunityGroup, YouthOrganization, Other
}

public enum PaymentProvider
{
    Stripe,
    PayPal
}
```

#### ClubSettings
```csharp
public class ClubSettings
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    
    // Registration Settings
    public bool AllowOnlineRegistration { get; set; } = true;
    public bool RequireEmergencyContact { get; set; } = true;
    public bool RequireMedicalInfo { get; set; } = false;
    public bool AllowFamilyAccounts { get; set; } = true;
    
    // Payment Settings
    public bool AllowOnlinePayments { get; set; } = true;
    public bool AllowManualPayments { get; set; } = true;      // Cash/Bank Transfer
    public bool AutoSendPaymentReminders { get; set; } = true;
    public int PaymentReminderDaysBefore { get; set; } = 14;   // Days before due date
    public int PaymentReminderFrequency { get; set; } = 7;     // Days between reminders
    
    // Booking Settings
    public bool AllowMemberBookings { get; set; } = true;
    public int MaxAdvanceBookingDays { get; set; } = 30;
    public int CancellationNoticePeriodHours { get; set; } = 24;
    public bool EnableWaitlist { get; set; } = true;
    
    // Communication Settings
    public bool SendWelcomeEmail { get; set; } = true;
    public bool SendBookingConfirmations { get; set; } = true;
    public bool SendPaymentReceipts { get; set; } = true;
    
    public Club Club { get; set; }
}
```

#### Member
```csharp
public class Member
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }                    // Tenant ID
    public string UserId { get; set; }                  // ASP.NET Identity User ID
    
    // Personal Details
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string ProfilePhotoUrl { get; set; }
    
    // Emergency Contact
    public string EmergencyContactName { get; set; }
    public string EmergencyContactPhone { get; set; }
    public string EmergencyContactRelation { get; set; }
    
    // Medical Info (optional)
    public string MedicalConditions { get; set; }
    public string Allergies { get; set; }
    public string DoctorName { get; set; }
    public string DoctorPhone { get; set; }
    
    // Family Account
    public bool IsFamilyAccount { get; set; }
    public Guid? PrimaryMemberId { get; set; }          // If this is a family member
    public Member PrimaryMember { get; set; }
    public ICollection<FamilyMember> FamilyMembers { get; set; }
    
    // Status
    public MemberStatus Status { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; }
    
    // Stripe/PayPal Customer IDs
    public string StripeCustomerId { get; set; }
    public string PayPalPayerId { get; set; }
    
    // Navigation
    public Club Club { get; set; }
    public ICollection<Membership> Memberships { get; set; }
    public ICollection<SessionBooking> SessionBookings { get; set; }
    public ICollection<EventTicket> EventTickets { get; set; }
    public ICollection<MemberDocument> Documents { get; set; }
}

public enum MemberStatus
{
    Pending,            // Registered but not paid
    Active,             // Paid and current
    Expired,            // Membership lapsed
    Suspended,          // Manually suspended
    Cancelled           // Left the club
}
```

#### FamilyMember
```csharp
public class FamilyMember
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid PrimaryMemberId { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public FamilyMemberRelation Relation { get; set; }
    public string MedicalConditions { get; set; }
    public string Allergies { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Member PrimaryMember { get; set; }
    public ICollection<SessionBooking> SessionBookings { get; set; }
}

public enum FamilyMemberRelation
{
    Spouse, Child, Sibling, Parent, Other
}
```

#### MembershipType
```csharp
public class MembershipType
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    
    public string Name { get; set; }                    // "Senior", "Junior", "Family"
    public string Description { get; set; }
    public decimal AnnualFee { get; set; }
    public decimal? MonthlyFee { get; set; }            // If monthly option available
    public decimal? SessionFee { get; set; }            // Pay-as-you-go rate
    
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? MaxFamilyMembers { get; set; }          // For family memberships
    
    public bool IsActive { get; set; } = true;
    public bool AllowOnlineSignup { get; set; } = true;
    public int SortOrder { get; set; }
    
    // Benefits/Features
    public bool IncludesBooking { get; set; } = true;
    public bool IncludesEvents { get; set; } = true;
    public int? MaxSessionsPerWeek { get; set; }
    
    public Club Club { get; set; }
    public ICollection<Membership> Memberships { get; set; }
}
```

#### Membership
```csharp
public class Membership
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid MembershipTypeId { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MembershipPaymentType PaymentType { get; set; }  // Annual, Monthly, PayAsYouGo
    public MembershipStatus Status { get; set; }
    
    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }
    
    public bool AutoRenew { get; set; }
    public string Notes { get; set; }
    
    public Member Member { get; set; }
    public MembershipType MembershipType { get; set; }
    public ICollection<Payment> Payments { get; set; }
}

public enum MembershipPaymentType
{
    Annual,
    Monthly,
    PayAsYouGo
}

public enum MembershipStatus
{
    Active,
    PendingPayment,
    Expired,
    Cancelled
}
```

#### Payment
```csharp
public class Payment
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? MembershipId { get; set; }
    public Guid? EventTicketId { get; set; }
    
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentType Type { get; set; }               // Membership, Event, Session, Other
    
    public string Description { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime? ProcessedDate { get; set; }
    
    // External References
    public string StripePaymentIntentId { get; set; }
    public string PayPalTransactionId { get; set; }
    public string ReceiptNumber { get; set; }
    public string ReceiptUrl { get; set; }
    
    // For manual payments
    public string ManualPaymentReference { get; set; }  // e.g., "Bank transfer ref: ABC123"
    public string RecordedBy { get; set; }              // Admin who recorded manual payment
    
    public Member Member { get; set; }
    public Membership Membership { get; set; }
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded,
    Cancelled
}

public enum PaymentMethod
{
    Stripe,
    PayPal,
    BankTransfer,
    Cash,
    Cheque,
    Other
}

public enum PaymentType
{
    Membership,
    EventTicket,
    SessionFee,
    Other
}
```

#### Session (Training/Practice)
```csharp
public class Session
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }
    
    public string Title { get; set; }                   // "U13s Training"
    public string Description { get; set; }
    public SessionCategory Category { get; set; }       // U11, U13, Seniors, etc.
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public int CurrentBookings { get; set; }
    
    public bool IsRecurring { get; set; }
    public Guid? RecurringScheduleId { get; set; }
    
    public decimal? SessionFee { get; set; }            // Override club default
    public bool IsCancelled { get; set; }
    public string CancellationReason { get; set; }
    
    public Club Club { get; set; }
    public Venue Venue { get; set; }
    public RecurringSchedule RecurringSchedule { get; set; }
    public ICollection<SessionBooking> Bookings { get; set; }
    public ICollection<Waitlist> WaitlistEntries { get; set; }
}

public enum SessionCategory
{
    AllAges, Juniors, Seniors, U7, U9, U11, U13, U15, U17, U19,
    Ladies, Mens, Mixed, Beginners, Advanced, Social, Competition
}
```

#### RecurringSchedule
```csharp
public class RecurringSchedule
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public SessionCategory Category { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int Capacity { get; set; }
    
    public DateTime ScheduleStartDate { get; set; }
    public DateTime? ScheduleEndDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    public decimal? SessionFee { get; set; }
    
    public ICollection<Session> GeneratedSessions { get; set; }
}
```

#### SessionBooking
```csharp
public class SessionBooking
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid SessionId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }           // If booking for family member
    
    public DateTime BookedAt { get; set; }
    public BookingStatus Status { get; set; }
    public bool Attended { get; set; }
    public DateTime? CheckedInAt { get; set; }
    
    public string Notes { get; set; }
    public string CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }
    
    public Session Session { get; set; }
    public Member Member { get; set; }
    public FamilyMember FamilyMember { get; set; }
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    NoShow,
    Attended
}
```

#### RecurringBooking
```csharp
public class RecurringBooking
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }
    public Guid RecurringScheduleId { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public RecurringSchedule RecurringSchedule { get; set; }
    public Member Member { get; set; }
    public FamilyMember FamilyMember { get; set; }
}
```

#### Waitlist
```csharp
public class Waitlist
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid SessionId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }
    
    public int Position { get; set; }
    public DateTime JoinedAt { get; set; }
    public bool NotificationSent { get; set; }
    public DateTime? NotificationSentAt { get; set; }
    
    public Session Session { get; set; }
    public Member Member { get; set; }
}
```

#### Event
```csharp
public class Event
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }
    
    public string Title { get; set; }                   // "Annual Awards Night"
    public string Description { get; set; }
    public EventType Type { get; set; }                 // Social, Tournament, AGM, etc.
    
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Location { get; set; }                // If different from venue
    
    public int? Capacity { get; set; }
    public int CurrentAttendees { get; set; }
    
    public bool IsTicketed { get; set; }
    public decimal? TicketPrice { get; set; }
    public decimal? MemberTicketPrice { get; set; }     // Discounted price for members
    public DateTime? TicketSalesEndDate { get; set; }
    
    public bool RequiresRSVP { get; set; }
    public DateTime? RSVPDeadline { get; set; }
    
    public bool IsCancelled { get; set; }
    public string CancellationReason { get; set; }
    public bool IsPublished { get; set; } = true;
    
    public string ImageUrl { get; set; }
    
    public Club Club { get; set; }
    public Venue Venue { get; set; }
    public ICollection<EventTicket> Tickets { get; set; }
    public ICollection<EventRSVP> RSVPs { get; set; }
}

public enum EventType
{
    Social, Tournament, AGM, Training, Fundraiser, 
    Competition, Meeting, Presentation, Other
}
```

#### EventTicket
```csharp
public class EventTicket
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid EventId { get; set; }
    public Guid MemberId { get; set; }
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    
    public string TicketCode { get; set; }              // Unique code for entry
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    
    public DateTime PurchasedAt { get; set; }
    public Guid? PaymentId { get; set; }
    
    public Event Event { get; set; }
    public Member Member { get; set; }
    public Payment Payment { get; set; }
}
```

#### EventRSVP
```csharp
public class EventRSVP
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid EventId { get; set; }
    public Guid MemberId { get; set; }
    
    public RSVPResponse Response { get; set; }
    public int GuestCount { get; set; }
    public string Notes { get; set; }
    public DateTime RespondedAt { get; set; }
    
    public Event Event { get; set; }
    public Member Member { get; set; }
}

public enum RSVPResponse
{
    Attending, NotAttending, Maybe
}
```

#### Venue
```csharp
public class Venue
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    
    public string Name { get; set; }                    // "Main Ground", "Indoor Nets"
    public string Description { get; set; }
    public string Address { get; set; }
    public string PostCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
    public int? Capacity { get; set; }
    public string Facilities { get; set; }              // JSON array or comma-separated
    public string ImageUrl { get; set; }
    
    public bool IsActive { get; set; } = true;
    public bool IsPrimary { get; set; }
    
    public Club Club { get; set; }
    public ICollection<Session> Sessions { get; set; }
    public ICollection<Event> Events { get; set; }
}
```

#### MemberDocument
```csharp
public class MemberDocument
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    
    public string FileName { get; set; }
    public string OriginalFileName { get; set; }
    public string ContentType { get; set; }
    public long FileSize { get; set; }
    public string FilePath { get; set; }
    
    public DocumentType Type { get; set; }
    public string Description { get; set; }
    
    public DateTime UploadedAt { get; set; }
    public string UploadedBy { get; set; }
    
    public Member Member { get; set; }
}

public enum DocumentType
{
    ProfilePhoto, MedicalForm, ConsentForm, 
    DBSCertificate, CoachingQualification, Other
}
```

#### Communication/Notification Entities
```csharp
public class EmailLog
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? MemberId { get; set; }
    
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public EmailType Type { get; set; }
    
    public EmailStatus Status { get; set; }
    public DateTime SentAt { get; set; }
    public string SendGridMessageId { get; set; }
    public string ErrorMessage { get; set; }
    
    public Member Member { get; set; }
}

public enum EmailType
{
    Welcome, PasswordReset, PaymentReminder, PaymentReceipt,
    BookingConfirmation, BookingCancellation, EventReminder,
    MembershipRenewal, BulkCommunication, WaitlistNotification
}

public enum EmailStatus
{
    Pending, Sent, Failed, Bounced
}

public class BulkEmailCampaign
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    
    public string Subject { get; set; }
    public string Body { get; set; }
    public string RecipientFilter { get; set; }         // JSON filter criteria
    public int TotalRecipients { get; set; }
    public int SentCount { get; set; }
    public int FailedCount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public string CreatedBy { get; set; }
    
    public CampaignStatus Status { get; set; }
}

public enum CampaignStatus
{
    Draft, Scheduled, Sending, Completed, Failed
}
```

#### Analytics/Reporting Entity
```csharp
public class ClubAnalyticsSnapshot
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public DateTime SnapshotDate { get; set; }
    
    // Membership Stats
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int NewMembersThisMonth { get; set; }
    public int ExpiredMemberships { get; set; }
    
    // Financial Stats
    public decimal TotalRevenueThisMonth { get; set; }
    public decimal TotalRevenueThisYear { get; set; }
    public decimal OutstandingPayments { get; set; }
    
    // Engagement Stats
    public int SessionsThisMonth { get; set; }
    public int TotalBookingsThisMonth { get; set; }
    public decimal AverageAttendanceRate { get; set; }
    public int EventsThisMonth { get; set; }
}
```

---

## API Endpoints

### Authentication & Identity
```
POST   /api/auth/register              Register new member
POST   /api/auth/login                 Login and get JWT token
POST   /api/auth/refresh               Refresh JWT token
POST   /api/auth/forgot-password       Send password reset email
POST   /api/auth/reset-password        Reset password with token
POST   /api/auth/verify-email          Verify email address
GET    /api/auth/me                    Get current user profile
PUT    /api/auth/change-password       Change password
```

### Super Admin (System-wide)
```
GET    /api/admin/clubs                List all clubs
POST   /api/admin/clubs                Create new club
GET    /api/admin/clubs/{id}           Get club details
PUT    /api/admin/clubs/{id}           Update club
DELETE /api/admin/clubs/{id}           Deactivate club
GET    /api/admin/clubs/{id}/stats     Get club statistics
GET    /api/admin/dashboard            System-wide dashboard
GET    /api/admin/users                List all users (across clubs)
```

### Club Management (Club Manager)
```
GET    /api/club/profile               Get club profile
PUT    /api/club/profile               Update club profile
PUT    /api/club/settings              Update club settings
PUT    /api/club/branding              Update logo/colors
GET    /api/club/dashboard             Club dashboard data
GET    /api/club/analytics             Analytics and reports
GET    /api/club/export/members        Export members to Excel
GET    /api/club/export/payments       Export payments to Excel
```

### Members
```
GET    /api/members                    List members (with filters)
POST   /api/members                    Create member (admin)
GET    /api/members/{id}               Get member details
PUT    /api/members/{id}               Update member
DELETE /api/members/{id}               Deactivate member
GET    /api/members/{id}/memberships   Get member's memberships
GET    /api/members/{id}/payments      Get member's payment history
GET    /api/members/{id}/bookings      Get member's bookings
POST   /api/members/{id}/note          Add note to member
POST   /api/members/import             Import members from CSV
POST   /api/members/bulk-email         Send bulk email to members
```

### Family Members
```
GET    /api/members/{id}/family        List family members
POST   /api/members/{id}/family        Add family member
PUT    /api/members/{id}/family/{fid}  Update family member
DELETE /api/members/{id}/family/{fid}  Remove family member
```

### Membership Types
```
GET    /api/membership-types           List membership types
POST   /api/membership-types           Create membership type
PUT    /api/membership-types/{id}      Update membership type
DELETE /api/membership-types/{id}      Delete membership type
```

### Memberships
```
GET    /api/memberships                List all memberships
POST   /api/memberships                Create membership
GET    /api/memberships/{id}           Get membership details
PUT    /api/memberships/{id}           Update membership
POST   /api/memberships/{id}/renew     Renew membership
POST   /api/memberships/{id}/cancel    Cancel membership
```

### Payments
```
GET    /api/payments                   List payments (with filters)
GET    /api/payments/{id}              Get payment details
POST   /api/payments/manual            Record manual payment
POST   /api/payments/{id}/refund       Process refund
GET    /api/payments/summary           Payment summary/stats

# Stripe
POST   /api/payments/stripe/create-intent    Create payment intent
POST   /api/payments/stripe/webhook          Stripe webhook handler

# PayPal
POST   /api/payments/paypal/create-order     Create PayPal order
POST   /api/payments/paypal/capture          Capture PayPal payment
POST   /api/payments/paypal/webhook          PayPal webhook handler
```

### Sessions (Training/Booking)
```
GET    /api/sessions                   List sessions (with date range)
POST   /api/sessions                   Create session
GET    /api/sessions/{id}              Get session details
PUT    /api/sessions/{id}              Update session
DELETE /api/sessions/{id}              Cancel session
GET    /api/sessions/{id}/bookings     Get session bookings
GET    /api/sessions/{id}/attendance   Get attendance list
POST   /api/sessions/{id}/attendance   Mark attendance
```

### Session Bookings
```
POST   /api/sessions/{id}/book         Book a session
DELETE /api/sessions/{id}/book         Cancel booking
POST   /api/sessions/{id}/waitlist     Join waitlist
DELETE /api/sessions/{id}/waitlist     Leave waitlist
```

### Recurring Schedules
```
GET    /api/recurring-schedules        List recurring schedules
POST   /api/recurring-schedules        Create recurring schedule
PUT    /api/recurring-schedules/{id}   Update recurring schedule
DELETE /api/recurring-schedules/{id}   Delete recurring schedule
POST   /api/recurring-schedules/{id}/generate   Generate sessions
```

### Recurring Bookings
```
GET    /api/recurring-bookings         List member's recurring bookings
POST   /api/recurring-bookings         Create recurring booking
PUT    /api/recurring-bookings/{id}    Update recurring booking
DELETE /api/recurring-bookings/{id}    Cancel recurring booking
```

### Events
```
GET    /api/events                     List events
POST   /api/events                     Create event
GET    /api/events/{id}                Get event details
PUT    /api/events/{id}                Update event
DELETE /api/events/{id}                Cancel event
GET    /api/events/{id}/attendees      Get attendee list
POST   /api/events/{id}/rsvp           RSVP to event
POST   /api/events/{id}/purchase       Purchase tickets
```

### Venues
```
GET    /api/venues                     List venues
POST   /api/venues                     Create venue
GET    /api/venues/{id}                Get venue details
PUT    /api/venues/{id}                Update venue
DELETE /api/venues/{id}                Delete venue
GET    /api/venues/{id}/schedule       Get venue schedule
```

### Documents/Files
```
POST   /api/documents/upload           Upload document
GET    /api/documents/{id}             Download document
DELETE /api/documents/{id}             Delete document
POST   /api/documents/profile-photo    Upload profile photo
```

### Communications
```
POST   /api/communications/email       Send email to member(s)
GET    /api/communications/history     Get email history
GET    /api/communications/templates   Get email templates
POST   /api/communications/templates   Create email template
```

### Reports & Analytics
```
GET    /api/reports/membership-stats   Membership statistics
GET    /api/reports/financial-summary  Financial summary
GET    /api/reports/attendance         Attendance reports
GET    /api/reports/retention          Member retention analysis
GET    /api/reports/growth             Growth trends
GET    /api/reports/export/{type}      Export report to Excel/PDF
```

### Member Self-Service Portal
```
GET    /api/portal/dashboard           Member dashboard
GET    /api/portal/profile             Get own profile
PUT    /api/portal/profile             Update own profile
GET    /api/portal/membership          Get current membership
GET    /api/portal/payments            Get payment history
GET    /api/portal/receipts/{id}       Download receipt
GET    /api/portal/bookings            Get my bookings
GET    /api/portal/upcoming-sessions   Get available sessions
GET    /api/portal/events              Get upcoming events
GET    /api/portal/family              Get family members
```

---

## Angular Frontend Structure

```
src/
├── app/
│   ├── core/                           # Singleton services, guards, interceptors
│   │   ├── services/
│   │   │   ├── auth.service.ts
│   │   │   ├── api.service.ts
│   │   │   ├── club.service.ts
│   │   │   ├── member.service.ts
│   │   │   ├── payment.service.ts
│   │   │   ├── session.service.ts
│   │   │   ├── event.service.ts
│   │   │   ├── notification.service.ts
│   │   │   └── file-upload.service.ts
│   │   ├── guards/
│   │   │   ├── auth.guard.ts
│   │   │   ├── super-admin.guard.ts
│   │   │   └── club-manager.guard.ts
│   │   ├── interceptors/
│   │   │   ├── auth.interceptor.ts
│   │   │   ├── tenant.interceptor.ts
│   │   │   └── error.interceptor.ts
│   │   └── models/
│   │       ├── club.model.ts
│   │       ├── member.model.ts
│   │       ├── membership.model.ts
│   │       ├── payment.model.ts
│   │       ├── session.model.ts
│   │       ├── event.model.ts
│   │       └── index.ts
│   │
│   ├── shared/                         # Shared components, directives, pipes
│   │   ├── components/
│   │   │   ├── header/
│   │   │   ├── sidebar/
│   │   │   ├── footer/
│   │   │   ├── loading-spinner/
│   │   │   ├── confirmation-dialog/
│   │   │   ├── data-table/
│   │   │   ├── pagination/
│   │   │   ├── file-upload/
│   │   │   ├── chart-card/
│   │   │   ├── stat-card/
│   │   │   ├── empty-state/
│   │   │   └── error-message/
│   │   ├── directives/
│   │   │   └── has-role.directive.ts
│   │   └── pipes/
│   │       ├── currency-gbp.pipe.ts
│   │       ├── date-format.pipe.ts
│   │       └── status-badge.pipe.ts
│   │
│   ├── features/                       # Feature modules (lazy-loaded)
│   │   │
│   │   ├── auth/                       # Authentication
│   │   │   ├── login/
│   │   │   ├── register/
│   │   │   ├── forgot-password/
│   │   │   ├── reset-password/
│   │   │   └── verify-email/
│   │   │
│   │   ├── super-admin/                # System Admin Area
│   │   │   ├── dashboard/
│   │   │   ├── clubs/
│   │   │   │   ├── club-list/
│   │   │   │   ├── club-detail/
│   │   │   │   └── club-form/
│   │   │   └── users/
│   │   │
│   │   ├── club-admin/                 # Club Manager Area
│   │   │   ├── dashboard/
│   │   │   │   └── dashboard.component.ts
│   │   │   ├── members/
│   │   │   │   ├── member-list/
│   │   │   │   ├── member-detail/
│   │   │   │   ├── member-form/
│   │   │   │   └── member-import/
│   │   │   ├── memberships/
│   │   │   │   ├── membership-types/
│   │   │   │   └── membership-list/
│   │   │   ├── payments/
│   │   │   │   ├── payment-list/
│   │   │   │   ├── payment-detail/
│   │   │   │   ├── manual-payment/
│   │   │   │   └── payment-dashboard/
│   │   │   ├── sessions/
│   │   │   │   ├── session-list/
│   │   │   │   ├── session-calendar/
│   │   │   │   ├── session-form/
│   │   │   │   ├── session-attendance/
│   │   │   │   └── recurring-schedules/
│   │   │   ├── events/
│   │   │   │   ├── event-list/
│   │   │   │   ├── event-detail/
│   │   │   │   ├── event-form/
│   │   │   │   └── event-attendees/
│   │   │   ├── venues/
│   │   │   │   ├── venue-list/
│   │   │   │   └── venue-form/
│   │   │   ├── communications/
│   │   │   │   ├── send-email/
│   │   │   │   ├── email-history/
│   │   │   │   └── email-templates/
│   │   │   ├── reports/
│   │   │   │   ├── membership-report/
│   │   │   │   ├── financial-report/
│   │   │   │   ├── attendance-report/
│   │   │   │   └── export-center/
│   │   │   └── settings/
│   │   │       ├── club-profile/
│   │   │       ├── club-settings/
│   │   │       ├── payment-settings/
│   │   │       └── branding/
│   │   │
│   │   └── member-portal/              # Member Self-Service
│   │       ├── dashboard/
│   │       ├── profile/
│   │       │   ├── view-profile/
│   │       │   ├── edit-profile/
│   │       │   └── family-members/
│   │       ├── membership/
│   │       │   ├── current-membership/
│   │       │   ├── renew-membership/
│   │       │   └── payment-history/
│   │       ├── bookings/
│   │       │   ├── available-sessions/
│   │       │   ├── my-bookings/
│   │       │   └── recurring-bookings/
│   │       ├── events/
│   │       │   ├── upcoming-events/
│   │       │   ├── my-tickets/
│   │       │   └── event-detail/
│   │       └── payments/
│   │           ├── make-payment/
│   │           └── receipts/
│   │
│   ├── layouts/
│   │   ├── admin-layout/               # Layout for admin areas
│   │   ├── portal-layout/              # Layout for member portal
│   │   └── auth-layout/                # Layout for auth pages
│   │
│   ├── app.component.ts
│   ├── app.config.ts
│   └── app.routes.ts
│
├── assets/
│   ├── images/
│   └── icons/
│
├── environments/
│   ├── environment.ts
│   └── environment.prod.ts
│
└── styles/
    ├── styles.scss                     # Global styles + Tailwind imports
    └── _variables.scss
```

---

## Key Features Implementation Details

### 1. Multi-Tenancy Implementation

**Tenant Resolution Middleware:**
```csharp
public class TenantMiddleware
{
    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Resolve tenant from:
        // 1. Subdomain (cricketclub.theleague.com)
        // 2. X-Tenant-Id header
        // 3. Claim in JWT token
        
        var tenantId = ResolveTenantId(context);
        tenantService.SetCurrentTenant(tenantId);
        await _next(context);
    }
}
```

**Global Query Filter:**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Apply to all entities with ClubId
    modelBuilder.Entity<Member>().HasQueryFilter(m => m.ClubId == _tenantService.CurrentTenantId);
    modelBuilder.Entity<Session>().HasQueryFilter(s => s.ClubId == _tenantService.CurrentTenantId);
    // ... etc
}
```

### 2. Payment Processing

**Stripe Integration:**
```csharp
public interface IStripePaymentService
{
    Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency, string customerId);
    Task<PaymentIntent> ConfirmPayment(string paymentIntentId);
    Task<Refund> CreateRefund(string paymentIntentId, decimal? amount);
    Task<Customer> CreateCustomer(string email, string name);
    Task HandleWebhook(string json, string signature);
}
```

**PayPal Integration:**
```csharp
public interface IPayPalPaymentService
{
    Task<Order> CreateOrder(decimal amount, string currency, string returnUrl, string cancelUrl);
    Task<Order> CaptureOrder(string orderId);
    Task<Refund> RefundPayment(string captureId, decimal? amount);
    Task HandleWebhook(string json, IHeaderDictionary headers);
}
```

### 3. Email Notifications (SendGrid)

**Email Service:**
```csharp
public interface IEmailService
{
    Task SendWelcomeEmail(Member member);
    Task SendPaymentReminder(Member member, Membership membership);
    Task SendPaymentReceipt(Member member, Payment payment);
    Task SendBookingConfirmation(Member member, SessionBooking booking);
    Task SendBulkEmail(List<string> emails, string subject, string body);
    Task SendPasswordResetEmail(string email, string resetToken);
}
```

### 4. Booking System

**Session Booking Logic:**
```csharp
public async Task<BookingResult> BookSession(Guid sessionId, Guid memberId, Guid? familyMemberId)
{
    var session = await GetSession(sessionId);
    
    // Check capacity
    if (session.CurrentBookings >= session.Capacity)
    {
        // Add to waitlist if enabled
        if (_clubSettings.EnableWaitlist)
            return await AddToWaitlist(sessionId, memberId, familyMemberId);
        
        return BookingResult.SessionFull;
    }
    
    // Check member hasn't already booked
    if (await HasExistingBooking(sessionId, memberId, familyMemberId))
        return BookingResult.AlreadyBooked;
    
    // Create booking
    var booking = new SessionBooking { ... };
    await _context.SessionBookings.AddAsync(booking);
    session.CurrentBookings++;
    
    // Send confirmation email
    await _emailService.SendBookingConfirmation(member, booking);
    
    return BookingResult.Success;
}
```

### 5. Recurring Bookings

**Generate Sessions from Schedule:**
```csharp
public async Task GenerateSessions(Guid scheduleId, DateTime fromDate, DateTime toDate)
{
    var schedule = await GetRecurringSchedule(scheduleId);
    var currentDate = fromDate;
    
    while (currentDate <= toDate)
    {
        if (currentDate.DayOfWeek == schedule.DayOfWeek)
        {
            var session = new Session
            {
                RecurringScheduleId = scheduleId,
                StartTime = currentDate.Date + schedule.StartTime,
                EndTime = currentDate.Date + schedule.EndTime,
                // ... other properties
            };
            
            await _context.Sessions.AddAsync(session);
            
            // Auto-book recurring booking holders
            await ProcessRecurringBookings(session);
        }
        currentDate = currentDate.AddDays(1);
    }
}
```

### 6. Family Account Management

**Family Registration Flow:**
```csharp
public async Task<Member> RegisterFamilyAccount(FamilyRegistrationDto dto)
{
    // Create primary member
    var primaryMember = new Member
    {
        IsFamilyAccount = true,
        // ... other properties
    };
    
    // Create family members
    foreach (var fm in dto.FamilyMembers)
    {
        var familyMember = new FamilyMember
        {
            PrimaryMemberId = primaryMember.Id,
            FirstName = fm.FirstName,
            DateOfBirth = fm.DateOfBirth,
            Relation = fm.Relation
        };
        primaryMember.FamilyMembers.Add(familyMember);
    }
    
    await _context.Members.AddAsync(primaryMember);
    return primaryMember;
}
```

### 7. Reports & Analytics

**Dashboard Data Service:**
```csharp
public class DashboardService
{
    public async Task<ClubDashboardDto> GetClubDashboard(Guid clubId)
    {
        return new ClubDashboardDto
        {
            TotalMembers = await GetMemberCount(),
            ActiveMembers = await GetActiveMemberCount(),
            NewMembersThisMonth = await GetNewMembersThisMonth(),
            
            TotalRevenueThisMonth = await GetRevenueThisMonth(),
            OutstandingPayments = await GetOutstandingPayments(),
            PaymentsDueThisWeek = await GetPaymentsDueThisWeek(),
            
            UpcomingSessions = await GetUpcomingSessions(7),
            SessionAttendanceRate = await CalculateAttendanceRate(),
            
            MembershipBreakdown = await GetMembershipBreakdown(),
            RevenueByMonth = await GetRevenueByMonth(12),
            AttendanceTrend = await GetAttendanceTrend(12)
        };
    }
}
```

---

## Seed Data Specification

### Sample Clubs

**Club 1: Willow Creek Cricket Club (Large)**
- Members: ~350
- Type: Cricket
- Membership Types: Senior (£180), Junior (£90), Family (£350), Student (£120), Social (£50)
- Multiple teams: 1st XI, 2nd XI, 3rd XI, U19, U15, U13, U11, Women's
- 2 venues: Main Ground, Indoor Training Centre
- Weekly training sessions for each age group
- Annual events: Awards Night, Season Opening, Club Tour

**Club 2: Riverside FC (Medium)**
- Members: ~180
- Type: Football
- Membership Types: Senior (£150), Junior (£75), Family (£280), Walking Football (£60)
- Teams: First Team, Reserves, U18, U16, U14, U12, Veterans, Ladies
- 1 venue: Riverside Park
- Training twice weekly per team
- Events: End of Season Dinner, Tournament

**Club 3: Oakwood Tennis Club (Small)**
- Members: ~85
- Type: Tennis
- Membership Types: Adult (£200), Junior (£100), Family (£400), Off-Peak (£120)
- 4 courts available for booking
- Weekly coaching sessions
- Events: Club Championship, Social Tennis Nights

### Seed Data Includes:
- 3 clubs with full configuration
- 50-100 members per club with realistic data
- 5-10 membership types per club
- 20-30 sessions per club (past and upcoming)
- 5-10 events per club
- Payment history (mix of paid, pending, overdue)
- Booking history with attendance records
- Family accounts with linked members
- Multiple venues for larger clubs

---

## Security Requirements

### Authentication
- JWT tokens with 15-minute expiry
- Refresh tokens with 7-day expiry
- Password hashing with BCrypt
- Email verification required
- Account lockout after 5 failed attempts

### Authorization
- Role-based access control (SuperAdmin, ClubManager, Member)
- Resource-based authorization for club data
- API rate limiting (100 requests/minute per user)

### Data Protection
- All sensitive data encrypted at rest
- HTTPS enforced
- PCI compliance for payment data (handled by Stripe/PayPal)
- GDPR compliance: data export, deletion requests
- Audit logging for sensitive operations

---

## File Upload Requirements

### Supported File Types
- Profile Photos: JPG, PNG, GIF (max 5MB)
- Documents: PDF, DOC, DOCX (max 10MB)
- Club Logo: PNG, SVG (max 2MB)

### Storage Structure
```
/uploads
  /{clubId}
    /members
      /{memberId}
        /profile-photo.jpg
        /documents
          /medical-form.pdf
          /consent-form.pdf
    /club
      /logo.png
      /events
        /{eventId}/banner.jpg
```

---

## Mobile-First Design Guidelines

### Breakpoints (Tailwind)
- Mobile: < 640px (default)
- Tablet: sm (640px+)
- Desktop: lg (1024px+)
- Large Desktop: xl (1280px+)

### Key Mobile Considerations
- Touch-friendly buttons (min 44px height)
- Bottom navigation for member portal
- Collapsible sidebar for admin
- Card-based layouts
- Swipe gestures for lists
- Pull-to-refresh
- Offline indicators

---

## Testing Requirements

### Backend
- Unit tests for all services
- Integration tests for API endpoints
- Test coverage > 80%

### Frontend
- Unit tests for components
- E2E tests for critical flows (registration, payment, booking)
- Accessibility testing (WCAG 2.1 AA)

---

## Deployment Notes

### Environment Variables
```
# Database
ConnectionStrings__DefaultConnection=

# JWT
Jwt__Secret=
Jwt__Issuer=
Jwt__Audience=

# Stripe
Stripe__SecretKey=
Stripe__PublishableKey=
Stripe__WebhookSecret=

# PayPal
PayPal__ClientId=
PayPal__ClientSecret=
PayPal__Mode=sandbox|live

# SendGrid
SendGrid__ApiKey=

# File Storage
FileStorage__BasePath=
```

---

## Implementation Priority

### Phase 1: Foundation (Week 1-2)
1. Project setup (API + Angular)
2. Database schema with EF Core
3. Authentication & authorization
4. Multi-tenancy middleware
5. Seed data

### Phase 2: Core Features (Week 3-4)
1. Member management
2. Membership types & memberships
3. Member self-service portal
4. Club admin dashboard

### Phase 3: Payments (Week 5-6)
1. Stripe integration
2. PayPal integration
3. Payment dashboard
4. Automatic reminders

### Phase 4: Booking & Events (Week 7-8)
1. Session management
2. Booking system with waitlist
3. Recurring schedules & bookings
4. Event management

### Phase 5: Communications & Reports (Week 9-10)
1. SendGrid integration
2. Bulk email campaigns
3. Reports & analytics
4. Data export (Excel)

### Phase 6: Polish & Testing (Week 11-12)
1. File uploads
2. Mobile optimization
3. Testing
4. Documentation

---

## Summary

This document provides a complete specification for implementing The League membership management system. The application supports:

- **Multi-tenant architecture** with shared database
- **Three user roles**: Super Admin, Club Manager, Member
- **Complete member management** with family accounts
- **Flexible membership types** with annual, monthly, and pay-as-you-go options
- **Dual payment processing** via Stripe and PayPal
- **Session booking** with recurring schedules and waitlists
- **Event management** with ticketing and RSVP
- **Email communications** via SendGrid
- **Interactive dashboards** with charts and exportable reports
- **Mobile-first responsive design**

The seed data includes 3 realistic sports clubs with hundreds of members, sessions, events, and payment history to demonstrate all features immediately upon deployment.
