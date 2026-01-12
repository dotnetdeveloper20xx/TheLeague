using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a budget for financial planning and tracking.
/// </summary>
public class Budget
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }
    public Guid FiscalYearId { get; set; }

    // Budget Details
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Reference { get; set; }
    public BudgetStatus Status { get; set; } = BudgetStatus.Draft;

    // Period
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Currency { get; set; } = "GBP";

    // Totals
    public decimal TotalBudgetedRevenue { get; set; }
    public decimal TotalBudgetedExpenses { get; set; }
    public decimal TotalBudgetedProfit { get; set; }
    public decimal TotalActualRevenue { get; set; }
    public decimal TotalActualExpenses { get; set; }
    public decimal TotalActualProfit { get; set; }
    public decimal RevenueVariance { get; set; }
    public decimal ExpenseVariance { get; set; }
    public decimal ProfitVariance { get; set; }
    public decimal RevenueVariancePercent { get; set; }
    public decimal ExpenseVariancePercent { get; set; }

    // Version Control
    public int Version { get; set; } = 1;
    public Guid? PreviousVersionId { get; set; }
    public bool IsLatestVersion { get; set; } = true;
    public string? VersionNotes { get; set; }

    // Approval
    public bool RequiresApproval { get; set; }
    public string? PreparedBy { get; set; }
    public DateTime? PreparedDate { get; set; }
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? RejectedBy { get; set; }
    public DateTime? RejectedDate { get; set; }
    public string? RejectionReason { get; set; }

    // Tracking
    public bool TrackActuals { get; set; } = true;
    public DateTime? ActualsLastUpdated { get; set; }
    public bool AlertOnOverBudget { get; set; } = true;
    public decimal? OverBudgetThreshold { get; set; } // Percentage threshold

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }
    public string? Assumptions { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public FiscalYear FiscalYear { get; set; } = null!;
    public Budget? PreviousVersion { get; set; }
    public ICollection<BudgetLine> Lines { get; set; } = new List<BudgetLine>();
}

/// <summary>
/// Represents a line item in a budget.
/// </summary>
public class BudgetLine
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public Guid AccountId { get; set; }

    // Line Details
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }

    // Budgeted Amounts by Period
    public decimal Period1Amount { get; set; }
    public decimal Period2Amount { get; set; }
    public decimal Period3Amount { get; set; }
    public decimal Period4Amount { get; set; }
    public decimal Period5Amount { get; set; }
    public decimal Period6Amount { get; set; }
    public decimal Period7Amount { get; set; }
    public decimal Period8Amount { get; set; }
    public decimal Period9Amount { get; set; }
    public decimal Period10Amount { get; set; }
    public decimal Period11Amount { get; set; }
    public decimal Period12Amount { get; set; }
    public decimal TotalBudgeted { get; set; }

    // Actuals by Period
    public decimal Period1Actual { get; set; }
    public decimal Period2Actual { get; set; }
    public decimal Period3Actual { get; set; }
    public decimal Period4Actual { get; set; }
    public decimal Period5Actual { get; set; }
    public decimal Period6Actual { get; set; }
    public decimal Period7Actual { get; set; }
    public decimal Period8Actual { get; set; }
    public decimal Period9Actual { get; set; }
    public decimal Period10Actual { get; set; }
    public decimal Period11Actual { get; set; }
    public decimal Period12Actual { get; set; }
    public decimal TotalActual { get; set; }

    // Variance
    public decimal Variance { get; set; }
    public decimal VariancePercent { get; set; }
    public bool IsOverBudget { get; set; }

    // Calculations
    public string? CalculationMethod { get; set; } // Fixed, Percent, Formula
    public string? Formula { get; set; }
    public Guid? BasedOnLineId { get; set; }
    public decimal? PercentOfBase { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? Justification { get; set; }

    // Sort
    public int SortOrder { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public Budget Budget { get; set; } = null!;
    public ChartOfAccount Account { get; set; } = null!;
    public BudgetLine? BasedOnLine { get; set; }
}
