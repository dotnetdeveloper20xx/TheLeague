using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Tracks membership freeze/pause history.
/// Supports medical, travel, financial, and other freeze reasons.
/// </summary>
public class MembershipFreeze
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MembershipId { get; set; }
    public Guid MemberId { get; set; }

    // Freeze Period
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? ActualEndDate { get; set; } // If ended early
    public int DurationDays { get; set; }

    // Reason
    public FreezeReason Reason { get; set; }
    public string? ReasonDetails { get; set; }
    public string? SupportingDocuments { get; set; } // JSON array of document URLs

    // Status
    public bool IsActive { get; set; }
    public bool ApprovedByStaff { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? RejectionReason { get; set; }

    // Financial
    public decimal? FeeDuringFreeze { get; set; }
    public bool FeePaid { get; set; }
    public Guid? FreezePaymentId { get; set; }

    // Impact on Membership
    public DateTime? OriginalMembershipEndDate { get; set; }
    public DateTime? ExtendedMembershipEndDate { get; set; } // End date after freeze extension

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Membership Membership { get; set; } = null!;
    public Member Member { get; set; } = null!;
}
