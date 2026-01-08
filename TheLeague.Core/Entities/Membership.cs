using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

public class Membership
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid MemberId { get; set; }
    public Guid MembershipTypeId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MembershipPaymentType PaymentType { get; set; }
    public MembershipStatus Status { get; set; }

    public decimal AmountPaid { get; set; }
    public decimal AmountDue { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }

    public bool AutoRenew { get; set; }
    public string? Notes { get; set; }

    public Member Member { get; set; } = null!;
    public MembershipType MembershipType { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
