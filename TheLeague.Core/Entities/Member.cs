using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class Member
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public string? UserId { get; set; }

    // Personal Details
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? ProfilePhotoUrl { get; set; }

    // Emergency Contact
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }

    // Medical Info (optional)
    public string? MedicalConditions { get; set; }
    public string? Allergies { get; set; }
    public string? DoctorName { get; set; }
    public string? DoctorPhone { get; set; }

    // Family Account
    public bool IsFamilyAccount { get; set; }
    public Guid? PrimaryMemberId { get; set; }
    public Member? PrimaryMember { get; set; }
    public ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();

    // Status
    public MemberStatus Status { get; set; } = MemberStatus.Pending;
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; }

    // Stripe/PayPal Customer IDs
    public string? StripeCustomerId { get; set; }
    public string? PayPalPayerId { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<SessionBooking> SessionBookings { get; set; } = new List<SessionBooking>();
    public ICollection<EventTicket> EventTickets { get; set; } = new List<EventTicket>();
    public ICollection<MemberDocument> Documents { get; set; } = new List<MemberDocument>();

    public string FullName => $"{FirstName} {LastName}";
}
