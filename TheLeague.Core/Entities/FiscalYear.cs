using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a fiscal year for financial tracking and reporting.
/// </summary>
public class FiscalYear
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Year Details
    public string Name { get; set; } = string.Empty; // e.g., "FY 2024/25"
    public int YearNumber { get; set; } // e.g., 2024
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Status
    public FiscalPeriodStatus Status { get; set; } = FiscalPeriodStatus.Current;
    public bool IsCurrent { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? ClosedBy { get; set; }
    public DateTime? LockedDate { get; set; }
    public string? LockedBy { get; set; }

    // Opening Balances
    public bool OpeningBalancesPosted { get; set; }
    public DateTime? OpeningBalancesPostedDate { get; set; }
    public string? OpeningBalancesPostedBy { get; set; }

    // Closing Entries
    public bool ClosingEntriesPosted { get; set; }
    public DateTime? ClosingEntriesPostedDate { get; set; }
    public string? ClosingEntriesPostedBy { get; set; }
    public Guid? ClosingJournalEntryId { get; set; }

    // Summary Balances
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetIncome { get; set; }
    public decimal TotalAssets { get; set; }
    public decimal TotalLiabilities { get; set; }
    public decimal TotalEquity { get; set; }
    public string Currency { get; set; } = "GBP";
    public DateTime? BalancesCalculatedAt { get; set; }

    // Settings
    public int NumberOfPeriods { get; set; } = 12; // Usually 12 months
    public bool AllowPostingToClosed { get; set; }
    public bool RequireApprovalToClose { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ICollection<FiscalPeriod> Periods { get; set; } = new List<FiscalPeriod>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}

/// <summary>
/// Represents a fiscal period (typically a month) within a fiscal year.
/// </summary>
public class FiscalPeriod
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid FiscalYearId { get; set; }

    // Period Details
    public string Name { get; set; } = string.Empty; // e.g., "January 2024"
    public int PeriodNumber { get; set; } // 1-12
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Status
    public FiscalPeriodStatus Status { get; set; } = FiscalPeriodStatus.Future;
    public bool IsCurrent { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? ClosedBy { get; set; }
    public DateTime? LockedDate { get; set; }
    public string? LockedBy { get; set; }
    public DateTime? ReopenedDate { get; set; }
    public string? ReopenedBy { get; set; }
    public string? ReopenReason { get; set; }

    // Adjusting Entries
    public bool AllowAdjustingEntries { get; set; }
    public int AdjustingEntriesCount { get; set; }
    public DateTime? LastAdjustingEntryDate { get; set; }

    // Summary Balances
    public decimal TotalRevenue { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetIncome { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
    public int TransactionCount { get; set; }
    public string Currency { get; set; } = "GBP";
    public DateTime? BalancesCalculatedAt { get; set; }

    // Reconciliation
    public bool IsReconciled { get; set; }
    public DateTime? ReconciledDate { get; set; }
    public string? ReconciledBy { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public FiscalYear FiscalYear { get; set; } = null!;
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
