using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents an event organized by the club.
/// Enhanced with comprehensive event management features.
/// </summary>
public class Event
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? VenueId { get; set; }
    public Guid? FacilityId { get; set; }
    public Guid? SeriesId { get; set; }

    // Basic Info
    public string Title { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public EventType Type { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;

    // Classification
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? Sport { get; set; }
    public SkillLevel SkillLevel { get; set; } = SkillLevel.AllLevels;
    public AgeGroup AgeGroup { get; set; } = AgeGroup.AllAges;
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public Gender? TargetGender { get; set; }

    // Schedule
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public bool IsAllDay { get; set; }
    public string? TimeZone { get; set; }
    public string? Location { get; set; }
    public string? Address { get; set; }
    public string? Room { get; set; }
    public string? MeetingPoint { get; set; }

    // Recurrence
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; } // JSON: RRULE format
    public Guid? ParentEventId { get; set; }
    public int? OccurrenceNumber { get; set; }

    // Capacity & Registration
    public int? Capacity { get; set; }
    public int CurrentAttendees { get; set; }
    public int WaitlistCount { get; set; }
    public bool AllowWaitlist { get; set; } = true;
    public int? WaitlistLimit { get; set; }
    public int? MinParticipants { get; set; }
    public bool RequiresRegistration { get; set; }
    public DateTime? RegistrationOpenDate { get; set; }
    public DateTime? RegistrationCloseDate { get; set; }
    public bool RequiresApproval { get; set; }

    // RSVP Settings
    public bool RequiresRSVP { get; set; }
    public DateTime? RSVPDeadline { get; set; }
    public bool AllowGuests { get; set; }
    public int? MaxGuestsPerRSVP { get; set; }

    // Ticketing
    public bool IsTicketed { get; set; }
    public decimal? TicketPrice { get; set; }
    public decimal? MemberTicketPrice { get; set; }
    public decimal? EarlyBirdPrice { get; set; }
    public DateTime? EarlyBirdDeadline { get; set; }
    public DateTime? TicketSalesStartDate { get; set; }
    public DateTime? TicketSalesEndDate { get; set; }
    public int? MaxTicketsPerPerson { get; set; }
    public string Currency { get; set; } = "GBP";
    public Guid? FeeId { get; set; }

    // Discounts
    public decimal? GroupDiscount { get; set; }
    public int? GroupDiscountMinSize { get; set; }
    public string? DiscountCodes { get; set; } // JSON array

    // Membership Restrictions
    public bool MembersOnly { get; set; }
    public string? AllowedMembershipTypes { get; set; } // JSON array

    // Check-in
    public bool AllowCheckIn { get; set; }
    public DateTime? CheckInOpensAt { get; set; }
    public DateTime? CheckInClosesAt { get; set; }
    public string? CheckInCode { get; set; }
    public string? CheckInQRCode { get; set; }

    // Organizer/Contact
    public Guid? OrganizerId { get; set; }
    public string? OrganizerName { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }

    // Sponsorship
    public bool HasSponsors { get; set; }
    public string? Sponsors { get; set; } // JSON array

    // Media
    public string? ImageUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? GalleryUrls { get; set; } // JSON array
    public string? VideoUrl { get; set; }
    public string? DocumentUrls { get; set; } // JSON array

    // External Links
    public string? ExternalUrl { get; set; }
    public string? LiveStreamUrl { get; set; }

    // Terms & Policies
    public string? TermsAndConditions { get; set; }
    public string? CancellationPolicy { get; set; }
    public decimal? CancellationFee { get; set; }
    public int? CancellationNoticeDays { get; set; }

    // Cancellation/Postponement
    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancelledBy { get; set; }
    public bool IsPostponed { get; set; }
    public DateTime? OriginalStartDateTime { get; set; }
    public string? PostponementReason { get; set; }

    // Publishing
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? PublishedBy { get; set; }
    public bool ShowOnWebsite { get; set; } = true;
    public bool AllowOnlineRegistration { get; set; } = true;
    public bool IsFeatured { get; set; }

    // Statistics
    public int TotalRegistrations { get; set; }
    public int TotalAttendees { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal? AverageRating { get; set; }
    public int ReviewCount { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? SpecialInstructions { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Venue? Venue { get; set; }
    public Facility? Facility { get; set; }
    public EventSeries? Series { get; set; }
    public Event? ParentEvent { get; set; }
    public Member? Organizer { get; set; }
    public Fee? Fee { get; set; }
    public ICollection<Event> ChildEvents { get; set; } = new List<Event>();
    public ICollection<EventTicket> Tickets { get; set; } = new List<EventTicket>();
    public ICollection<EventRSVP> RSVPs { get; set; } = new List<EventRSVP>();
    public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
    public ICollection<EventSession> Sessions { get; set; } = new List<EventSession>();
}

/// <summary>
/// Represents a series of related events (e.g., monthly meetups, annual gala).
/// </summary>
public class EventSeries
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ICollection<Event> Events { get; set; } = new List<Event>();
}

/// <summary>
/// Represents a session within a multi-session event.
/// </summary>
public class EventSession
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid? VenueId { get; set; }
    public Guid? FacilityId { get; set; }

    public int SessionNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Speaker { get; set; }

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Room { get; set; }

    public int? Capacity { get; set; }
    public int AttendeeCount { get; set; }
    public bool RequiresSeparateRegistration { get; set; }

    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }

    public string? Notes { get; set; }

    // Navigation
    public Event Event { get; set; } = null!;
    public Venue? Venue { get; set; }
    public Facility? Facility { get; set; }
}

/// <summary>
/// Enhanced event ticket with detailed tracking.
/// </summary>
public class EventTicket
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid ClubId { get; set; }
    public Guid? MemberId { get; set; }

    // Ticket Details
    public string TicketNumber { get; set; } = string.Empty;
    public string? TicketType { get; set; }
    public TicketStatus Status { get; set; } = TicketStatus.Available;
    public string? Barcode { get; set; }
    public string? QRCode { get; set; }

    // Attendee Info
    public string? AttendeeName { get; set; }
    public string? AttendeeEmail { get; set; }
    public string? AttendeePhone { get; set; }
    public bool IsGuest { get; set; }

    // Pricing
    public decimal Price { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? DiscountCode { get; set; }
    public decimal FinalPrice { get; set; }
    public string Currency { get; set; } = "GBP";

    // Payment
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaymentReference { get; set; }
    public Guid? PaymentId { get; set; }

    // Check-in
    public bool IsCheckedIn { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public string? CheckedInBy { get; set; }
    public DateTime? CheckedOutAt { get; set; }

    // Cancellation/Refund
    public bool IsCancelled { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public bool RefundRequested { get; set; }
    public bool RefundProcessed { get; set; }
    public decimal? RefundAmount { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Event Event { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member? Member { get; set; }
    public Payment? Payment { get; set; }
}

/// <summary>
/// Enhanced RSVP with detailed response tracking.
/// </summary>
public class EventRSVP
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid ClubId { get; set; }
    public Guid? MemberId { get; set; }

    // RSVP Details
    public RSVPResponse Response { get; set; }
    public int GuestCount { get; set; }
    public string? GuestNames { get; set; } // JSON array

    // Contact Info (for non-members)
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // Dietary/Special Requirements
    public string? DietaryRequirements { get; set; }
    public string? SpecialRequirements { get; set; }

    // Check-in
    public bool IsCheckedIn { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public string? CheckedInBy { get; set; }
    public int? GuestsCheckedIn { get; set; }

    // Response History
    public DateTime RespondedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? PreviousResponse { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? Message { get; set; }

    // Navigation
    public Event Event { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member? Member { get; set; }
}

/// <summary>
/// Represents a member's registration for an event.
/// </summary>
public class EventRegistration
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid? FamilyMemberId { get; set; }

    // Registration Details
    public string RegistrationNumber { get; set; } = string.Empty;
    public EventRegistrationStatus Status { get; set; } = EventRegistrationStatus.Pending;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedDate { get; set; }
    public int? WaitlistPosition { get; set; }

    // Pricing
    public decimal Price { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? DiscountCode { get; set; }
    public string? DiscountReason { get; set; }
    public decimal FinalPrice { get; set; }
    public string Currency { get; set; } = "GBP";

    // Payment
    public bool IsPaid { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal BalanceDue { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaymentReference { get; set; }
    public Guid? PaymentId { get; set; }
    public Guid? InvoiceId { get; set; }

    // Check-in
    public bool IsCheckedIn { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public string? CheckedInBy { get; set; }
    public DateTime? CheckedOutAt { get; set; }

    // Cancellation
    public DateTime? CancelledDate { get; set; }
    public string? CancelledBy { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? RefundAmount { get; set; }
    public bool RefundProcessed { get; set; }

    // Emergency Contact
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }

    // Special Requirements
    public string? DietaryRequirements { get; set; }
    public string? MedicalNotes { get; set; }
    public string? SpecialRequirements { get; set; }
    public bool ConsentFormSubmitted { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Event Event { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public FamilyMember? FamilyMember { get; set; }
    public Payment? Payment { get; set; }
    public Invoice? Invoice { get; set; }
}
