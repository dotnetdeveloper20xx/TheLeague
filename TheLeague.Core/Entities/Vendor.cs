using TheLeague.Core.Enums;

namespace TheLeague.Core.Entities;

/// <summary>
/// Represents a vendor/supplier for the club.
/// Tracks vendor details, payment terms, and relationship history.
/// </summary>
public class Vendor
{
    public Guid Id { get; set; }
    public Guid ClubId { get; set; }

    // Basic Info
    public string VendorNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? TradingName { get; set; }
    public VendorType Type { get; set; } = VendorType.Supplier;
    public VendorStatus Status { get; set; } = VendorStatus.Active;

    // Contact Information
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Website { get; set; }

    // Address
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? PostCode { get; set; }
    public string? Country { get; set; }

    // Tax & Registration
    public string? TaxId { get; set; } // VAT number
    public string? CompanyNumber { get; set; }
    public bool IsVatRegistered { get; set; }
    public string? TaxClassification { get; set; }

    // Bank Details
    public string? BankName { get; set; }
    public string? BankAccountName { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? SortCode { get; set; }
    public string? IBAN { get; set; }
    public string? SwiftCode { get; set; }

    // Payment Terms
    public PaymentTerms PaymentTerms { get; set; } = PaymentTerms.Net30;
    public int? CustomPaymentDays { get; set; }
    public string Currency { get; set; } = "GBP";
    public Guid? DefaultExpenseAccountId { get; set; }
    public Guid? DefaultTaxRateId { get; set; }

    // Credit
    public decimal? CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }

    // Categorization
    public string? Categories { get; set; } // JSON array
    public string? Tags { get; set; } // JSON array

    // Relationship
    public DateTime? RelationshipStartDate { get; set; }
    public string? AccountManager { get; set; }
    public string? PreferredCommunication { get; set; }

    // Performance
    public decimal? AveragePaymentDays { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpend { get; set; }
    public decimal TotalSpendYTD { get; set; }
    public DateTime? LastOrderDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? Rating { get; set; } // Good, Average, Poor

    // Status & Compliance
    public bool RequiresW9 { get; set; }
    public bool W9OnFile { get; set; }
    public DateTime? W9ReceivedDate { get; set; }
    public bool RequiresInsuranceCert { get; set; }
    public bool InsuranceCertOnFile { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }

    // Approval
    public bool RequiresPOApproval { get; set; }
    public decimal? POApprovalThreshold { get; set; }

    // Notes
    public string? Notes { get; set; }
    public string? InternalNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation
    public Club Club { get; set; } = null!;
    public ChartOfAccount? DefaultExpenseAccount { get; set; }
    public TaxRate? DefaultTaxRate { get; set; }
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
