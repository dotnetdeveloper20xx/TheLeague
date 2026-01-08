using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class EmailService : IEmailService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmailService> _logger;

    public EmailService(ApplicationDbContext context, ILogger<EmailService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SendWelcomeEmailAsync(Member member)
    {
        await LogEmailAsync(member.ClubId, member.Id, member.Email, "Welcome to our Club!",
            $"Dear {member.FirstName},\n\nWelcome to our club! We're excited to have you as a member.",
            EmailType.Welcome);
    }

    public async Task SendPaymentReminderAsync(Member member, Membership membership)
    {
        await LogEmailAsync(member.ClubId, member.Id, member.Email, "Payment Reminder",
            $"Dear {member.FirstName},\n\nThis is a reminder that your membership payment of £{membership.AmountDue} is due.",
            EmailType.PaymentReminder);
    }

    public async Task SendPaymentReceiptAsync(Member member, Payment payment)
    {
        await LogEmailAsync(member.ClubId, member.Id, member.Email, $"Payment Receipt - {payment.ReceiptNumber}",
            $"Dear {member.FirstName},\n\nThank you for your payment of £{payment.Amount}.\nReceipt Number: {payment.ReceiptNumber}",
            EmailType.PaymentReceipt);
    }

    public async Task SendBookingConfirmationAsync(Member member, SessionBooking booking)
    {
        await LogEmailAsync(member.ClubId, member.Id, member.Email, "Booking Confirmation",
            $"Dear {member.FirstName},\n\nYour booking has been confirmed.",
            EmailType.BookingConfirmation);
    }

    public async Task SendBookingCancellationAsync(Member member, SessionBooking booking)
    {
        await LogEmailAsync(member.ClubId, member.Id, member.Email, "Booking Cancelled",
            $"Dear {member.FirstName},\n\nYour booking has been cancelled.",
            EmailType.BookingCancellation);
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        _logger.LogInformation("Mock: Sending password reset email to {Email} with token {Token}", email, resetToken);
        // In a real implementation, this would send an actual email
    }

    public async Task SendEmailVerificationAsync(string email, string verificationToken)
    {
        _logger.LogInformation("Mock: Sending email verification to {Email} with token {Token}", email, verificationToken);
        // In a real implementation, this would send an actual email
    }

    public async Task SendBulkEmailAsync(Guid clubId, IEnumerable<Guid> memberIds, string subject, string body)
    {
        var members = await _context.Members.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId && memberIds.Contains(m.Id))
            .ToListAsync();

        foreach (var member in members)
        {
            await LogEmailAsync(clubId, member.Id, member.Email, subject, body, EmailType.BulkCommunication);
        }
    }

    public async Task<PagedResult<EmailLogDto>> GetEmailHistoryAsync(Guid clubId, EmailFilterRequest filter)
    {
        var query = _context.EmailLogs.IgnoreQueryFilters()
            .Where(e => e.ClubId == clubId)
            .Include(e => e.Member)
            .AsQueryable();

        if (filter.MemberId.HasValue)
            query = query.Where(e => e.MemberId == filter.MemberId.Value);
        if (filter.Type.HasValue)
            query = query.Where(e => e.Type == filter.Type.Value);
        if (filter.Status.HasValue)
            query = query.Where(e => e.Status == filter.Status.Value);
        if (filter.DateFrom.HasValue)
            query = query.Where(e => e.SentAt >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(e => e.SentAt <= filter.DateTo.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(e => e.SentAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(e => new EmailLogDto(
                e.Id,
                e.MemberId,
                e.Member != null ? e.Member.FirstName + " " + e.Member.LastName : null,
                e.ToEmail,
                e.Subject,
                e.Body,
                e.Type,
                e.Status,
                e.SentAt,
                e.ErrorMessage
            ))
            .ToListAsync();

        return new PagedResult<EmailLogDto>(
            items,
            totalCount,
            filter.Page,
            filter.PageSize,
            (int)Math.Ceiling(totalCount / (double)filter.PageSize)
        );
    }

    public async Task<IEnumerable<BulkEmailCampaignDto>> GetBulkCampaignsAsync(Guid clubId)
    {
        var campaigns = await _context.BulkEmailCampaigns.IgnoreQueryFilters()
            .Where(c => c.ClubId == clubId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return campaigns.Select(c => new BulkEmailCampaignDto(
            c.Id, c.Subject, c.Body, c.RecipientFilter, c.TotalRecipients,
            c.SentCount, c.FailedCount, c.CreatedAt, c.SentAt, c.CreatedBy, c.Status
        ));
    }

    public async Task<BulkEmailCampaignDto> CreateBulkCampaignAsync(Guid clubId, CreateCampaignRequest request, string createdBy)
    {
        var campaign = new BulkEmailCampaign
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Subject = request.Subject,
            Body = request.Body,
            RecipientFilter = System.Text.Json.JsonSerializer.Serialize(request.RecipientFilter),
            Status = CampaignStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        _context.BulkEmailCampaigns.Add(campaign);
        await _context.SaveChangesAsync();

        return new BulkEmailCampaignDto(
            campaign.Id, campaign.Subject, campaign.Body, campaign.RecipientFilter,
            campaign.TotalRecipients, campaign.SentCount, campaign.FailedCount,
            campaign.CreatedAt, campaign.SentAt, campaign.CreatedBy, campaign.Status
        );
    }

    public async Task<bool> SendBulkCampaignAsync(Guid clubId, Guid campaignId)
    {
        var campaign = await _context.BulkEmailCampaigns.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.ClubId == clubId && c.Id == campaignId);

        if (campaign == null) return false;

        // Get all members for the club (in real implementation, would apply filter)
        var members = await _context.Members.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId && m.IsActive)
            .ToListAsync();

        campaign.TotalRecipients = members.Count;
        campaign.Status = CampaignStatus.Sending;

        foreach (var member in members)
        {
            try
            {
                await LogEmailAsync(clubId, member.Id, member.Email, campaign.Subject, campaign.Body, EmailType.BulkCommunication);
                campaign.SentCount++;
            }
            catch
            {
                campaign.FailedCount++;
            }
        }

        campaign.Status = CampaignStatus.Completed;
        campaign.SentAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task LogEmailAsync(Guid clubId, Guid? memberId, string toEmail, string subject, string body, EmailType type)
    {
        var log = new EmailLog
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            MemberId = memberId,
            ToEmail = toEmail,
            Subject = subject,
            Body = body,
            Type = type,
            Status = EmailStatus.Sent,
            SentAt = DateTime.UtcNow,
            SendGridMessageId = $"mock_{Guid.NewGuid()}"
        };

        _context.EmailLogs.Add(log);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Mock Email Sent - To: {Email}, Subject: {Subject}", toEmail, subject);
    }
}
