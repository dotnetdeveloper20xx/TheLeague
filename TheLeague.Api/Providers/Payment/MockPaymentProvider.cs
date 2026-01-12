using Microsoft.EntityFrameworkCore;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Providers.Payment;

/// <summary>
/// Mock payment provider for development and testing.
/// Simulates realistic delays and can optionally simulate failures.
/// </summary>
public class MockPaymentProvider : IPaymentProvider
{
    private readonly ILogger<MockPaymentProvider> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Random _random = new();

    // In-memory storage for payment intents (simulating Stripe's behavior)
    private static readonly Dictionary<string, MockPaymentIntent> _paymentIntents = new();

    public string ProviderName => "Mock";

    public MockPaymentProvider(
        ILogger<MockPaymentProvider> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<PaymentIntentResult> CreatePaymentIntentAsync(PaymentIntentRequest request)
    {
        var config = await GetConfigurationAsync();

        _logger.LogInformation(
            "[MOCK PAYMENT] Creating payment intent for {Amount} {Currency}...",
            request.Amount, request.Currency);

        // Simulate realistic delay
        await Task.Delay(config.MockPaymentDelayMs / 2);

        // Generate mock payment intent
        var paymentIntentId = $"pi_mock_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}"[..32];
        var clientSecret = $"{paymentIntentId}_secret_{Guid.NewGuid():N}"[..50];

        // Store for later processing
        _paymentIntents[paymentIntentId] = new MockPaymentIntent
        {
            Id = paymentIntentId,
            ClientSecret = clientSecret,
            Amount = request.Amount,
            Currency = request.Currency,
            ClubId = request.ClubId,
            MemberId = request.MemberId,
            MembershipId = request.MembershipId,
            InvoiceId = request.InvoiceId,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        _logger.LogInformation(
            "[MOCK PAYMENT] Payment intent created: {PaymentIntentId}",
            paymentIntentId);

        return PaymentIntentResult.Succeeded(paymentIntentId, clientSecret);
    }

    public async Task<PaymentResult> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        var config = await GetConfigurationAsync();

        _logger.LogInformation(
            "[MOCK PAYMENT] Processing payment {PaymentIntentId} for {Amount} {Currency}...",
            request.PaymentIntentId, request.Amount, request.Currency);

        // Simulate realistic delay (loading spinner in UI)
        await Task.Delay(config.MockPaymentDelayMs);

        // Simulate occasional failures if configured
        if (config.MockPaymentFailureRate > 0 && _random.NextDouble() < config.MockPaymentFailureRate)
        {
            _logger.LogWarning("[MOCK PAYMENT] Simulated payment failure");
            return PaymentResult.Failed("Card declined (simulated failure)", "card_declined");
        }

        // Generate mock transaction
        var transactionId = $"ch_mock_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}"[..32];
        var cardLast4 = request.CardNumber?.Length >= 4
            ? request.CardNumber[^4..]
            : "4242";
        var cardBrand = DetectCardBrand(request.CardNumber);

        _logger.LogInformation(
            "[MOCK PAYMENT] Payment successful - Transaction: {TransactionId}, Card: {CardBrand} ****{CardLast4}",
            transactionId, cardBrand, cardLast4);

        // Clean up the payment intent
        _paymentIntents.Remove(request.PaymentIntentId);

        return PaymentResult.Succeeded(
            transactionId: transactionId,
            cardLast4: cardLast4,
            cardBrand: cardBrand,
            receiptUrl: $"https://mock.theleague.com/receipts/{transactionId}"
        );
    }

    public async Task<RefundResult> ProcessRefundAsync(string transactionId, decimal amount)
    {
        var config = await GetConfigurationAsync();

        _logger.LogInformation(
            "[MOCK PAYMENT] Processing refund of {Amount} for transaction {TransactionId}...",
            amount, transactionId);

        // Simulate realistic delay
        await Task.Delay(config.MockPaymentDelayMs);

        // Generate mock refund
        var refundId = $"re_mock_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}"[..32];

        _logger.LogInformation(
            "[MOCK PAYMENT] Refund successful - RefundId: {RefundId}, Amount: {Amount}",
            refundId, amount);

        return RefundResult.Succeeded(refundId, amount);
    }

    public async Task<bool> TestConnectionAsync()
    {
        _logger.LogInformation("[MOCK PAYMENT] Testing connection...");
        await Task.Delay(500); // Simulate connection test
        _logger.LogInformation("[MOCK PAYMENT] Connection test successful");
        return true;
    }

    private async Task<Core.Entities.SystemConfiguration> GetConfigurationAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var config = await context.SystemConfigurations.FirstOrDefaultAsync();
        if (config == null)
        {
            // Return defaults if no configuration exists
            return new Core.Entities.SystemConfiguration();
        }
        return config;
    }

    private static string DetectCardBrand(string? cardNumber)
    {
        if (string.IsNullOrEmpty(cardNumber)) return "Card";

        return cardNumber[0] switch
        {
            '4' => "Visa",
            '5' => "Mastercard",
            '3' => "Amex",
            '6' => "Discover",
            _ => "Card"
        };
    }

    private class MockPaymentIntent
    {
        public string Id { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "GBP";
        public Guid ClubId { get; set; }
        public Guid MemberId { get; set; }
        public Guid? MembershipId { get; set; }
        public Guid? InvoiceId { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
