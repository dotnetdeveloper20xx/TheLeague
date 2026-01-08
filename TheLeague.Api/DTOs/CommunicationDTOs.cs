using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record EmailLogDto(
    Guid Id,
    Guid? MemberId,
    string? MemberName,
    string ToEmail,
    string Subject,
    string Body,
    EmailType Type,
    EmailStatus Status,
    DateTime SentAt,
    string? ErrorMessage
);

public record SendEmailRequest(
    [Required] IEnumerable<Guid> MemberIds,
    [Required] string Subject,
    [Required] string Body
);

public record BulkEmailCampaignDto(
    Guid Id,
    string Subject,
    string Body,
    string? RecipientFilter,
    int TotalRecipients,
    int SentCount,
    int FailedCount,
    DateTime CreatedAt,
    DateTime? SentAt,
    string? CreatedBy,
    CampaignStatus Status
);

public record CreateCampaignRequest(
    [Required] string Subject,
    [Required] string Body,
    MemberFilterRequest? RecipientFilter
);

public record EmailFilterRequest(
    Guid? MemberId,
    EmailType? Type,
    EmailStatus? Status,
    DateTime? DateFrom,
    DateTime? DateTo,
    int Page = 1,
    int PageSize = 20
);
