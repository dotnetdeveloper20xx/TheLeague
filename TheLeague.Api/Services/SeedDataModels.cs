namespace TheLeague.Api.Services;

/// <summary>
/// Models for deserializing seed data from JSON file
/// </summary>
public class SeedDataRoot
{
    public SystemConfigurationSeed SystemConfiguration { get; set; } = new();
    public List<ClubSeed> Clubs { get; set; } = new();
    public UsersSeed Users { get; set; } = new();
    public List<MembershipTypeSeed> MembershipTypes { get; set; } = new();
    public List<VenueSeed> Venues { get; set; } = new();
    public List<MemberSeed> Members { get; set; } = new();
    public List<FeeSeed> Fees { get; set; } = new();
    public List<RecurringScheduleSeed> RecurringSchedules { get; set; } = new();
    public List<SessionSeed> Sessions { get; set; } = new();
    public List<EventSeed> Events { get; set; } = new();
    public List<CompetitionSeed> Competitions { get; set; } = new();
    public List<PaymentSeed> Payments { get; set; } = new();
    public List<InvoiceSeed> Invoices { get; set; } = new();
}

public class SystemConfigurationSeed
{
    public string PaymentProvider { get; set; } = "Mock";
    public int MockPaymentDelayMs { get; set; } = 1500;
    public double MockPaymentFailureRate { get; set; } = 0.0;
    public string EmailProvider { get; set; } = "Mock";
    public int MockEmailDelayMs { get; set; } = 500;
    public string DefaultFromEmail { get; set; } = "noreply@theleague.com";
    public string DefaultFromName { get; set; } = "The League";
    public bool MaintenanceMode { get; set; } = false;
    public bool AllowNewRegistrations { get; set; } = true;
    public bool EnableEmailNotifications { get; set; } = true;
    public string PlatformName { get; set; } = "The League";
    public string PrimaryColor { get; set; } = "#6366f1";
}

public class ClubSeed
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Type { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string? Website { get; set; }
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string PostCode { get; set; } = "";
    public string PrimaryColor { get; set; } = "#6366f1";
    public string SecondaryColor { get; set; } = "#93c5fd";
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UsersSeed
{
    public SuperAdminSeed SuperAdmin { get; set; } = new();
    public List<ClubManagerSeed> ClubManagers { get; set; } = new();
}

public class SuperAdminSeed
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}

public class ClubManagerSeed
{
    public string ClubId { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}

public class MembershipTypeSeed
{
    public string ClubId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal AnnualFee { get; set; }
    public decimal MonthlyFee { get; set; }
    public decimal SessionFee { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int MaxFamilyMembers { get; set; }
    public bool IsActive { get; set; } = true;
    public bool AllowOnlineSignup { get; set; } = true;
    public bool IncludesBooking { get; set; } = true;
    public bool IncludesEvents { get; set; } = true;
    public int? MaxSessionsPerWeek { get; set; }
    public int SortOrder { get; set; }
}

public class VenueSeed
{
    public string ClubId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Address { get; set; } = "";
    public string PostCode { get; set; } = "";
    public int Capacity { get; set; }
    public string? Facilities { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsPrimary { get; set; }
}

public class MemberSeed
{
    public string ClubId { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string DateOfBirth { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string PostCode { get; set; } = "";
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }
    public string? MedicalConditions { get; set; }
    public string? Allergies { get; set; }
    public bool IsFamilyAccount { get; set; }
    public string Status { get; set; } = "Active";
    public string MembershipType { get; set; } = "";
    public string JoinedDate { get; set; } = "";
    public List<FamilyMemberSeed>? FamilyMembers { get; set; }
}

public class FamilyMemberSeed
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string DateOfBirth { get; set; } = "";
    public string Relation { get; set; } = "";
    public string? MedicalConditions { get; set; }
    public string? Allergies { get; set; }
}

public class FeeSeed
{
    public string ClubId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";
    public string Frequency { get; set; } = "OneTime";
    public string Description { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public bool Taxable { get; set; }
    public decimal TaxRate { get; set; }
}

public class RecurringScheduleSeed
{
    public string ClubId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public string DayOfWeek { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public int VenueIndex { get; set; }
    public int Capacity { get; set; }
    public decimal SessionFee { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SessionSeed
{
    public string ClubId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public int DaysFromNow { get; set; }
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public int VenueIndex { get; set; }
    public int Capacity { get; set; }
    public decimal SessionFee { get; set; }
}

public class EventSeed
{
    public string ClubId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Type { get; set; } = "";
    public int DaysFromNow { get; set; }
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public int Duration { get; set; } = 1;
    public int VenueIndex { get; set; }
    public int Capacity { get; set; }
    public bool IsTicketed { get; set; }
    public decimal TicketPrice { get; set; }
    public decimal MemberTicketPrice { get; set; }
    public bool RequiresRSVP { get; set; }
    public bool IsMembersOnly { get; set; }
    public string SkillLevel { get; set; } = "AllLevels";
}

public class CompetitionSeed
{
    public string ClubId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Sport { get; set; } = "";
    public string Format { get; set; } = "";
    public string StartDate { get; set; } = "";
    public string EndDate { get; set; } = "";
    public decimal EntryFee { get; set; }
    public bool IsPublished { get; set; }
    public List<TeamSeed> Teams { get; set; } = new();
}

public class TeamSeed
{
    public string Name { get; set; } = "";
    public string Captain { get; set; } = "";
    public string Color { get; set; } = "";
}

public class PaymentSeed
{
    public string MemberEmail { get; set; } = "";
    public decimal Amount { get; set; }
    public string Type { get; set; } = "";
    public string Method { get; set; } = "";
    public string Status { get; set; } = "";
    public int DaysAgo { get; set; }
    public string Description { get; set; } = "";
}

public class InvoiceSeed
{
    public string MemberEmail { get; set; } = "";
    public decimal Amount { get; set; }
    public string Status { get; set; } = "";
    public int DaysAgo { get; set; }
    public int DueDate { get; set; }
    public string Description { get; set; } = "";
    public List<InvoiceLineItemSeed> LineItems { get; set; } = new();
}

public class InvoiceLineItemSeed
{
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
}
