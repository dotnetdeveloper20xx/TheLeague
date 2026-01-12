namespace TheLeague.Api.DTOs;

/// <summary>
/// System configuration DTO - secrets are masked
/// </summary>
public record SystemConfigurationDto
{
    public Guid Id { get; init; }

    // Payment Configuration
    public string PaymentProvider { get; init; } = "Mock";
    public int MockPaymentDelayMs { get; init; } = 1500;
    public double MockPaymentFailureRate { get; init; } = 0.0;
    public string? StripePublishableKey { get; init; }
    public string? StripeSecretKeyMasked { get; init; }
    public string? StripeWebhookSecretMasked { get; init; }
    public bool StripeConfigured { get; init; }

    // Email Configuration
    public string EmailProvider { get; init; } = "Mock";
    public int MockEmailDelayMs { get; init; } = 500;
    public string? SendGridApiKeyMasked { get; init; }
    public bool SendGridConfigured { get; init; }
    public string DefaultFromEmail { get; init; } = "noreply@theleague.com";
    public string DefaultFromName { get; init; } = "The League";

    // Feature Flags
    public bool MaintenanceMode { get; init; }
    public string? MaintenanceMessage { get; init; }
    public bool AllowNewRegistrations { get; init; } = true;
    public bool EnableEmailNotifications { get; init; } = true;

    // Appearance
    public string PlatformName { get; init; } = "The League";
    public string PrimaryColor { get; init; } = "#6366f1";
    public string? LogoUrl { get; init; }

    // Audit
    public DateTime LastModifiedAt { get; init; }
    public string LastModifiedBy { get; init; } = "System";
    public int Version { get; init; }
}

/// <summary>
/// Request to update system configuration
/// </summary>
public record UpdateSystemConfigurationRequest
{
    // Payment Configuration
    public string? PaymentProvider { get; init; }
    public int? MockPaymentDelayMs { get; init; }
    public double? MockPaymentFailureRate { get; init; }
    public string? StripePublishableKey { get; init; }
    public string? StripeSecretKey { get; init; }
    public string? StripeWebhookSecret { get; init; }

    // Email Configuration
    public string? EmailProvider { get; init; }
    public int? MockEmailDelayMs { get; init; }
    public string? SendGridApiKey { get; init; }
    public string? DefaultFromEmail { get; init; }
    public string? DefaultFromName { get; init; }

    // Feature Flags
    public bool? MaintenanceMode { get; init; }
    public string? MaintenanceMessage { get; init; }
    public bool? AllowNewRegistrations { get; init; }
    public bool? EnableEmailNotifications { get; init; }

    // Appearance
    public string? PlatformName { get; init; }
    public string? PrimaryColor { get; init; }
    public string? LogoUrl { get; init; }
}

/// <summary>
/// Configuration audit log entry DTO
/// </summary>
public record ConfigurationAuditLogDto(
    Guid Id,
    string Action,
    string Section,
    string? PropertyChanged,
    string? OldValue,
    string? NewValue,
    string ChangedBy,
    DateTime Timestamp,
    string? IpAddress
);

/// <summary>
/// Result of testing a provider connection
/// </summary>
public record ProviderTestResult
{
    public bool Success { get; init; }
    public string Provider { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime TestedAt { get; init; } = DateTime.UtcNow;

    public static ProviderTestResult Succeeded(string provider, string message = "Connection successful") =>
        new() { Success = true, Provider = provider, Message = message };

    public static ProviderTestResult Failed(string provider, string message) =>
        new() { Success = false, Provider = provider, Message = message };
}

/// <summary>
/// Request to send a test email
/// </summary>
public record SendTestEmailRequest(string ToEmail);
