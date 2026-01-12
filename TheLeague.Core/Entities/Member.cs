using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class Member
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public string? UserId { get; set; }

    // Member Identification
    public string MemberNumber { get; set; } = string.Empty; // Unique per club, e.g., "MBR-001"
    public string? QRCodeData { get; set; } // Encoded data for QR code generation

    // Personal Details
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? Address { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }
    public string? ProfilePhotoUrl { get; set; }

    // Emergency Contact
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }

    // Secondary Emergency Contact
    public string? SecondaryEmergencyContactName { get; set; }
    public string? SecondaryEmergencyContactPhone { get; set; }
    public string? SecondaryEmergencyContactRelation { get; set; }

    // Medical Info (optional)
    public string? MedicalConditions { get; set; }
    public string? Allergies { get; set; }
    public string? DoctorName { get; set; }
    public string? DoctorPhone { get; set; }
    public string? BloodType { get; set; }
    public string? MedicalNotes { get; set; }

    // Social Media Links
    public string? FacebookUrl { get; set; }
    public string? TwitterHandle { get; set; }
    public string? InstagramHandle { get; set; }
    public string? LinkedInUrl { get; set; }

    // Custom Fields (JSON)
    public string? CustomFieldValues { get; set; } // JSON: {"field1": "value1", "field2": "value2"}

    // Preferences
    public bool MarketingOptIn { get; set; }
    public bool SmsOptIn { get; set; }
    public bool EmailOptIn { get; set; } = true;
    public string? PreferredContactMethod { get; set; }
    public string? PreferredLanguage { get; set; }

    // Family Account
    public bool IsFamilyAccount { get; set; }
    public Guid? PrimaryMemberId { get; set; }
    public Member? PrimaryMember { get; set; }
    public ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();

    // Onboarding & Application
    public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Approved;
    public DateTime? ApplicationDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? ApprovedBy { get; set; }
    public bool WaiverAccepted { get; set; }
    public DateTime? WaiverAcceptedDate { get; set; }
    public string? WaiverSignatureUrl { get; set; }
    public bool TermsAccepted { get; set; }
    public DateTime? TermsAcceptedDate { get; set; }
    public bool OrientationCompleted { get; set; }
    public DateTime? OrientationDate { get; set; }

    // Status
    public MemberStatus Status { get; set; } = MemberStatus.Pending;
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginDate { get; set; }
    public DateTime? LastActivityDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; }
    public string? DeactivationReason { get; set; }
    public DateTime? DeactivatedAt { get; set; }

    // Referral
    public Guid? ReferredByMemberId { get; set; }
    public string? ReferralSource { get; set; }

    // Stripe/PayPal Customer IDs
    public string? StripeCustomerId { get; set; }
    public string? PayPalPayerId { get; set; }
    public string? GoCardlessCustomerId { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public Member? ReferredByMember { get; set; }
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<SessionBooking> SessionBookings { get; set; } = new List<SessionBooking>();
    public ICollection<EventTicket> EventTickets { get; set; } = new List<EventTicket>();
    public ICollection<MemberDocument> Documents { get; set; } = new List<MemberDocument>();
    public ICollection<MemberNote> Notes { get; set; } = new List<MemberNote>();

    public string FullName => $"{FirstName} {LastName}";
    public int? Age => DateOfBirth.HasValue ? (int)((DateTime.Today - DateOfBirth.Value).TotalDays / 365.25) : null;
}
