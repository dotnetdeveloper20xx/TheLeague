namespace TheLeague.Api.Providers.Payment;

/// <summary>
/// Abstraction for payment processing providers.
/// Implementations include Mock (development) and Stripe (production).
/// </summary>
public interface IPaymentProvider
{
    /// <summary>
    /// Name of the payment provider (e.g., "Mock", "Stripe")
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Creates a payment intent for card payment
    /// </summary>
    Task<PaymentIntentResult> CreatePaymentIntentAsync(PaymentIntentRequest request);

    /// <summary>
    /// Processes a payment using the payment intent
    /// </summary>
    Task<PaymentResult> ProcessPaymentAsync(ProcessPaymentRequest request);

    /// <summary>
    /// Processes a refund for a previous payment
    /// </summary>
    Task<RefundResult> ProcessRefundAsync(string transactionId, decimal amount);

    /// <summary>
    /// Tests the connection to the payment provider
    /// </summary>
    Task<bool> TestConnectionAsync();
}
