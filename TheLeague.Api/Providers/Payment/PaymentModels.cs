namespace TheLeague.Api.Providers.Payment;

/// <summary>
/// Request to create a payment intent
/// </summary>
public record PaymentIntentRequest(
    decimal Amount,
    string Currency,
    Guid ClubId,
    Guid MemberId,
    Guid? MembershipId = null,
    Guid? InvoiceId = null,
    string? Description = null
);

/// <summary>
/// Result of creating a payment intent
/// </summary>
public record PaymentIntentResult
{
    public bool Success { get; init; }
    public string? PaymentIntentId { get; init; }
    public string? ClientSecret { get; init; }
    public string? ErrorMessage { get; init; }

    public static PaymentIntentResult Succeeded(string paymentIntentId, string clientSecret) =>
        new() { Success = true, PaymentIntentId = paymentIntentId, ClientSecret = clientSecret };

    public static PaymentIntentResult Failed(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };
}

/// <summary>
/// Request to process a payment
/// </summary>
public record ProcessPaymentRequest(
    string PaymentIntentId,
    decimal Amount,
    string Currency,
    Guid ClubId,
    Guid MemberId,
    string? CardNumber = null,
    string? CardExpiry = null,
    string? CardCvc = null,
    string? CardholderName = null,
    Guid? MembershipId = null,
    Guid? InvoiceId = null,
    string? Description = null
);

/// <summary>
/// Result of processing a payment
/// </summary>
public record PaymentResult
{
    public bool Success { get; init; }
    public string? TransactionId { get; init; }
    public string? CardLast4 { get; init; }
    public string? CardBrand { get; init; }
    public string? ReceiptUrl { get; init; }
    public string? ErrorMessage { get; init; }
    public string? ErrorCode { get; init; }

    public static PaymentResult Succeeded(
        string transactionId,
        string? cardLast4 = null,
        string? cardBrand = null,
        string? receiptUrl = null) =>
        new()
        {
            Success = true,
            TransactionId = transactionId,
            CardLast4 = cardLast4,
            CardBrand = cardBrand,
            ReceiptUrl = receiptUrl
        };

    public static PaymentResult Failed(string errorMessage, string? errorCode = null) =>
        new()
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = errorCode
        };
}

/// <summary>
/// Result of processing a refund
/// </summary>
public record RefundResult
{
    public bool Success { get; init; }
    public string? RefundId { get; init; }
    public decimal AmountRefunded { get; init; }
    public string? ErrorMessage { get; init; }

    public static RefundResult Succeeded(string refundId, decimal amountRefunded) =>
        new() { Success = true, RefundId = refundId, AmountRefunded = amountRefunded };

    public static RefundResult Failed(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };
}
