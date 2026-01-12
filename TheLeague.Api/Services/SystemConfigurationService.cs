using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Providers.Email;
using TheLeague.Api.Providers.Payment;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

/// <summary>
/// Service for managing system configuration with encryption support
/// </summary>
public class SystemConfigurationService : ISystemConfigurationService
{
    private readonly ApplicationDbContext _context;
    private readonly IDataProtector _protector;
    private readonly IPaymentProviderFactory _paymentProviderFactory;
    private readonly IEmailProviderFactory _emailProviderFactory;
    private readonly ILogger<SystemConfigurationService> _logger;

    private const string DataProtectionPurpose = "SystemConfiguration.Secrets";

    public SystemConfigurationService(
        ApplicationDbContext context,
        IDataProtectionProvider dataProtectionProvider,
        IPaymentProviderFactory paymentProviderFactory,
        IEmailProviderFactory emailProviderFactory,
        ILogger<SystemConfigurationService> logger)
    {
        _context = context;
        _protector = dataProtectionProvider.CreateProtector(DataProtectionPurpose);
        _paymentProviderFactory = paymentProviderFactory;
        _emailProviderFactory = emailProviderFactory;
        _logger = logger;
    }

    public async Task<SystemConfigurationDto> GetConfigurationAsync()
    {
        var config = await _context.SystemConfigurations.FirstOrDefaultAsync();
        if (config == null)
        {
            // Create default configuration if none exists
            config = new SystemConfiguration();
            _context.SystemConfigurations.Add(config);
            await _context.SaveChangesAsync();
        }

        return MapToDto(config);
    }

    public async Task<SystemConfigurationDto> UpdateConfigurationAsync(
        UpdateSystemConfigurationRequest request,
        string updatedBy,
        string? ipAddress)
    {
        var config = await _context.SystemConfigurations.FirstOrDefaultAsync();
        if (config == null)
        {
            config = new SystemConfiguration();
            _context.SystemConfigurations.Add(config);
        }

        var changes = new List<(string Property, string? OldValue, string? NewValue)>();

        // Update Payment Configuration
        if (request.PaymentProvider != null && request.PaymentProvider != config.PaymentProvider)
        {
            changes.Add(("PaymentProvider", config.PaymentProvider, request.PaymentProvider));
            config.PaymentProvider = request.PaymentProvider;
        }
        if (request.MockPaymentDelayMs.HasValue && request.MockPaymentDelayMs != config.MockPaymentDelayMs)
        {
            changes.Add(("MockPaymentDelayMs", config.MockPaymentDelayMs.ToString(), request.MockPaymentDelayMs.ToString()));
            config.MockPaymentDelayMs = request.MockPaymentDelayMs.Value;
        }
        if (request.MockPaymentFailureRate.HasValue && Math.Abs(request.MockPaymentFailureRate.Value - config.MockPaymentFailureRate) > 0.001)
        {
            changes.Add(("MockPaymentFailureRate", config.MockPaymentFailureRate.ToString("P"), request.MockPaymentFailureRate.Value.ToString("P")));
            config.MockPaymentFailureRate = request.MockPaymentFailureRate.Value;
        }
        if (request.StripePublishableKey != null && request.StripePublishableKey != config.StripePublishableKey)
        {
            changes.Add(("StripePublishableKey", MaskSecret(config.StripePublishableKey), MaskSecret(request.StripePublishableKey)));
            config.StripePublishableKey = request.StripePublishableKey;
        }
        if (!string.IsNullOrEmpty(request.StripeSecretKey))
        {
            changes.Add(("StripeSecretKey", "[REDACTED]", "[REDACTED]"));
            config.StripeSecretKeyEncrypted = EncryptSecret(request.StripeSecretKey);
        }
        if (!string.IsNullOrEmpty(request.StripeWebhookSecret))
        {
            changes.Add(("StripeWebhookSecret", "[REDACTED]", "[REDACTED]"));
            config.StripeWebhookSecretEncrypted = EncryptSecret(request.StripeWebhookSecret);
        }

        // Update Email Configuration
        if (request.EmailProvider != null && request.EmailProvider != config.EmailProvider)
        {
            changes.Add(("EmailProvider", config.EmailProvider, request.EmailProvider));
            config.EmailProvider = request.EmailProvider;
        }
        if (request.MockEmailDelayMs.HasValue && request.MockEmailDelayMs != config.MockEmailDelayMs)
        {
            changes.Add(("MockEmailDelayMs", config.MockEmailDelayMs.ToString(), request.MockEmailDelayMs.ToString()));
            config.MockEmailDelayMs = request.MockEmailDelayMs.Value;
        }
        if (!string.IsNullOrEmpty(request.SendGridApiKey))
        {
            changes.Add(("SendGridApiKey", "[REDACTED]", "[REDACTED]"));
            config.SendGridApiKeyEncrypted = EncryptSecret(request.SendGridApiKey);
        }
        if (request.DefaultFromEmail != null && request.DefaultFromEmail != config.DefaultFromEmail)
        {
            changes.Add(("DefaultFromEmail", config.DefaultFromEmail, request.DefaultFromEmail));
            config.DefaultFromEmail = request.DefaultFromEmail;
        }
        if (request.DefaultFromName != null && request.DefaultFromName != config.DefaultFromName)
        {
            changes.Add(("DefaultFromName", config.DefaultFromName, request.DefaultFromName));
            config.DefaultFromName = request.DefaultFromName;
        }

        // Update Feature Flags
        if (request.MaintenanceMode.HasValue && request.MaintenanceMode != config.MaintenanceMode)
        {
            changes.Add(("MaintenanceMode", config.MaintenanceMode.ToString(), request.MaintenanceMode.ToString()));
            config.MaintenanceMode = request.MaintenanceMode.Value;
        }
        if (request.MaintenanceMessage != null && request.MaintenanceMessage != config.MaintenanceMessage)
        {
            changes.Add(("MaintenanceMessage", config.MaintenanceMessage, request.MaintenanceMessage));
            config.MaintenanceMessage = request.MaintenanceMessage;
        }
        if (request.AllowNewRegistrations.HasValue && request.AllowNewRegistrations != config.AllowNewRegistrations)
        {
            changes.Add(("AllowNewRegistrations", config.AllowNewRegistrations.ToString(), request.AllowNewRegistrations.ToString()));
            config.AllowNewRegistrations = request.AllowNewRegistrations.Value;
        }
        if (request.EnableEmailNotifications.HasValue && request.EnableEmailNotifications != config.EnableEmailNotifications)
        {
            changes.Add(("EnableEmailNotifications", config.EnableEmailNotifications.ToString(), request.EnableEmailNotifications.ToString()));
            config.EnableEmailNotifications = request.EnableEmailNotifications.Value;
        }

        // Update Appearance
        if (request.PlatformName != null && request.PlatformName != config.PlatformName)
        {
            changes.Add(("PlatformName", config.PlatformName, request.PlatformName));
            config.PlatformName = request.PlatformName;
        }
        if (request.PrimaryColor != null && request.PrimaryColor != config.PrimaryColor)
        {
            changes.Add(("PrimaryColor", config.PrimaryColor, request.PrimaryColor));
            config.PrimaryColor = request.PrimaryColor;
        }
        if (request.LogoUrl != null && request.LogoUrl != config.LogoUrl)
        {
            changes.Add(("LogoUrl", config.LogoUrl, request.LogoUrl));
            config.LogoUrl = request.LogoUrl;
        }

        // Update audit fields
        config.LastModifiedAt = DateTime.UtcNow;
        config.LastModifiedBy = updatedBy;
        config.Version++;

        // Log all changes to audit log
        foreach (var change in changes)
        {
            var section = DetermineSection(change.Property);
            _context.ConfigurationAuditLogs.Add(new ConfigurationAuditLog
            {
                Id = Guid.NewGuid(),
                Action = "Updated",
                Section = section,
                PropertyChanged = change.Property,
                OldValue = change.OldValue,
                NewValue = change.NewValue,
                ChangedBy = updatedBy,
                Timestamp = DateTime.UtcNow,
                IpAddress = ipAddress
            });
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "System configuration updated by {User}. Changes: {ChangeCount}",
            updatedBy, changes.Count);

        return MapToDto(config);
    }

    public async Task<List<ConfigurationAuditLogDto>> GetAuditLogAsync(int? limit = null)
    {
        var query = _context.ConfigurationAuditLogs
            .OrderByDescending(l => l.Timestamp)
            .AsQueryable();

        if (limit.HasValue)
        {
            query = query.Take(limit.Value);
        }

        var logs = await query.ToListAsync();

        return logs.Select(l => new ConfigurationAuditLogDto(
            l.Id,
            l.Action,
            l.Section,
            l.PropertyChanged,
            l.OldValue,
            l.NewValue,
            l.ChangedBy,
            l.Timestamp,
            l.IpAddress
        )).ToList();
    }

    public async Task<ProviderTestResult> TestPaymentProviderAsync()
    {
        try
        {
            var provider = _paymentProviderFactory.GetProvider();
            var success = await provider.TestConnectionAsync();

            return success
                ? ProviderTestResult.Succeeded(provider.ProviderName, "Payment provider connection successful")
                : ProviderTestResult.Failed(provider.ProviderName, "Payment provider connection failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error testing payment provider");
            return ProviderTestResult.Failed(_paymentProviderFactory.CurrentProviderName, ex.Message);
        }
    }

    public async Task<ProviderTestResult> TestEmailProviderAsync()
    {
        try
        {
            var provider = _emailProviderFactory.GetProvider();
            var success = await provider.TestConnectionAsync();

            return success
                ? ProviderTestResult.Succeeded(provider.ProviderName, "Email provider connection successful")
                : ProviderTestResult.Failed(provider.ProviderName, "Email provider connection failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error testing email provider");
            return ProviderTestResult.Failed(_emailProviderFactory.CurrentProviderName, ex.Message);
        }
    }

    public async Task<ProviderTestResult> SendTestEmailAsync(string toEmail)
    {
        try
        {
            var provider = _emailProviderFactory.GetProvider();
            var config = await GetConfigurationAsync();

            var message = new EmailMessage(
                To: toEmail,
                Subject: $"Test Email from {config.PlatformName}",
                Body: $@"
                    <h1>Test Email</h1>
                    <p>This is a test email from <strong>{config.PlatformName}</strong>.</p>
                    <p>If you received this email, your email provider ({provider.ProviderName}) is configured correctly.</p>
                    <p><small>Sent at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</small></p>
                ",
                IsHtml: true
            );

            var result = await provider.SendEmailAsync(message);

            return result.Success
                ? ProviderTestResult.Succeeded(provider.ProviderName, $"Test email sent successfully to {toEmail}")
                : ProviderTestResult.Failed(provider.ProviderName, result.ErrorMessage ?? "Failed to send test email");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test email to {ToEmail}", toEmail);
            return ProviderTestResult.Failed(_emailProviderFactory.CurrentProviderName, ex.Message);
        }
    }

    private SystemConfigurationDto MapToDto(SystemConfiguration config)
    {
        return new SystemConfigurationDto
        {
            Id = config.Id,
            PaymentProvider = config.PaymentProvider,
            MockPaymentDelayMs = config.MockPaymentDelayMs,
            MockPaymentFailureRate = config.MockPaymentFailureRate,
            StripePublishableKey = config.StripePublishableKey,
            StripeSecretKeyMasked = MaskSecret(config.StripeSecretKeyEncrypted != null ? "[SET]" : null),
            StripeWebhookSecretMasked = MaskSecret(config.StripeWebhookSecretEncrypted != null ? "[SET]" : null),
            StripeConfigured = !string.IsNullOrEmpty(config.StripeSecretKeyEncrypted),
            EmailProvider = config.EmailProvider,
            MockEmailDelayMs = config.MockEmailDelayMs,
            SendGridApiKeyMasked = MaskSecret(config.SendGridApiKeyEncrypted != null ? "[SET]" : null),
            SendGridConfigured = !string.IsNullOrEmpty(config.SendGridApiKeyEncrypted),
            DefaultFromEmail = config.DefaultFromEmail,
            DefaultFromName = config.DefaultFromName,
            MaintenanceMode = config.MaintenanceMode,
            MaintenanceMessage = config.MaintenanceMessage,
            AllowNewRegistrations = config.AllowNewRegistrations,
            EnableEmailNotifications = config.EnableEmailNotifications,
            PlatformName = config.PlatformName,
            PrimaryColor = config.PrimaryColor,
            LogoUrl = config.LogoUrl,
            LastModifiedAt = config.LastModifiedAt,
            LastModifiedBy = config.LastModifiedBy,
            Version = config.Version
        };
    }

    private string EncryptSecret(string plainText)
    {
        return _protector.Protect(plainText);
    }

    private string DecryptSecret(string cipherText)
    {
        try
        {
            return _protector.Unprotect(cipherText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to decrypt secret");
            return string.Empty;
        }
    }

    private static string? MaskSecret(string? secret)
    {
        if (string.IsNullOrEmpty(secret)) return null;
        if (secret == "[SET]") return "****";
        if (secret.Length <= 8) return "****";
        return $"{secret[..4]}****{secret[^4..]}";
    }

    private static string DetermineSection(string property)
    {
        return property switch
        {
            var p when p.Contains("Payment") || p.Contains("Stripe") || p.Contains("Mock") && p.Contains("Payment") => "Payment",
            var p when p.Contains("Email") || p.Contains("SendGrid") || p.Contains("Mock") && p.Contains("Email") => "Email",
            var p when p.Contains("Maintenance") || p.Contains("Allow") || p.Contains("Enable") => "Features",
            var p when p.Contains("Platform") || p.Contains("Color") || p.Contains("Logo") => "Appearance",
            _ => "General"
        };
    }
}
