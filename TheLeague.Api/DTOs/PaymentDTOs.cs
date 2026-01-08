using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record PaymentDto(
    Guid Id,
    Guid MemberId,
    string MemberName,
    Guid? MembershipId,
    Guid? EventTicketId,
    decimal Amount,
    string Currency,
    PaymentStatus Status,
    PaymentMethod Method,
    PaymentType Type,
    string? Description,
    DateTime PaymentDate,
    DateTime? ProcessedDate,
    string? ReceiptNumber,
    string? ReceiptUrl,
    string? ManualPaymentReference,
    string? RecordedBy
);

public record PaymentListDto(
    Guid Id,
    string MemberName,
    decimal Amount,
    string Currency,
    PaymentStatus Status,
    PaymentMethod Method,
    PaymentType Type,
    DateTime PaymentDate,
    string? ReceiptNumber
);

public record ManualPaymentRequest(
    [Required] Guid MemberId,
    [Required] decimal Amount,
    [Required] PaymentMethod Method,
    Guid? MembershipId = null,
    string Currency = "GBP",
    PaymentType Type = PaymentType.Membership,
    string? Description = null,
    string? ManualPaymentReference = null
);

public record PaymentFilterRequest(
    Guid? MemberId,
    PaymentStatus? Status,
    PaymentMethod? Method,
    PaymentType? Type,
    DateTime? DateFrom,
    DateTime? DateTo,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "PaymentDate",
    bool SortDescending = true
);

public record PaymentSummaryDto(
    decimal TotalRevenue,
    decimal TotalRevenueThisMonth,
    decimal TotalRevenueThisYear,
    decimal OutstandingPayments,
    int TotalPayments,
    int PaymentsThisMonth,
    IEnumerable<PaymentByMethodDto> PaymentsByMethod,
    IEnumerable<PaymentByTypeDto> PaymentsByType
);

public record PaymentByMethodDto(
    PaymentMethod Method,
    int Count,
    decimal TotalAmount
);

public record PaymentByTypeDto(
    PaymentType Type,
    int Count,
    decimal TotalAmount
);

// Mock Stripe/PayPal DTOs
public record CreatePaymentIntentRequest(
    [Required] Guid MemberId,
    [Required] decimal Amount,
    Guid? MembershipId = null,
    string Currency = "GBP",
    PaymentType Type = PaymentType.Membership,
    string? Description = null
);

public record CreatePaymentIntentResponse(
    string PaymentIntentId,
    string ClientSecret,
    decimal Amount,
    string Currency,
    string Status
);

public record PayPalOrderRequest(
    [Required] Guid MemberId,
    [Required] decimal Amount,
    [Required] string ReturnUrl,
    [Required] string CancelUrl,
    Guid? MembershipId = null,
    string Currency = "GBP",
    PaymentType Type = PaymentType.Membership,
    string? Description = null
);

public record PayPalOrderResponse(
    string OrderId,
    string ApprovalUrl,
    decimal Amount,
    string Currency,
    string Status
);

public record RefundRequest(
    decimal? Amount,
    string? Reason
);
