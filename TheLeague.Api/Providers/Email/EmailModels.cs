namespace TheLeague.Api.Providers.Email;

/// <summary>
/// Email message to be sent
/// </summary>
public record EmailMessage(
    string To,
    string Subject,
    string Body,
    bool IsHtml = true,
    string? FromEmail = null,
    string? FromName = null,
    string? ReplyTo = null,
    IEnumerable<EmailAttachment>? Attachments = null,
    Dictionary<string, string>? Metadata = null
);

/// <summary>
/// Email attachment
/// </summary>
public record EmailAttachment(
    string FileName,
    byte[] Content,
    string ContentType
);

/// <summary>
/// Result of sending an email
/// </summary>
public record EmailResult
{
    public bool Success { get; init; }
    public string? MessageId { get; init; }
    public int SuccessCount { get; init; }
    public int FailureCount { get; init; }
    public string? ErrorMessage { get; init; }
    public List<string> FailedRecipients { get; init; } = new();

    public static EmailResult Succeeded(string messageId) =>
        new() { Success = true, MessageId = messageId, SuccessCount = 1 };

    public static EmailResult BulkSucceeded(int successCount, int failureCount = 0, List<string>? failedRecipients = null) =>
        new()
        {
            Success = failureCount == 0,
            SuccessCount = successCount,
            FailureCount = failureCount,
            FailedRecipients = failedRecipients ?? new()
        };

    public static EmailResult Failed(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage, FailureCount = 1 };
}
