using TheLeague.Api.DTOs;
using TheLeague.Core.Entities;

namespace TheLeague.Api.Services.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(Member member);
    Task SendPaymentReminderAsync(Member member, Membership membership);
    Task SendPaymentReceiptAsync(Member member, Payment payment);
    Task SendBookingConfirmationAsync(Member member, SessionBooking booking);
    Task SendBookingCancellationAsync(Member member, SessionBooking booking);
    Task SendPasswordResetEmailAsync(string email, string resetToken);
    Task SendEmailVerificationAsync(string email, string verificationToken);
    Task SendBulkEmailAsync(Guid clubId, IEnumerable<Guid> memberIds, string subject, string body);

    Task<PagedResult<EmailLogDto>> GetEmailHistoryAsync(Guid clubId, EmailFilterRequest filter);
    Task<IEnumerable<BulkEmailCampaignDto>> GetBulkCampaignsAsync(Guid clubId);
    Task<BulkEmailCampaignDto> CreateBulkCampaignAsync(Guid clubId, CreateCampaignRequest request, string createdBy);
    Task<bool> SendBulkCampaignAsync(Guid clubId, Guid campaignId);
}
