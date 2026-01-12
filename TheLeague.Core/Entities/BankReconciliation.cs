using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a bank reconciliation for a specific period and bank account.
/// </summary>
public class BankReconciliation
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid BankAccountId { get; set; } // ChartOfAccount with IsBankAccount = true

    // Reconciliation Period
    public DateTime StatementDate { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string? StatementReference { get; set; }

    // Opening & Closing
    public decimal OpeningBalance { get; set; }
    public decimal ClosingBalance { get; set; } // Per bank statement
    public string Currency { get; set; } = "GBP";

    // Book Balances
    public decimal BookOpeningBalance { get; set; }
    public decimal BookClosingBalance { get; set; }

    // Reconciliation Summary
    public decimal TotalDeposits { get; set; }
    public decimal TotalWithdrawals { get; set; }
    public decimal TotalBankFees { get; set; }
    public decimal TotalInterest { get; set; }

    // Outstanding Items
    public decimal OutstandingDeposits { get; set; } // Deposits in transit
    public decimal OutstandingWithdrawals { get; set; } // Outstanding cheques
    public int OutstandingItemsCount { get; set; }

    // Adjustments
    public decimal Adjustments { get; set; }
    public string? AdjustmentNotes { get; set; }

    // Variance
    public decimal Variance { get; set; } // Difference between book and bank
    public decimal AdjustedBookBalance { get; set; }
    public bool IsBalanced { get; set; }

    // Status
    public ReconciliationStatus Status { get; set; } = ReconciliationStatus.NotStarted;
    public DateTime? StartedDate { get; set; }
    public string? StartedBy { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? CompletedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }

    // Statistics
    public int TotalTransactions { get; set; }
    public int ReconciledTransactions { get; set; }
    public int UnreconciledTransactions { get; set; }
    public decimal ReconciledPercentage { get; set; }

    // Statement Import
    public string? ImportedFileName { get; set; }
    public DateTime? ImportedDate { get; set; }
    public string? ImportFormat { get; set; } // OFX, CSV, QIF, etc.

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ChartOfAccount BankAccount { get; set; } = null!;
    public ICollection<BankReconciliationLine> Lines { get; set; } = new List<BankReconciliationLine>();
}

/// <summary>
/// Represents a line item in a bank reconciliation.
/// </summary>
public class BankReconciliationLine
{
    public Guid Id { get; set; }
    public Guid BankReconciliationId { get; set; }
    public Guid? JournalEntryLineId { get; set; }

    // Transaction Details
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public string? Payee { get; set; }

    // Amounts
    public decimal Deposit { get; set; }
    public decimal Withdrawal { get; set; }
    public decimal RunningBalance { get; set; }
    public string Currency { get; set; } = "GBP";

    // Bank Statement Data
    public string? BankReference { get; set; }
    public string? BankDescription { get; set; }
    public string? BankTransactionType { get; set; }

    // Matching
    public bool IsReconciled { get; set; }
    public DateTime? ReconciledDate { get; set; }
    public string? ReconciledBy { get; set; }
    public string? MatchType { get; set; } // Auto, Manual, Rule-based
    public decimal? MatchConfidence { get; set; } // For auto-matching

    // Bank-only items
    public bool IsBankOnlyItem { get; set; } // Appears on statement but not in books
    public bool RequiresJournalEntry { get; set; }
    public Guid? CreatedJournalEntryId { get; set; }

    // Book-only items
    public bool IsBookOnlyItem { get; set; } // In books but not cleared
    public bool IsOutstanding { get; set; }
    public DateTime? ExpectedClearDate { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public BankReconciliation BankReconciliation { get; set; } = null!;
    public JournalEntryLine? JournalEntryLine { get; set; }
}
