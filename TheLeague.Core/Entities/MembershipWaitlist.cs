using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Manages waitlist for capped membership types.
/// Tracks position, offers, and conversions.
/// </summary>
public class MembershipWaitlist
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MembershipTypeId { get; set; }

    // Applicant Information
    public Guid? MemberId { get; set; } // If existing member
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    // Waitlist Position
    public int Position { get; set; }
    public DateTime JoinedWaitlistDate { get; set; } = DateTime.UtcNow;
    public WaitlistStatus Status { get; set; } = WaitlistStatus.Waiting;

    // Offer Details
    public DateTime? OfferSentDate { get; set; }
    public DateTime? OfferExpiryDate { get; set; }
    public int OfferValidDays { get; set; } = 7;
    public int OfferCount { get; set; } // Number of times offered
    public DateTime? LastOfferDeclinedDate { get; set; }
    public string? DeclineReason { get; set; }

    // Acceptance
    public DateTime? AcceptedDate { get; set; }
    public Guid? CreatedMembershipId { get; set; }

    // Removal
    public DateTime? RemovedDate { get; set; }
    public string? RemovalReason { get; set; }
    public string? RemovedBy { get; set; }

    // Priority & Notes
    public int PriorityScore { get; set; } // Higher = more priority
    public bool IsVIP { get; set; }
    public string? ReferredBy { get; set; }
    public Guid? ReferringMemberId { get; set; }
    public string? Notes { get; set; }

    // Notification Preferences
    public bool EmailNotificationsEnabled { get; set; } = true;
    public bool SmsNotificationsEnabled { get; set; }
    public DateTime? LastNotificationSent { get; set; }
    public int NotificationCount { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public MembershipType MembershipType { get; set; } = null!;
    public Member? Member { get; set; }
    public Member? ReferringMember { get; set; }
    public Membership? CreatedMembership { get; set; }
}
