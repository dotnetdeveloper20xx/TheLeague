using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a piece of equipment owned by the club.
/// Tracks inventory, location, condition, and usage.
/// </summary>
public class Equipment
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? FacilityId { get; set; }
    public Guid? VenueId { get; set; }

    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? SerialNumber { get; set; }
    public string? BarCode { get; set; }
    public string? Description { get; set; }
    public EquipmentCategory Category { get; set; }
    public string? SubCategory { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }

    // Status & Condition
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Available;
    public EquipmentCondition Condition { get; set; } = EquipmentCondition.Good;
    public string? ConditionNotes { get; set; }

    // Inventory
    public int Quantity { get; set; } = 1;
    public int AvailableQuantity { get; set; } = 1;
    public int ReservedQuantity { get; set; }
    public int OnLoanQuantity { get; set; }
    public int MinimumStock { get; set; }
    public bool LowStockAlert { get; set; }

    // Specifications
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal? Weight { get; set; }
    public string? WeightUnit { get; set; }
    public string? Dimensions { get; set; }
    public string? Specifications { get; set; } // JSON

    // Location
    public string? StorageLocation { get; set; }
    public string? Shelf { get; set; }
    public string? Bin { get; set; }

    // Purchase Info
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? PurchaseCurrency { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public string? SupplierName { get; set; }
    public Guid? VendorId { get; set; }
    public string? InvoiceNumber { get; set; }

    // Value & Depreciation
    public decimal? CurrentValue { get; set; }
    public decimal? ReplacementCost { get; set; }
    public decimal? DepreciationRate { get; set; } // Percentage per year
    public decimal? SalvageValue { get; set; }
    public int? UsefulLifeYears { get; set; }

    // Warranty
    public bool HasWarranty { get; set; }
    public DateTime? WarrantyStartDate { get; set; }
    public DateTime? WarrantyEndDate { get; set; }
    public string? WarrantyProvider { get; set; }
    public string? WarrantyTerms { get; set; }
    public string? WarrantyContactInfo { get; set; }

    // Insurance
    public bool IsInsured { get; set; }
    public string? InsurancePolicy { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }
    public decimal? InsuredValue { get; set; }

    // Lending/Loan Settings
    public bool IsLoanable { get; set; } = true;
    public bool RequiresDeposit { get; set; }
    public decimal? DepositAmount { get; set; }
    public bool RequiresApproval { get; set; }
    public int? MaxLoanDays { get; set; }
    public decimal? DailyLoanFee { get; set; }
    public decimal? LateFeePerDay { get; set; }
    public string? LoanTerms { get; set; }

    // Membership Restrictions
    public bool MembersOnly { get; set; }
    public string? AllowedMembershipTypes { get; set; } // JSON array
    public int? MinimumAge { get; set; }
    public bool RequiresCertification { get; set; }
    public string? RequiredCertifications { get; set; } // JSON array

    // Maintenance
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public int? MaintenanceIntervalDays { get; set; }
    public string? MaintenanceNotes { get; set; }

    // Inspection
    public DateTime? LastInspectionDate { get; set; }
    public DateTime? NextInspectionDate { get; set; }
    public string? InspectionFrequency { get; set; }
    public bool PassedLastInspection { get; set; }
    public string? InspectionNotes { get; set; }

    // Images & Documents
    public string? ImageUrl { get; set; }
    public string? GalleryUrls { get; set; } // JSON array
    public string? ManualUrl { get; set; }
    public string? DocumentUrls { get; set; } // JSON array

    // Usage Tracking
    public int TotalLoanCount { get; set; }
    public int TotalUsageHours { get; set; }
    public DateTime? LastUsedDate { get; set; }
    public decimal? TotalLoanRevenue { get; set; }

    // Retirement/Disposal
    public bool IsRetired { get; set; }
    public DateTime? RetiredDate { get; set; }
    public string? RetiredBy { get; set; }
    public string? RetirementReason { get; set; }
    public string? DisposalMethod { get; set; }
    public decimal? DisposalValue { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? SafetyNotes { get; set; }
    public string? UsageInstructions { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Facility? Facility { get; set; }
    public Venue? Venue { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public Vendor? Vendor { get; set; }
    public ICollection<EquipmentLoan> Loans { get; set; } = new List<EquipmentLoan>();
    public ICollection<EquipmentMaintenance> MaintenanceRecords { get; set; } = new List<EquipmentMaintenance>();
    public ICollection<EquipmentReservation> Reservations { get; set; } = new List<EquipmentReservation>();
}

/// <summary>
/// Represents a loan/rental of equipment to a member.
/// </summary>
public class EquipmentLoan
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }

    // Loan Details
    public string LoanNumber { get; set; } = string.Empty;
    public LoanStatus Status { get; set; } = LoanStatus.Requested;
    public int Quantity { get; set; } = 1;

    // Dates
    public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public int? DaysOverdue { get; set; }

    // Condition
    public EquipmentCondition ConditionAtLoan { get; set; }
    public EquipmentCondition? ConditionAtReturn { get; set; }
    public string? ConditionNotes { get; set; }

    // Approval
    public bool RequiresApproval { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovalNotes { get; set; }

    // Deposit
    public bool DepositRequired { get; set; }
    public decimal? DepositAmount { get; set; }
    public bool DepositPaid { get; set; }
    public DateTime? DepositPaidDate { get; set; }
    public string? DepositPaymentRef { get; set; }
    public bool DepositReturned { get; set; }
    public DateTime? DepositReturnedDate { get; set; }
    public decimal? DepositDeduction { get; set; }
    public string? DeductionReason { get; set; }

    // Fees
    public decimal? DailyFee { get; set; }
    public decimal? TotalFee { get; set; }
    public decimal? LateFee { get; set; }
    public decimal? DamageFee { get; set; }
    public decimal? TotalAmount { get; set; }
    public string Currency { get; set; } = "GBP";

    // Payment
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaymentReference { get; set; }
    public Guid? PaymentId { get; set; }

    // Collection/Return
    public string? CollectedBy { get; set; }
    public DateTime? CollectedAt { get; set; }
    public string? ReturnedTo { get; set; }
    public string? ReturnLocation { get; set; }

    // Related
    public Guid? SessionId { get; set; }
    public Guid? EventId { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? MemberNotes { get; set; }

    // Reminders
    public bool ReminderSent { get; set; }
    public DateTime? ReminderSentDate { get; set; }
    public int ReminderCount { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Equipment Equipment { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Session? Session { get; set; }
    public Event? Event { get; set; }
    public Payment? Payment { get; set; }
}

/// <summary>
/// Represents maintenance performed on equipment.
/// </summary>
public class EquipmentMaintenance
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid? VendorId { get; set; }

    // Maintenance Details
    public string? Title { get; set; }
    public string? Description { get; set; }
    public EquipmentMaintenanceType Type { get; set; }
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;
    public MaintenancePriority Priority { get; set; } = MaintenancePriority.Medium;

    // Schedule
    public DateTime ScheduledDate { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? EstimatedDuration { get; set; } // Minutes
    public int? ActualDuration { get; set; } // Minutes

    // Assignment
    public string? AssignedTo { get; set; }
    public string? CompletedBy { get; set; }

    // Condition
    public EquipmentCondition? ConditionBefore { get; set; }
    public EquipmentCondition? ConditionAfter { get; set; }

    // Cost
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public string Currency { get; set; } = "GBP";
    public Guid? ExpenseId { get; set; }

    // Parts
    public string? PartsUsed { get; set; } // JSON array
    public decimal? PartsCost { get; set; }
    public decimal? LaborCost { get; set; }

    // Recurring
    public bool IsRecurring { get; set; }
    public int? RecurrenceIntervalDays { get; set; }
    public DateTime? NextScheduledDate { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? WorkPerformed { get; set; }
    public string? IssuesFound { get; set; }
    public string? Recommendations { get; set; }

    // Attachments
    public string? AttachmentUrls { get; set; } // JSON array

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Equipment Equipment { get; set; } = null!;
    public Vendor? Vendor { get; set; }
    public Expense? Expense { get; set; }
}

/// <summary>
/// Represents a reservation of equipment for future use.
/// </summary>
public class EquipmentReservation
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid ClubId { get; set; }
    public Guid? MemberId { get; set; }

    // Reservation Details
    public string ReservationNumber { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;

    // Date/Time
    public DateTime ReservationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }

    // Status
    public bool IsConfirmed { get; set; }
    public DateTime? ConfirmedDate { get; set; }
    public string? ConfirmedBy { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? CancelledBy { get; set; }
    public string? CancellationReason { get; set; }

    // Converted to Loan
    public bool ConvertedToLoan { get; set; }
    public Guid? LoanId { get; set; }
    public DateTime? ConvertedDate { get; set; }

    // Related
    public Guid? SessionId { get; set; }
    public Guid? EventId { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? Purpose { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    // Navigation
    public Equipment Equipment { get; set; } = null!;
    public Club Club { get; set; } = null!;
    public Member? Member { get; set; }
    public EquipmentLoan? Loan { get; set; }
    public Session? Session { get; set; }
    public Event? Event { get; set; }
}
