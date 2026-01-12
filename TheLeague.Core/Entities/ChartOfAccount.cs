using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents an account in the chart of accounts.
/// Supports hierarchical account structure for financial management.
/// </summary>
public class ChartOfAccount
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Account Details
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AccountCategory Category { get; set; }
    public AccountType Type { get; set; }

    // Hierarchy
    public Guid? ParentAccountId { get; set; }
    public int Level { get; set; } = 1;
    public string? FullPath { get; set; } // e.g., "4000-4100-4110"
    public bool IsHeader { get; set; } // Group/header account (no transactions)

    // Balances
    public decimal OpeningBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal YearToDateBalance { get; set; }
    public decimal LastYearBalance { get; set; }
    public string Currency { get; set; } = "GBP";
    public bool IsDebitNormal { get; set; } // Normal balance side

    // Tax
    public Guid? DefaultTaxRateId { get; set; }
    public bool IsTaxable { get; set; }

    // Bank Details (for bank accounts)
    public bool IsBankAccount { get; set; }
    public string? BankName { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? SortCode { get; set; }
    public string? IBAN { get; set; }
    public string? SwiftCode { get; set; }

    // Settings
    public bool AllowDirectPosting { get; set; } = true;
    public bool RequiresDepartment { get; set; }
    public bool RequiresProject { get; set; }
    public bool ShowOnDashboard { get; set; }

    // Status
    public bool IsActive { get; set; } = true;
    public bool IsSystemAccount { get; set; } // Cannot be deleted
    public bool IsLocked { get; set; }
    public DateTime? LockedDate { get; set; }
    public string? LockedBy { get; set; }

    // Sort & Display
    public int SortOrder { get; set; }
    public string? DisplayColor { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ChartOfAccount? ParentAccount { get; set; }
    public TaxRate? DefaultTaxRate { get; set; }
    public ICollection<ChartOfAccount> ChildAccounts { get; set; } = new List<ChartOfAccount>();
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}
