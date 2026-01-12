using Microsoft.EntityFrameworkCore;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Providers.Email;

/// <summary>
/// Mock email provider for development and testing.
/// Logs emails to console and database instead of actually sending them.
/// </summary>
public class MockEmailProvider : IEmailProvider
{
    private readonly ILogger<MockEmailProvider> _logger;
    private readonly IServiceProvider _serviceProvider;

    public string ProviderName => "Mock";

    public MockEmailProvider(
        ILogger<MockEmailProvider> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<EmailResult> SendEmailAsync(EmailMessage message)
    {
        var config = await GetConfigurationAsync();

        _logger.LogInformation("[MOCK EMAIL] Sending email to {To}...", message.To);

        // Simulate sending delay
        await Task.Delay(config.MockEmailDelayMs);

        // Generate mock message ID
        var messageId = $"mock_msg_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}"[..40];

        // Log detailed email info
        var bodyPreview = message.Body.Length > 100
            ? message.Body[..100] + "..."
            : message.Body;

        _logger.LogInformation(
            @"[MOCK EMAIL] Email sent successfully
  MessageId: {MessageId}
  To: {To}
  From: {From}
  Subject: {Subject}
  Body Preview: {BodyPreview}
  IsHtml: {IsHtml}
  Attachments: {AttachmentCount}",
            messageId,
            message.To,
            $"{message.FromName ?? config.DefaultFromName} <{message.FromEmail ?? config.DefaultFromEmail}>",
            message.Subject,
            bodyPreview,
            message.IsHtml,
            message.Attachments?.Count() ?? 0);

        return EmailResult.Succeeded(messageId);
    }

    public async Task<EmailResult> SendBulkEmailAsync(IEnumerable<EmailMessage> messages)
    {
        var config = await GetConfigurationAsync();
        var messageList = messages.ToList();

        _logger.LogInformation("[MOCK EMAIL] Sending bulk email to {Count} recipients...", messageList.Count);

        var successCount = 0;
        var failureCount = 0;
        var failedRecipients = new List<string>();

        foreach (var message in messageList)
        {
            // Simulate per-message delay (shorter for bulk)
            await Task.Delay(config.MockEmailDelayMs / 4);

            // Simulate occasional failures (1% for bulk)
            if (new Random().NextDouble() < 0.01)
            {
                failureCount++;
                failedRecipients.Add(message.To);
                _logger.LogWarning("[MOCK EMAIL] Failed to send to {To} (simulated failure)", message.To);
            }
            else
            {
                successCount++;
                _logger.LogDebug("[MOCK EMAIL] Sent to {To}", message.To);
            }
        }

        _logger.LogInformation(
            "[MOCK EMAIL] Bulk email complete - Success: {SuccessCount}, Failed: {FailureCount}",
            successCount, failureCount);

        return EmailResult.BulkSucceeded(successCount, failureCount, failedRecipients);
    }

    public async Task<bool> TestConnectionAsync()
    {
        _logger.LogInformation("[MOCK EMAIL] Testing connection...");
        await Task.Delay(300); // Simulate connection test
        _logger.LogInformation("[MOCK EMAIL] Connection test successful");
        return true;
    }

    private async Task<Core.Entities.SystemConfiguration> GetConfigurationAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var config = await context.SystemConfigurations.FirstOrDefaultAsync();
        return config ?? new Core.Entities.SystemConfiguration();
    }
}
