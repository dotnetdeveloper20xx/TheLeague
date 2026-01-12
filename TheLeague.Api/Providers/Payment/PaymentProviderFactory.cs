using Microsoft.EntityFrameworkCore;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Providers.Payment;

/// <summary>
/// Factory for creating payment providers based on system configuration.
/// Reads the configuration at startup and returns the appropriate provider.
/// </summary>
public interface IPaymentProviderFactory
{
    /// <summary>
    /// Gets the configured payment provider
    /// </summary>
    IPaymentProvider GetProvider();

    /// <summary>
    /// Gets the name of the currently configured provider
    /// </summary>
    string CurrentProviderName { get; }
}

public class PaymentProviderFactory : IPaymentProviderFactory
{
    private readonly IPaymentProvider _provider;
    private readonly string _providerName;
    private readonly ILogger<PaymentProviderFactory> _logger;

    public PaymentProviderFactory(
        IServiceProvider serviceProvider,
        ILogger<PaymentProviderFactory> logger)
    {
        _logger = logger;

        // Determine which provider to use based on configuration
        // This is evaluated at startup time
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var config = context.SystemConfigurations.FirstOrDefault();
        _providerName = config?.PaymentProvider ?? "Mock";

        _logger.LogInformation("Initializing payment provider: {Provider}", _providerName);

        // Create the appropriate provider
        _provider = _providerName.ToLowerInvariant() switch
        {
            "stripe" => new StripePaymentProvider(
                scope.ServiceProvider.GetRequiredService<ILogger<StripePaymentProvider>>(),
                serviceProvider),
            _ => new MockPaymentProvider(
                scope.ServiceProvider.GetRequiredService<ILogger<MockPaymentProvider>>(),
                serviceProvider)
        };

        _logger.LogInformation("Payment provider initialized: {Provider}", _provider.ProviderName);
    }

    public IPaymentProvider GetProvider() => _provider;

    public string CurrentProviderName => _providerName;
}
