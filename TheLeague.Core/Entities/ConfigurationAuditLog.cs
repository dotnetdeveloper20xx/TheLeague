namespace TheLeague.Core.Entities;

/// <summary>
/// Audit trail for system configuration changes.
/// Records who changed what and when.
/// </summary>
public class ConfigurationAuditLog
{
    public Guid Id { get; set; }

    /// <summary>
    /// Type of action performed: "Created", "Updated", "Viewed"
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Configuration section: "Payment", "Email", "Features", "Appearance"
    /// </summary>
    public string Section { get; set; } = string.Empty;

    /// <summary>
    /// Name of the property that was changed (null for bulk updates)
    /// </summary>
    public string? PropertyChanged { get; set; }

    /// <summary>
    /// Previous value (masked for secrets, e.g., "[REDACTED]")
    /// </summary>
    public string? OldValue { get; set; }

    /// <summary>
    /// New value (masked for secrets, e.g., "[REDACTED]")
    /// </summary>
    public string? NewValue { get; set; }

    /// <summary>
    /// User who made the change (email or username)
    /// </summary>
    public string ChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// When the change was made
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// IP address of the user who made the change
    /// </summary>
    public string? IpAddress { get; set; }
}
