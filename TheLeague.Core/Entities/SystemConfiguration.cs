namespace TheLeague.Core.Entities;

/// <summary>
/// Global system configuration stored in database.
/// Only one record exists - singleton pattern.
/// </summary>
public class SystemConfiguration
{
    public Guid Id { get; set; }

    // Payment Configuration
    public string PaymentProvider { get; set; } = "Mock";  // Mock, Stripe
    public int MockPaymentDelayMs { get; set; } = 1500;
    public double MockPaymentFailureRate { get; set; } = 0.0;
    public string? StripePublishableKey { get; set; }
    public string? StripeSecretKeyEncrypted { get; set; }
    public string? StripeWebhookSecretEncrypted { get; set; }

    // Email Configuration
    public string EmailProvider { get; set; } = "Mock";  // Mock, SendGrid
    public int MockEmailDelayMs { get; set; } = 500;
    public string? SendGridApiKeyEncrypted { get; set; }
    public string DefaultFromEmail { get; set; } = "noreply@theleague.com";
    public string DefaultFromName { get; set; } = "The League";

    // Feature Flags
    public bool MaintenanceMode { get; set; } = false;
    public string? MaintenanceMessage { get; set; }
    public bool AllowNewRegistrations { get; set; } = true;
    public bool EnableEmailNotifications { get; set; } = true;

    // Appearance
    public string PlatformName { get; set; } = "The League";
    public string PrimaryColor { get; set; } = "#6366f1";
    public string? LogoUrl { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
    public string LastModifiedBy { get; set; } = "System";
    public int Version { get; set; } = 1;  // Optimistic concurrency
}
