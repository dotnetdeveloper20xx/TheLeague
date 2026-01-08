namespace TheLeague.Core.Entities;

public class MembershipType
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal AnnualFee { get; set; }
    public decimal? MonthlyFee { get; set; }
    public decimal? SessionFee { get; set; }

    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? MaxFamilyMembers { get; set; }

    public bool IsActive { get; set; } = true;
    public bool AllowOnlineSignup { get; set; } = true;
    public int SortOrder { get; set; }

    // Benefits/Features
    public bool IncludesBooking { get; set; } = true;
    public bool IncludesEvents { get; set; } = true;
    public int? MaxSessionsPerWeek { get; set; }

    public Club Club { get; set; } = null!;
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}
