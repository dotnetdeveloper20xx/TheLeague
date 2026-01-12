using Microsoft.EntityFrameworkCore;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Providers.Email;

/// <summary>
/// Factory for creating email providers based on system configuration.
/// Reads the configuration at startup and returns the appropriate provider.
/// </summary>
public interface IEmailProviderFactory
{
    /// <summary>
    /// Gets the configured email provider
    /// </summary>
    IEmailProvider GetProvider();

    /// <summary>
    /// Gets the name of the currently configured provider
    /// </summary>
    string CurrentProviderName { get; }
}

public class EmailProviderFactory : IEmailProviderFactory
{
    private readonly IEmailProvider _provider;
    private readonly string _providerName;
    private readonly ILogger<EmailProviderFactory> _logger;

    public EmailProviderFactory(
        IServiceProvider serviceProvider,
        ILogger<EmailProviderFactory> logger)
    {
        _logger = logger;

        // Determine which provider to use based on configuration
        // This is evaluated at startup time
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var config = context.SystemConfigurations.FirstOrDefault();
        _providerName = config?.EmailProvider ?? "Mock";

        _logger.LogInformation("Initializing email provider: {Provider}", _providerName);

        // Create the appropriate provider
        _provider = _providerName.ToLowerInvariant() switch
        {
            "sendgrid" => new SendGridEmailProvider(
                scope.ServiceProvider.GetRequiredService<ILogger<SendGridEmailProvider>>(),
                serviceProvider),
            _ => new MockEmailProvider(
                scope.ServiceProvider.GetRequiredService<ILogger<MockEmailProvider>>(),
                serviceProvider)
        };

        _logger.LogInformation("Email provider initialized: {Provider}", _provider.ProviderName);
    }

    public IEmailProvider GetProvider() => _provider;

    public string CurrentProviderName => _providerName;
}
