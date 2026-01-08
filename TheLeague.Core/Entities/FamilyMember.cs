using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class FamilyMember
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid PrimaryMemberId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public FamilyMemberRelation Relation { get; set; }
    public string? MedicalConditions { get; set; }
    public string? Allergies { get; set; }
    public bool IsActive { get; set; } = true;

    public Member PrimaryMember { get; set; } = null!;
    public ICollection<SessionBooking> SessionBookings { get; set; } = new List<SessionBooking>();

    public string FullName => $"{FirstName} {LastName}";
}
