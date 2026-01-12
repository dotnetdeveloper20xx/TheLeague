namespace TheLeague.Api.Providers.Email;

/// <summary>
/// Abstraction for email sending providers.
/// Implementations include Mock (development) and SendGrid (production).
/// </summary>
public interface IEmailProvider
{
    /// <summary>
    /// Name of the email provider (e.g., "Mock", "SendGrid")
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Sends a single email
    /// </summary>
    Task<EmailResult> SendEmailAsync(EmailMessage message);

    /// <summary>
    /// Sends multiple emails (bulk send)
    /// </summary>
    Task<EmailResult> SendBulkEmailAsync(IEnumerable<EmailMessage> messages);

    /// <summary>
    /// Tests the connection to the email provider
    /// </summary>
    Task<bool> TestConnectionAsync();
}
