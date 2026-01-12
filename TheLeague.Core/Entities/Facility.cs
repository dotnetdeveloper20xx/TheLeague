using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents an individual facility within a venue (court, pool, room, etc.).
/// Tracks capacity, availability, pricing, and maintenance.
/// </summary>
public class Facility
{
    public Guid Id { get; set; }
    public Guid VenueId { get; set; }
    public Guid ClubId { get; set; }

    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public FacilityType Type { get; set; }
    public FacilityStatus Status { get; set; } = FacilityStatus.Available;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }

    // Dimensions & Capacity
    public int? Capacity { get; set; }
    public int? MinimumCapacity { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Area { get; set; }
    public string? DimensionUnit { get; set; } // meters, feet
    public string? SurfaceType { get; set; } // Grass, Clay, Hard Court, etc.

    // Equipment & Features
    public string? Equipment { get; set; } // JSON array of included equipment
    public string? Features { get; set; } // JSON array of features
    public bool HasLighting { get; set; }
    public bool HasHeating { get; set; }
    public bool HasAirConditioning { get; set; }
    public bool IsIndoor { get; set; }
    public bool IsAccessible { get; set; }

    // Images
    public string? ImageUrl { get; set; }
    public string? GalleryUrls { get; set; } // JSON array

    // Booking Settings
    public bool AllowOnlineBooking { get; set; } = true;
    public bool RequiresApproval { get; set; }
    public int? MinBookingDuration { get; set; } // Minutes
    public int? MaxBookingDuration { get; set; } // Minutes
    public int? DefaultBookingDuration { get; set; } // Minutes
    public int? BookingSlotInterval { get; set; } // Minutes between slots
    public int? AdvanceBookingDays { get; set; }
    public int? MaxConcurrentBookings { get; set; }
    public int? BufferTimeBetweenBookings { get; set; } // Minutes

    // Pricing
    public decimal? BasePrice { get; set; }
    public decimal? MemberPrice { get; set; }
    public decimal? NonMemberPrice { get; set; }
    public decimal? PeakPrice { get; set; }
    public decimal? OffPeakPrice { get; set; }
    public PricingType PricingType { get; set; } = PricingType.PerHour;
    public string Currency { get; set; } = "GBP";
    public bool PriceIncludesVat { get; set; }
    public Guid? DefaultTaxRateId { get; set; }

    // Restrictions
    public string? AllowedMembershipTypes { get; set; } // JSON array of membership plan IDs
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public string? RequiredCertifications { get; set; } // JSON array
    public string? BookingRestrictions { get; set; }

    // Maintenance
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public int? MaintenanceIntervalDays { get; set; }
    public string? MaintenanceNotes { get; set; }

    // Usage Tracking
    public int TotalBookingsCount { get; set; }
    public int TotalHoursBooked { get; set; }
    public decimal? TotalRevenue { get; set; }
    public DateTime? LastBookedDate { get; set; }

    // Related
    public Guid? ParentFacilityId { get; set; } // For sub-facilities

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? UsageInstructions { get; set; }
    public string? SafetyGuidelines { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Venue Venue { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Facility? ParentFacility { get; set; }
    public TaxRate? DefaultTaxRate { get; set; }
    public ICollection<Facility> SubFacilities { get; set; } = new List<Facility>();
    public ICollection<FacilityBooking> Bookings { get; set; } = new List<FacilityBooking>();
    public ICollection<FacilityAvailability> AvailabilitySchedules { get; set; } = new List<FacilityAvailability>();
    public ICollection<FacilityPricing> PricingRules { get; set; } = new List<FacilityPricing>();
    public ICollection<FacilityMaintenance> MaintenanceRecords { get; set; } = new List<FacilityMaintenance>();
    public ICollection<FacilityBlockout> Blockouts { get; set; } = new List<FacilityBlockout>();
}

/// <summary>
/// Represents availability schedule for a facility.
/// </summary>
public class FacilityAvailability
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }

    // Schedule
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Effective Period
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public string? SeasonName { get; set; }

    // Peak/Off-Peak
    public bool IsPeakTime { get; set; }

    // Restrictions
    public bool MembersOnly { get; set; }
    public string? AllowedMembershipTypes { get; set; } // JSON array

    // Notes
    public string? Notes { get; set; }

    // Navigation
    public Facility Facility { get; set; } = null!;
}

/// <summary>
/// Represents custom pricing rules for a facility.
/// </summary>
public class FacilityPricing
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }

    // Pricing Details
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public PricingType PricingType { get; set; }
    public string Currency { get; set; } = "GBP";

    // Schedule
    public DayOfWeek? DayOfWeek { get; set; } // null means all days
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    // Effective Period
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }

    // Eligibility
    public bool IsMemberRate { get; set; }
    public Guid? MembershipTypeId { get; set; }
    public bool IsJuniorRate { get; set; }
    public bool IsSeniorRate { get; set; }

    // Priority
    public int Priority { get; set; } // Higher priority rules apply first

    // Navigation
    public Facility Facility { get; set; } = null!;
    public MembershipType? MembershipType { get; set; }
}

/// <summary>
/// Represents a booking for a facility.
/// </summary>
public class FacilityBooking
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid ClubId { get; set; }
    public Guid? MemberId { get; set; }

    // Booking Reference
    public string BookingNumber { get; set; } = string.Empty;
    public FacilityBookingStatus Status { get; set; } = FacilityBookingStatus.Pending;

    // Date/Time
    public DateTime BookingDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int DurationMinutes { get; set; }

    // Participants
    public int NumberOfParticipants { get; set; } = 1;
    public string? ParticipantNames { get; set; } // JSON array

    // Guest Booking (for non-members)
    public bool IsGuestBooking { get; set; }
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }

    // Pricing
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? DiscountCode { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal FinalPrice { get; set; }
    public string Currency { get; set; } = "GBP";

    // Payment
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
    public Guid? PaymentId { get; set; }

    // Check-in
    public DateTime? CheckedInAt { get; set; }
    public string? CheckedInBy { get; set; }
    public DateTime? CheckedOutAt { get; set; }

    // Cancellation
    public DateTime? CancelledAt { get; set; }
    public string? CancelledBy { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? RefundAmount { get; set; }
    public bool RefundProcessed { get; set; }

    // Recurring
    public bool IsRecurring { get; set; }
    public Guid? RecurrenceGroupId { get; set; }
    public string? RecurrencePattern { get; set; } // Weekly, Monthly, etc.
    public DateTime? RecurrenceEndDate { get; set; }

    // Related
    public Guid? SessionId { get; set; } // If booked for a session
    public Guid? EventId { get; set; } // If booked for an event

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? SpecialRequests { get; set; }

    // Reminders
    public bool ReminderSent { get; set; }
    public DateTime? ReminderSentAt { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? BookingSource { get; set; } // Online, Phone, WalkIn, etc.

    // Navigation
    public Facility Facility { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member? Member { get; set; }
    public Session? Session { get; set; }
    public Event? Event { get; set; }
    public Payment? Payment { get; set; }
}

/// <summary>
/// Represents a maintenance record for a facility.
/// </summary>
public class FacilityMaintenance
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }
    public Guid? VendorId { get; set; }

    // Maintenance Details
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public MaintenanceType Type { get; set; }
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;
    public MaintenancePriority Priority { get; set; } = MaintenancePriority.Medium;

    // Schedule
    public DateTime ScheduledDate { get; set; }
    public TimeOnly? ScheduledTime { get; set; }
    public int? EstimatedDurationMinutes { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? ActualDurationMinutes { get; set; }

    // Assignment
    public string? AssignedTo { get; set; }
    public string? AssignedTeam { get; set; }
    public string? CompletedBy { get; set; }

    // Cost
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public string Currency { get; set; } = "GBP";
    public Guid? ExpenseId { get; set; }
    public Guid? PurchaseOrderId { get; set; }

    // Parts & Materials
    public string? PartsRequired { get; set; } // JSON array
    public string? PartsUsed { get; set; } // JSON array

    // Inspection
    public bool RequiresInspection { get; set; }
    public DateTime? InspectionDate { get; set; }
    public string? InspectedBy { get; set; }
    public bool PassedInspection { get; set; }
    public string? InspectionNotes { get; set; }

    // Recurring
    public bool IsRecurring { get; set; }
    public int? RecurrenceIntervalDays { get; set; }
    public DateTime? NextScheduledDate { get; set; }
    public Guid? ParentMaintenanceId { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? WorkPerformed { get; set; }
    public string? IssuesFound { get; set; }
    public string? FollowUpRequired { get; set; }

    // Attachments
    public string? AttachmentUrls { get; set; } // JSON array

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Facility Facility { get; set; } = null!;
    public Vendor? Vendor { get; set; }
    public Expense? Expense { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public FacilityMaintenance? ParentMaintenance { get; set; }
    public ICollection<FacilityMaintenance> FollowUpMaintenance { get; set; } = new List<FacilityMaintenance>();
}

/// <summary>
/// Represents a blockout period for a facility (holidays, tournaments, etc.).
/// </summary>
public class FacilityBlockout
{
    public Guid Id { get; set; }
    public Guid FacilityId { get; set; }

    // Blockout Details
    public string? Title { get; set; }
    public string? Reason { get; set; }
    public BookingSlotStatus BlockoutType { get; set; } = BookingSlotStatus.Blocked;

    // Date/Time
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeOnly? StartTime { get; set; } // null means all day
    public TimeOnly? EndTime { get; set; }
    public bool IsAllDay { get; set; } = true;

    // Recurring
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public DateTime? RecurrenceEndDate { get; set; }

    // Related
    public Guid? EventId { get; set; }
    public Guid? MaintenanceId { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public Facility Facility { get; set; } = null!;
    public Event? Event { get; set; }
    public FacilityMaintenance? Maintenance { get; set; }
}
