namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a guest/day pass for non-members.
/// Can be purchased directly or provided by existing members.
/// </summary>
public class GuestPass
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Pass Information
    public string PassCode { get; set; } = string.Empty; // Unique code
    public string? QRCodeData { get; set; }

    // Guest Details
    public string GuestFirstName { get; set; } = string.Empty;
    public string GuestLastName { get; set; } = string.Empty;
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }

    // Host Member (if pass is from a member's allocation)
    public Guid? HostMemberId { get; set; }
    public bool IsFromMemberAllocation { get; set; }

    // Validity
    public DateTime ValidDate { get; set; } // Single day validity
    public DateTime? ValidFrom { get; set; } // For multi-day passes
    public DateTime? ValidUntil { get; set; }
    public TimeSpan? ValidFromTime { get; set; } // e.g., 9:00 AM
    public TimeSpan? ValidUntilTime { get; set; } // e.g., 5:00 PM
    public bool IsMultiDayPass { get; set; }
    public int? NumberOfDays { get; set; }

    // Access
    public string? AllowedFacilities { get; set; } // JSON array of facility IDs (null = all)
    public bool IncludesClasses { get; set; }
    public int? MaxSessions { get; set; }
    public int SessionsUsed { get; set; }

    // Status
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public bool IsExpired { get; set; }
    public bool IsCancelled { get; set; }
    public string? CancellationReason { get; set; }

    // Pricing
    public decimal Price { get; set; }
    public decimal? DiscountApplied { get; set; }
    public decimal FinalPrice { get; set; }
    public Guid? PaymentId { get; set; }
    public bool IsPaid { get; set; }
    public bool IsComplimentary { get; set; }

    // Conversion Tracking
    public bool ConvertedToMember { get; set; }
    public Guid? ConvertedMemberId { get; set; }
    public DateTime? ConversionDate { get; set; }

    // Waiver
    public bool WaiverSigned { get; set; }
    public DateTime? WaiverSignedDate { get; set; }
    public string? WaiverSignatureUrl { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Member? HostMember { get; set; }
    public Payment? Payment { get; set; }
    public Member? ConvertedMember { get; set; }
}
