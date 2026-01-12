using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Comprehensive audit log for all financial transactions and changes.
/// Provides complete audit trail for compliance and review.
/// </summary>
public class FinancialAuditLog
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Action Details
    public AuditActionType Action { get; set; }
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    public string ActionBy { get; set; } = string.Empty;
    public string? ActionByIpAddress { get; set; }
    public string? ActionByUserAgent { get; set; }

    // Entity Information
    public string EntityType { get; set; } = string.Empty; // JournalEntry, Payment, Invoice, etc.
    public Guid EntityId { get; set; }
    public string? EntityReference { get; set; }

    // Change Details
    public string? Description { get; set; }
    public string? OldValues { get; set; } // JSON
    public string? NewValues { get; set; } // JSON
    public string? ChangeSummary { get; set; }

    // Financial Impact
    public decimal? AmountBefore { get; set; }
    public decimal? AmountAfter { get; set; }
    public decimal? AmountChange { get; set; }
    public string Currency { get; set; } = "GBP";

    // Affected Accounts
    public string? AffectedAccounts { get; set; } // JSON array of account IDs
    public int? AffectedAccountsCount { get; set; }

    // Related Records
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public Guid? ParentAuditLogId { get; set; } // For grouped changes

    // Approval Context
    public bool RequiredApproval { get; set; }
    public string? ApproverRole { get; set; }
    public Guid? ApprovalId { get; set; }

    // Fiscal Period
    public Guid? FiscalPeriodId { get; set; }
    public Guid? FiscalYearId { get; set; }

    // Session Information
    public string? SessionId { get; set; }
    public string? RequestId { get; set; }

    // Classification
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public int Severity { get; set; } // 1=Low, 2=Medium, 3=High, 4=Critical

    // Compliance
    public bool IsComplianceRelevant { get; set; }
    public string? ComplianceFlags { get; set; } // JSON array
    public bool HasBeenReviewed { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string? ReviewedBy { get; set; }
    public string? ReviewNotes { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata - cannot be modified after creation
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Club Club { get; set; } = null!;
    public FiscalPeriod? FiscalPeriod { get; set; }
    public FiscalYear? FiscalYear { get; set; }
    public FinancialAuditLog? ParentAuditLog { get; set; }
}

/// <summary>
/// Saved financial report configuration for repeated use.
/// </summary>
public class SavedFinancialReport
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Report Details
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public FinancialReportType Type { get; set; }

    // Configuration
    public string? FilterCriteria { get; set; } // JSON configuration
    public string? ColumnConfiguration { get; set; } // JSON
    public string? SortConfiguration { get; set; } // JSON
    public string? GroupingConfiguration { get; set; } // JSON
    public string? CalculatedFields { get; set; } // JSON

    // Date Range
    public string? DateRangeType { get; set; } // Custom, ThisMonth, LastMonth, etc.
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Comparison
    public bool IncludePreviousPeriod { get; set; }
    public bool IncludeBudget { get; set; }
    public bool ShowVariance { get; set; }

    // Display
    public string? ChartType { get; set; }
    public bool ShowChart { get; set; }
    public bool ShowTable { get; set; } = true;
    public string? DisplayFormat { get; set; }

    // Scheduling
    public bool IsScheduled { get; set; }
    public string? Schedule { get; set; } // Cron expression
    public DateTime? NextRunDate { get; set; }
    public DateTime? LastRunDate { get; set; }
    public string? EmailRecipients { get; set; } // JSON array of emails
    public string? ExportFormat { get; set; } // PDF, Excel, CSV

    // Access
    public bool IsPublic { get; set; }
    public bool IsDefault { get; set; }
    public string? SharedWithRoles { get; set; } // JSON array of roles
    public string? SharedWithUsers { get; set; } // JSON array of user IDs

    // Status
    public bool IsActive { get; set; } = true;
    public bool IsFavorite { get; set; }
    public int RunCount { get; set; }

    // Notes
    public string? Notes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
}
