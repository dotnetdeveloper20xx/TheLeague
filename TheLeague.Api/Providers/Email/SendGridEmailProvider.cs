using Microsoft.EntityFrameworkCore;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Providers.Email;

/// <summary>
/// SendGrid email provider for production use.
/// This is a stub implementation - real SendGrid SDK integration would go here.
/// </summary>
public class SendGridEmailProvider : IEmailProvider
{
    private readonly ILogger<SendGridEmailProvider> _logger;
    private readonly IServiceProvider _serviceProvider;

    public string ProviderName => "SendGrid";

    public SendGridEmailProvider(
        ILogger<SendGridEmailProvider> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<EmailResult> SendEmailAsync(EmailMessage message)
    {
        var config = await GetConfigurationAsync();

        // Validate SendGrid is configured
        if (string.IsNullOrEmpty(config.SendGridApiKeyEncrypted))
        {
            _logger.LogError("[SENDGRID] SendGrid API key not configured");
            return EmailResult.Failed("SendGrid is not configured. Please set up SendGrid API key in System Configuration.");
        }

        _logger.LogInformation("[SENDGRID] Sending email to {To}...", message.To);

        // TODO: Implement real SendGrid SDK integration
        // var client = new SendGridClient(decryptedApiKey);
        // var from = new EmailAddress(message.FromEmail ?? config.DefaultFromEmail, message.FromName ?? config.DefaultFromName);
        // var to = new EmailAddress(message.To);
        // var msg = MailHelper.CreateSingleEmail(from, to, message.Subject,
        //     message.IsHtml ? null : message.Body,
        //     message.IsHtml ? message.Body : null);
        //
        // if (message.Attachments != null)
        // {
        //     foreach (var attachment in message.Attachments)
        //     {
        //         msg.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.ContentType);
        //     }
        // }
        //
        // var response = await client.SendEmailAsync(msg);
        // if (response.IsSuccessStatusCode)
        // {
        //     return EmailResult.Succeeded(response.Headers.GetValues("X-Message-Id").FirstOrDefault() ?? Guid.NewGuid().ToString());
        // }
        // return EmailResult.Failed($"SendGrid error: {response.StatusCode}");

        return EmailResult.Failed("SendGrid integration not yet implemented. Please use Mock provider for development.");
    }

    public async Task<EmailResult> SendBulkEmailAsync(IEnumerable<EmailMessage> messages)
    {
        var config = await GetConfigurationAsync();

        if (string.IsNullOrEmpty(config.SendGridApiKeyEncrypted))
        {
            return EmailResult.Failed("SendGrid is not configured.");
        }

        _logger.LogInformation("[SENDGRID] Sending bulk email to {Count} recipients...", messages.Count());

        // TODO: Implement real SendGrid SDK bulk send
        // Uses SendGrid's personalization feature for efficient bulk sending

        return EmailResult.Failed("SendGrid integration not yet implemented.");
    }

    public async Task<bool> TestConnectionAsync()
    {
        var config = await GetConfigurationAsync();

        if (string.IsNullOrEmpty(config.SendGridApiKeyEncrypted))
        {
            _logger.LogWarning("[SENDGRID] Cannot test connection - SendGrid not configured");
            return false;
        }

        _logger.LogInformation("[SENDGRID] Testing connection...");

        // TODO: Implement real SendGrid SDK connection test
        // try
        // {
        //     var client = new SendGridClient(decryptedApiKey);
        //     // Make a simple API call to verify the key works
        //     return true;
        // }
        // catch (Exception ex)
        // {
        //     _logger.LogError(ex, "[SENDGRID] Connection test failed");
        //     return false;
        // }

        return false;
    }

    private async Task<Core.Entities.SystemConfiguration> GetConfigurationAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var config = await context.SystemConfigurations.FirstOrDefaultAsync();
        return config ?? new Core.Entities.SystemConfiguration();
    }
}
