using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Defines discount rules that can be applied to membership types.
/// Supports various discount types: early bird, loyalty, family, corporate, promotional, etc.
/// </summary>
public class MembershipDiscount
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid? MembershipTypeId { get; set; } // Null = applies to all types

    // Basic Information
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PromoCode { get; set; } // Promotional code (unique per club)
    public DiscountType Type { get; set; }

    // Discount Value
    public decimal? PercentageOff { get; set; } // e.g., 10 for 10%
    public decimal? FixedAmountOff { get; set; } // Fixed amount discount
    public decimal? FinalPrice { get; set; } // Override to specific price

    // Validity Period
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
    public bool IsActive { get; set; } = true;

    // Usage Limits
    public int? MaxTotalUses { get; set; } // Total times this discount can be used
    public int CurrentUseCount { get; set; }
    public int? MaxUsesPerMember { get; set; } // Times a single member can use
    public bool FirstTimeJoinersOnly { get; set; } // Only for new members

    // Eligibility Rules
    public int? MinTenureMonths { get; set; } // For loyalty discounts
    public int? MaxTenureMonths { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? MinFamilySize { get; set; } // For family discounts
    public string? RequiredMembershipTypes { get; set; } // JSON array of type IDs
    public string? ExcludedMembershipTypes { get; set; } // JSON array
    public bool RequiresReferral { get; set; }
    public string? ReferrerMembershipTypes { get; set; } // Types that can refer

    // Stacking Rules
    public bool CanStackWithOther { get; set; } // Can combine with other discounts
    public int Priority { get; set; } // Higher priority applied first

    // Corporate/Group Settings
    public string? CorporatePartnerName { get; set; }
    public string? CorporatePartnerCode { get; set; }
    public int? CorporateMinMembers { get; set; }

    // Seasonal Settings
    public string? SeasonalMonths { get; set; } // JSON array: [1,2,3] for Jan-Mar

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public MembershipType? MembershipType { get; set; }
}
