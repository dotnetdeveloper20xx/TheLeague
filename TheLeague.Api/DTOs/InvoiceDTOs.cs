using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record InvoiceDto(
    Guid Id,
    string InvoiceNumber,
    Guid MemberId,
    string MemberName,
    string? MemberEmail,
    DateTime InvoiceDate,
    DateTime DueDate,
    DateTime? PaidDate,
    DateTime? SentDate,
    decimal SubTotal,
    decimal? DiscountAmount,
    decimal? TaxAmount,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal BalanceDue,
    string Currency,
    InvoiceStatus Status,
    CollectionStatus CollectionStatus,
    string? BillingName,
    string? BillingAddress,
    string? BillingEmail,
    int PaymentTermsDays,
    bool IsOverdue,
    int DaysOverdue,
    int RemindersSent,
    string? Notes,
    string? PdfUrl,
    DateTime CreatedAt,
    IEnumerable<InvoiceLineItemDto> LineItems,
    IEnumerable<InvoicePaymentDto> Payments
);

public record InvoiceListDto(
    Guid Id,
    string InvoiceNumber,
    Guid MemberId,
    string MemberName,
    DateTime InvoiceDate,
    DateTime DueDate,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal BalanceDue,
    string Currency,
    InvoiceStatus Status,
    bool IsOverdue,
    int DaysOverdue
);

public record InvoiceLineItemDto(
    Guid Id,
    string Description,
    FeeType? FeeType,
    int Quantity,
    decimal UnitPrice,
    decimal? DiscountAmount,
    decimal SubTotal,
    decimal? TaxAmount,
    decimal Total,
    string? ServicePeriod,
    int SortOrder
);

public record InvoicePaymentDto(
    Guid Id,
    decimal Amount,
    DateTime PaymentDate,
    PaymentMethod Method,
    PaymentStatus Status,
    string? ReceiptNumber
);

public record InvoiceCreateRequest(
    [Required] Guid MemberId,
    [Required] DateTime DueDate,
    DateTime? InvoiceDate = null,
    string? PurchaseOrderNumber = null,
    string? BillingName = null,
    string? BillingAddress = null,
    string? BillingCity = null,
    string? BillingPostcode = null,
    string? BillingCountry = null,
    string? BillingEmail = null,
    int PaymentTermsDays = 30,
    bool AllowPartialPayment = true,
    bool AllowOnlinePayment = true,
    string? Notes = null,
    string? InternalNotes = null,
    string Currency = "GBP",
    IEnumerable<InvoiceLineItemCreateRequest>? LineItems = null
);

public record InvoiceUpdateRequest(
    DateTime? DueDate,
    string? PurchaseOrderNumber,
    string? BillingName,
    string? BillingAddress,
    string? BillingCity,
    string? BillingPostcode,
    string? BillingCountry,
    string? BillingEmail,
    int? PaymentTermsDays,
    bool? AllowPartialPayment,
    bool? AllowOnlinePayment,
    string? Notes,
    string? InternalNotes
);

public record InvoiceLineItemCreateRequest(
    [Required] string Description,
    [Required] decimal UnitPrice,
    int Quantity = 1,
    Guid? FeeId = null,
    FeeType? FeeType = null,
    decimal? DiscountAmount = null,
    decimal? DiscountPercent = null,
    decimal? TaxRate = null,
    string? ServicePeriod = null,
    DateTime? ServiceStartDate = null,
    DateTime? ServiceEndDate = null,
    string? GLAccountCode = null,
    string? CostCenter = null,
    int SortOrder = 0
);

public record InvoiceFilterRequest(
    Guid? MemberId = null,
    InvoiceStatus? Status = null,
    CollectionStatus? CollectionStatus = null,
    bool? IsOverdue = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    DateTime? DueDateFrom = null,
    DateTime? DueDateTo = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 20
);

public record InvoiceSendRequest(
    bool SendEmail = true,
    string? CustomMessage = null
);

public record InvoiceVoidRequest(
    [Required] string Reason
);

public record RecordPaymentRequest(
    [Required] decimal Amount,
    [Required] PaymentMethod Method,
    DateTime? PaymentDate = null,
    string? Reference = null,
    string? Notes = null
);

public record InvoiceSummaryDto(
    int TotalInvoices,
    int DraftCount,
    int SentCount,
    int PaidCount,
    int OverdueCount,
    decimal TotalOutstanding,
    decimal TotalOverdue,
    decimal TotalPaidThisMonth
);
