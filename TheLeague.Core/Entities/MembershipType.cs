using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Defines a membership plan/type that members can subscribe to.
/// Supports various pricing models, access levels, and lifecycle configurations.
/// </summary>
public class MembershipType
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Basic Information
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; } // For display in lists
    public MembershipCategory Category { get; set; } = MembershipCategory.Individual;
    public string? ColorCode { get; set; } // For UI display
    public string? IconName { get; set; }

    // Pricing
    public decimal BasePrice { get; set; } // Base price before discounts
    public decimal? WeeklyFee { get; set; }
    public decimal? FortnightlyFee { get; set; }
    public decimal? MonthlyFee { get; set; }
    public decimal? QuarterlyFee { get; set; }
    public decimal? BiannualFee { get; set; }
    public decimal AnnualFee { get; set; }
    public decimal? LifetimeFee { get; set; }
    public decimal? SessionFee { get; set; } // Pay-as-you-go rate
    public decimal? JoiningFee { get; set; } // One-time registration fee
    public decimal? AdminFee { get; set; } // Annual admin fee
    public string Currency { get; set; } = "GBP";

    // Billing Configuration
    public BillingCycle DefaultBillingCycle { get; set; } = BillingCycle.Annual;
    public bool AllowMonthlyPayment { get; set; } = true;
    public bool AllowQuarterlyPayment { get; set; } = true;
    public bool AllowAnnualPayment { get; set; } = true;
    public bool ProRataEnabled { get; set; } = true; // Pro-rata for mid-cycle joins
    public int? ProRataMinDays { get; set; } = 7; // Minimum days to charge pro-rata

    // Age Restrictions
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }

    // Family/Group Settings
    public int? MaxFamilyMembers { get; set; }
    public int? MinFamilyMembers { get; set; }
    public decimal? AdditionalMemberFee { get; set; } // Fee per additional family member

    // Access & Benefits
    public AccessType AccessType { get; set; } = AccessType.FullAccess;
    public bool IncludesBooking { get; set; } = true;
    public bool IncludesEvents { get; set; } = true;
    public bool IncludesClasses { get; set; } = true;
    public bool IncludesGym { get; set; } = true;
    public int? MaxSessionsPerWeek { get; set; }
    public int? MaxSessionsPerMonth { get; set; }
    public int? MaxBookingsPerDay { get; set; }
    public int? AdvanceBookingDays { get; set; } // Days in advance member can book
    public string? IncludedFacilities { get; set; } // JSON array of facility IDs
    public string? ExcludedFacilities { get; set; } // JSON array of facility IDs
    public int? GuestPassesIncluded { get; set; } // Number of guest passes per period
    public int? GuestPassResetPeriodDays { get; set; } = 365; // Period for guest pass reset

    // Trial & Promotional
    public bool IsTrial { get; set; }
    public int? TrialDurationDays { get; set; }
    public bool IsPromotional { get; set; }
    public DateTime? PromotionStartDate { get; set; }
    public DateTime? PromotionEndDate { get; set; }
    public decimal? PromotionalPrice { get; set; }
    public bool IsComplimentary { get; set; } // Sponsored/free membership
    public bool IsDayPass { get; set; } // Day/guest pass type

    // Capacity & Availability
    public int? MaxMembers { get; set; } // Cap on total members (enables waitlist)
    public int CurrentMemberCount { get; set; }
    public bool HasWaitlist { get; set; }
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableUntil { get; set; }

    // Renewal & Lifecycle
    public bool AutoRenewDefault { get; set; } = true;
    public int RenewalReminderDays { get; set; } = 30; // Days before expiry
    public int? GracePeriodDays { get; set; } = 14; // Days after expiry before suspension
    public bool AllowFreeze { get; set; } = true;
    public int? MaxFreezeDays { get; set; } = 90; // Max freeze duration per year
    public int? MinFreezeNoticeDays { get; set; } = 7; // Days notice required
    public decimal? FreezeFeePerMonth { get; set; } // Fee during freeze

    // Upgrade/Downgrade
    public bool AllowUpgrade { get; set; } = true;
    public bool AllowDowngrade { get; set; } = true;
    public string? UpgradeToIds { get; set; } // JSON array of membership type IDs
    public string? DowngradeToIds { get; set; } // JSON array of membership type IDs

    // Cancellation
    public int? CancellationNoticeDays { get; set; } = 30;
    public decimal? EarlyCancellationFee { get; set; }
    public int? MinCommitmentMonths { get; set; } // Minimum commitment period
    public bool RequireCancellationReason { get; set; }

    // Pricing Rules
    public bool GrandfatherExistingPrice { get; set; } // Keep old price for existing members
    public DateTime? NextPriceIncreaseDate { get; set; }
    public decimal? NewPriceAfterIncrease { get; set; }

    // Status & Display
    public bool IsActive { get; set; } = true;
    public bool AllowOnlineSignup { get; set; } = true;
    public bool ShowOnWebsite { get; set; } = true;
    public bool IsFeatured { get; set; } // Highlight on pricing page
    public int SortOrder { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<MembershipDiscount> Discounts { get; set; } = new List<MembershipDiscount>();
    public ICollection<MembershipWaitlist> Waitlist { get; set; } = new List<MembershipWaitlist>();
}
