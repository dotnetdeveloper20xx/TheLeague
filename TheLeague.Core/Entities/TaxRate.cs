using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a tax rate configuration for the club.
/// Supports VAT, sales tax, and other tax types.
/// </summary>
public class TaxRate
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Tax Details
    public string Name { get; set; } = string.Empty; // e.g., "Standard VAT", "Reduced Rate"
    public string Code { get; set; } = string.Empty; // e.g., "VAT20", "VAT5"
    public string? Description { get; set; }
    public TaxRateType Type { get; set; } = TaxRateType.StandardRate;

    // Rate
    public decimal Rate { get; set; } // e.g., 20.00 for 20%
    public decimal? EffectiveRate { get; set; } // For compound rates
    public bool IsCompound { get; set; }

    // Effective Dates
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsCurrentlyActive { get; set; } = true;

    // Accounts
    public Guid? CollectedAccountId { get; set; } // Where tax collected is posted
    public Guid? PaidAccountId { get; set; } // Where tax paid is posted

    // Reporting
    public string? TaxAuthorityCode { get; set; } // For official reporting
    public string? ReportingCategory { get; set; }
    public bool IncludeInTaxReturn { get; set; } = true;
    public int? ReportingBox { get; set; } // VAT return box number

    // Settings
    public bool IsDefault { get; set; }
    public bool CanBeOverridden { get; set; } = true;
    public bool AppliesToSales { get; set; } = true;
    public bool AppliesToPurchases { get; set; } = true;
    public bool AppliesToServices { get; set; } = true;

    // Display
    public int SortOrder { get; set; }
    public string? DisplayColor { get; set; }

    // Status
    public bool IsActive { get; set; } = true;
    public bool IsSystemRate { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ChartOfAccount? CollectedAccount { get; set; }
    public ChartOfAccount? PaidAccount { get; set; }
}
