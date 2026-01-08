using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class ClubService : IClubService
{
    private readonly ApplicationDbContext _context;

    public ClubService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
    {
        var clubs = await _context.Clubs
            .Include(c => c.Settings)
            .Include(c => c.Members)
            .AsNoTracking()
            .ToListAsync();

        return clubs.Select(MapToDto);
    }

    public async Task<ClubDto?> GetClubByIdAsync(Guid id)
    {
        var club = await _context.Clubs
            .Include(c => c.Settings)
            .Include(c => c.Members)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return club == null ? null : MapToDto(club);
    }

    public async Task<ClubDto?> GetClubBySlugAsync(string slug)
    {
        var club = await _context.Clubs
            .Include(c => c.Settings)
            .Include(c => c.Members)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug);

        return club == null ? null : MapToDto(club);
    }

    public async Task<ClubDto> CreateClubAsync(ClubCreateRequest request)
    {
        var club = new Club
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug.ToLower(),
            Description = request.Description,
            LogoUrl = request.LogoUrl,
            PrimaryColor = request.PrimaryColor,
            SecondaryColor = request.SecondaryColor,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            Address = request.Address,
            Website = request.Website,
            ClubType = request.ClubType,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Settings = new ClubSettings { Id = Guid.NewGuid() }
        };

        club.Settings.ClubId = club.Id;
        _context.Clubs.Add(club);
        await _context.SaveChangesAsync();

        return MapToDto(club);
    }

    public async Task<ClubDto?> UpdateClubAsync(Guid id, ClubUpdateRequest request)
    {
        var club = await _context.Clubs
            .Include(c => c.Settings)
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (club == null) return null;

        if (request.Name != null) club.Name = request.Name;
        if (request.Description != null) club.Description = request.Description;
        if (request.LogoUrl != null) club.LogoUrl = request.LogoUrl;
        if (request.PrimaryColor != null) club.PrimaryColor = request.PrimaryColor;
        if (request.SecondaryColor != null) club.SecondaryColor = request.SecondaryColor;
        if (request.ContactEmail != null) club.ContactEmail = request.ContactEmail;
        if (request.ContactPhone != null) club.ContactPhone = request.ContactPhone;
        if (request.Address != null) club.Address = request.Address;
        if (request.Website != null) club.Website = request.Website;
        if (request.ClubType.HasValue) club.ClubType = request.ClubType.Value;
        if (request.IsActive.HasValue) club.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync();
        return MapToDto(club);
    }

    public async Task<bool> DeleteClubAsync(Guid id)
    {
        var club = await _context.Clubs.FindAsync(id);
        if (club == null) return false;

        club.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ClubSettingsDto?> GetClubSettingsAsync(Guid clubId)
    {
        var settings = await _context.ClubSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ClubId == clubId);

        return settings == null ? null : MapSettingsToDto(settings);
    }

    public async Task<ClubSettingsDto?> UpdateClubSettingsAsync(Guid clubId, ClubSettingsUpdateRequest request)
    {
        var settings = await _context.ClubSettings.FirstOrDefaultAsync(s => s.ClubId == clubId);
        if (settings == null) return null;

        if (request.AllowOnlineRegistration.HasValue) settings.AllowOnlineRegistration = request.AllowOnlineRegistration.Value;
        if (request.RequireEmergencyContact.HasValue) settings.RequireEmergencyContact = request.RequireEmergencyContact.Value;
        if (request.RequireMedicalInfo.HasValue) settings.RequireMedicalInfo = request.RequireMedicalInfo.Value;
        if (request.AllowFamilyAccounts.HasValue) settings.AllowFamilyAccounts = request.AllowFamilyAccounts.Value;
        if (request.AllowOnlinePayments.HasValue) settings.AllowOnlinePayments = request.AllowOnlinePayments.Value;
        if (request.AllowManualPayments.HasValue) settings.AllowManualPayments = request.AllowManualPayments.Value;
        if (request.AutoSendPaymentReminders.HasValue) settings.AutoSendPaymentReminders = request.AutoSendPaymentReminders.Value;
        if (request.PaymentReminderDaysBefore.HasValue) settings.PaymentReminderDaysBefore = request.PaymentReminderDaysBefore.Value;
        if (request.PaymentReminderFrequency.HasValue) settings.PaymentReminderFrequency = request.PaymentReminderFrequency.Value;
        if (request.AllowMemberBookings.HasValue) settings.AllowMemberBookings = request.AllowMemberBookings.Value;
        if (request.MaxAdvanceBookingDays.HasValue) settings.MaxAdvanceBookingDays = request.MaxAdvanceBookingDays.Value;
        if (request.CancellationNoticePeriodHours.HasValue) settings.CancellationNoticePeriodHours = request.CancellationNoticePeriodHours.Value;
        if (request.EnableWaitlist.HasValue) settings.EnableWaitlist = request.EnableWaitlist.Value;
        if (request.SendWelcomeEmail.HasValue) settings.SendWelcomeEmail = request.SendWelcomeEmail.Value;
        if (request.SendBookingConfirmations.HasValue) settings.SendBookingConfirmations = request.SendBookingConfirmations.Value;
        if (request.SendPaymentReceipts.HasValue) settings.SendPaymentReceipts = request.SendPaymentReceipts.Value;

        await _context.SaveChangesAsync();
        return MapSettingsToDto(settings);
    }

    public async Task<ClubDashboardDto> GetClubDashboardAsync(Guid clubId)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfYear = new DateTime(now.Year, 1, 1);

        // Get member stats
        var members = await _context.Members.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .ToListAsync();

        var totalMembers = members.Count;
        var activeMembers = members.Count(m => m.Status == MemberStatus.Active);
        var newMembersThisMonth = members.Count(m => m.JoinedDate >= startOfMonth);

        // Get membership stats
        var memberships = await _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .Include(m => m.MembershipType)
            .ToListAsync();

        var expiredMemberships = memberships.Count(m => m.Status == MembershipStatus.Expired);

        // Get payment stats
        var payments = await _context.Payments.IgnoreQueryFilters()
            .Where(p => p.ClubId == clubId && p.Status == PaymentStatus.Completed)
            .ToListAsync();

        var totalRevenueThisMonth = payments.Where(p => p.PaymentDate >= startOfMonth).Sum(p => p.Amount);
        var totalRevenueThisYear = payments.Where(p => p.PaymentDate >= startOfYear).Sum(p => p.Amount);
        var outstandingPayments = await _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId && m.AmountDue > 0)
            .SumAsync(m => m.AmountDue);

        // Get session stats
        var upcomingSessions = await _context.Sessions.IgnoreQueryFilters()
            .Where(s => s.ClubId == clubId && s.StartTime > now && !s.IsCancelled)
            .CountAsync();

        var upcomingEvents = await _context.Events.IgnoreQueryFilters()
            .Where(e => e.ClubId == clubId && e.StartDateTime > now && !e.IsCancelled)
            .CountAsync();

        // Calculate attendance rate
        var recentBookings = await _context.SessionBookings.IgnoreQueryFilters()
            .Where(b => b.ClubId == clubId && b.Session.StartTime >= startOfMonth && b.Session.StartTime <= now)
            .ToListAsync();

        var attendanceRate = recentBookings.Count > 0
            ? (decimal)recentBookings.Count(b => b.Attended) / recentBookings.Count * 100
            : 0;

        // Membership breakdown
        var membershipBreakdown = memberships
            .Where(m => m.Status == MembershipStatus.Active)
            .GroupBy(m => m.MembershipType?.Name ?? "Unknown")
            .Select(g => new MembershipBreakdownDto(
                g.Key,
                g.Count(),
                totalMembers > 0 ? (decimal)g.Count() / activeMembers * 100 : 0
            ))
            .ToList();

        // Revenue by month (last 12 months)
        var revenueByMonth = payments
            .Where(p => p.PaymentDate >= now.AddMonths(-12))
            .GroupBy(p => p.PaymentDate.ToString("MMM yyyy"))
            .Select(g => new RevenueByMonthDto(g.Key, g.Sum(p => p.Amount)))
            .ToList();

        // Recent members
        var recentMembers = members
            .OrderByDescending(m => m.JoinedDate)
            .Take(5)
            .Select(m =>
            {
                var membership = memberships.FirstOrDefault(ms => ms.MemberId == m.Id && ms.Status == MembershipStatus.Active);
                return new RecentMemberDto(
                    m.Id,
                    m.FullName,
                    m.Email,
                    m.JoinedDate,
                    membership?.MembershipType?.Name ?? "None"
                );
            })
            .ToList();

        // Upcoming sessions list
        var upcomingSessionsList = await _context.Sessions.IgnoreQueryFilters()
            .Where(s => s.ClubId == clubId && s.StartTime > now && !s.IsCancelled)
            .OrderBy(s => s.StartTime)
            .Take(5)
            .Select(s => new UpcomingSessionDto(s.Id, s.Title, s.StartTime, s.CurrentBookings, s.Capacity))
            .ToListAsync();

        return new ClubDashboardDto(
            totalMembers,
            activeMembers,
            newMembersThisMonth,
            expiredMemberships,
            totalRevenueThisMonth,
            totalRevenueThisYear,
            outstandingPayments,
            upcomingSessions,
            upcomingEvents,
            attendanceRate,
            membershipBreakdown,
            revenueByMonth,
            recentMembers,
            upcomingSessionsList
        );
    }

    private static ClubDto MapToDto(Club club) => new(
        club.Id,
        club.Name,
        club.Slug,
        club.Description,
        club.LogoUrl,
        club.PrimaryColor,
        club.SecondaryColor,
        club.ContactEmail,
        club.ContactPhone,
        club.Address,
        club.Website,
        club.ClubType,
        club.IsActive,
        club.CreatedAt,
        club.RenewalDate,
        club.Members?.Count ?? 0,
        club.Settings == null ? null : MapSettingsToDto(club.Settings)
    );

    private static ClubSettingsDto MapSettingsToDto(ClubSettings s) => new(
        s.Id,
        s.AllowOnlineRegistration,
        s.RequireEmergencyContact,
        s.RequireMedicalInfo,
        s.AllowFamilyAccounts,
        s.AllowOnlinePayments,
        s.AllowManualPayments,
        s.AutoSendPaymentReminders,
        s.PaymentReminderDaysBefore,
        s.PaymentReminderFrequency,
        s.AllowMemberBookings,
        s.MaxAdvanceBookingDays,
        s.CancellationNoticePeriodHours,
        s.EnableWaitlist,
        s.SendWelcomeEmail,
        s.SendBookingConfirmations,
        s.SendPaymentReceipts
    );
}
