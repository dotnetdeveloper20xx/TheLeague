using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Enums;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MembershipStatsDto> GetMembershipStatsAsync(Guid clubId)
    {
        var members = await _context.Members.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .ToListAsync();

        var memberships = await _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .Include(m => m.MembershipType)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfYear = new DateTime(now.Year, 1, 1);

        var totalMembers = members.Count;
        var activeMembers = members.Count(m => m.Status == MemberStatus.Active);
        var pendingMembers = members.Count(m => m.Status == MemberStatus.Pending);
        var expiredMembers = members.Count(m => m.Status == MemberStatus.Expired);
        var suspendedMembers = members.Count(m => m.Status == MemberStatus.Suspended);
        var cancelledMembers = members.Count(m => m.Status == MemberStatus.Cancelled);
        var newMembersThisMonth = members.Count(m => m.JoinedDate >= startOfMonth);
        var newMembersThisYear = members.Count(m => m.JoinedDate >= startOfYear);

        var retentionRate = totalMembers > 0 ? (double)activeMembers / totalMembers * 100 : 0;

        var byMembershipType = memberships
            .Where(m => m.Status == MembershipStatus.Active)
            .GroupBy(m => new { m.MembershipTypeId, Name = m.MembershipType?.Name ?? "Unknown" })
            .Select(g => new MembershipTypeStatsDto(
                g.Key.MembershipTypeId,
                g.Key.Name,
                g.Count(),
                g.Count(),
                g.Sum(m => m.AmountPaid)
            ))
            .ToList();

        var monthlyTrend = Enumerable.Range(0, 12)
            .Select(i =>
            {
                var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
                var monthEnd = monthStart.AddMonths(1);
                var newMembers = members.Count(m => m.JoinedDate >= monthStart && m.JoinedDate < monthEnd);
                var churned = memberships.Count(m => m.Status == MembershipStatus.Cancelled && m.EndDate >= monthStart && m.EndDate < monthEnd);
                var active = members.Count(m => m.JoinedDate < monthEnd && m.Status == MemberStatus.Active);
                return new MonthlyMemberStatsDto(monthStart.ToString("MMM yyyy"), newMembers, churned, active);
            })
            .Reverse()
            .ToList();

        return new MembershipStatsDto(
            totalMembers, activeMembers, pendingMembers, expiredMembers,
            suspendedMembers, cancelledMembers, newMembersThisMonth, newMembersThisYear,
            retentionRate, byMembershipType, monthlyTrend
        );
    }

    public async Task<FinancialSummaryDto> GetFinancialSummaryAsync(Guid clubId)
    {
        var payments = await _context.Payments.IgnoreQueryFilters()
            .Where(p => p.ClubId == clubId && p.Status == PaymentStatus.Completed)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfYear = new DateTime(now.Year, 1, 1);

        var totalRevenue = payments.Sum(p => p.Amount);
        var totalRevenueThisMonth = payments.Where(p => p.PaymentDate >= startOfMonth).Sum(p => p.Amount);
        var totalRevenueThisYear = payments.Where(p => p.PaymentDate >= startOfYear).Sum(p => p.Amount);
        var outstandingPayments = await _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .SumAsync(m => m.AmountDue);
        var averagePaymentAmount = payments.Count > 0 ? payments.Average(p => p.Amount) : 0;

        var monthlyRevenue = Enumerable.Range(0, 12)
            .Select(i =>
            {
                var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
                var monthEnd = monthStart.AddMonths(1);
                var monthPayments = payments.Where(p => p.PaymentDate >= monthStart && p.PaymentDate < monthEnd);
                return new MonthlyRevenueDto(monthStart.ToString("MMM yyyy"), monthPayments.Sum(p => p.Amount), monthPayments.Count());
            })
            .Reverse()
            .ToList();

        var revenueByType = payments
            .GroupBy(p => p.Type.ToString())
            .Select(g => new RevenueByTypeDto(g.Key, g.Sum(p => p.Amount), g.Count(), totalRevenue > 0 ? g.Sum(p => p.Amount) / totalRevenue * 100 : 0))
            .ToList();

        var revenueByMethod = payments
            .GroupBy(p => p.Method.ToString())
            .Select(g => new RevenueByMethodDto(g.Key, g.Sum(p => p.Amount), g.Count(), totalRevenue > 0 ? g.Sum(p => p.Amount) / totalRevenue * 100 : 0))
            .ToList();

        return new FinancialSummaryDto(
            totalRevenue, totalRevenueThisMonth, totalRevenueThisYear,
            outstandingPayments, averagePaymentAmount,
            monthlyRevenue, revenueByType, revenueByMethod
        );
    }

    public async Task<AttendanceReportDto> GetAttendanceReportAsync(Guid clubId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var from = fromDate ?? DateTime.UtcNow.AddMonths(-3);
        var to = toDate ?? DateTime.UtcNow;

        var sessions = await _context.Sessions.IgnoreQueryFilters()
            .Where(s => s.ClubId == clubId && s.StartTime >= from && s.StartTime <= to)
            .Include(s => s.Bookings)
            .ToListAsync();

        var bookings = sessions.SelectMany(s => s.Bookings ?? []).ToList();

        var totalSessions = sessions.Count;
        var totalBookings = bookings.Count;
        var totalAttended = bookings.Count(b => b.Attended);
        var overallAttendanceRate = totalBookings > 0 ? (decimal)totalAttended / totalBookings * 100 : 0;

        var sessionStats = sessions
            .OrderByDescending(s => s.StartTime)
            .Take(20)
            .Select(s =>
            {
                var sBookings = s.Bookings?.ToList() ?? [];
                var attended = sBookings.Count(b => b.Attended);
                return new SessionAttendanceDto(
                    s.Id, s.Title, s.StartTime, s.Capacity, sBookings.Count, attended,
                    sBookings.Count > 0 ? (decimal)attended / sBookings.Count * 100 : 0
                );
            })
            .ToList();

        var topAttendees = bookings
            .GroupBy(b => new { b.MemberId, b.Member?.FullName })
            .Select(g => new MemberAttendanceDto(
                g.Key.MemberId,
                g.Key.FullName ?? "Unknown",
                g.Count(),
                g.Count(b => b.Attended),
                g.Count() > 0 ? (decimal)g.Count(b => b.Attended) / g.Count() * 100 : 0
            ))
            .OrderByDescending(m => m.TotalAttended)
            .Take(10)
            .ToList();

        var byCategory = sessions
            .GroupBy(s => s.Category.ToString())
            .Select(g =>
            {
                var catBookings = g.SelectMany(s => s.Bookings ?? []).ToList();
                var attended = catBookings.Count(b => b.Attended);
                return new CategoryAttendanceDto(
                    g.Key, g.Count(), catBookings.Count, attended,
                    catBookings.Count > 0 ? (decimal)attended / catBookings.Count * 100 : 0
                );
            })
            .ToList();

        return new AttendanceReportDto(
            totalSessions, totalBookings, totalAttended, overallAttendanceRate,
            sessionStats, topAttendees, byCategory
        );
    }

    public async Task<GrowthTrendDto> GetGrowthTrendsAsync(Guid clubId, int months = 12)
    {
        var now = DateTime.UtcNow;
        var members = await _context.Members.IgnoreQueryFilters().Where(m => m.ClubId == clubId).ToListAsync();
        var payments = await _context.Payments.IgnoreQueryFilters().Where(p => p.ClubId == clubId && p.Status == PaymentStatus.Completed).ToListAsync();
        var bookings = await _context.SessionBookings.IgnoreQueryFilters().Where(b => b.ClubId == clubId).ToListAsync();

        var memberGrowth = new List<GrowthDataPointDto>();
        var revenueGrowth = new List<GrowthDataPointDto>();
        var bookingGrowth = new List<GrowthDataPointDto>();

        decimal? prevMembers = null, prevRevenue = null, prevBookings = null;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var memberCount = (decimal)members.Count(m => m.JoinedDate < monthEnd);
            var revenueAmount = payments.Where(p => p.PaymentDate >= monthStart && p.PaymentDate < monthEnd).Sum(p => p.Amount);
            var bookingCount = (decimal)bookings.Count(b => b.BookedAt >= monthStart && b.BookedAt < monthEnd);

            memberGrowth.Add(CreateGrowthPoint(monthStart.ToString("MMM yyyy"), memberCount, prevMembers));
            revenueGrowth.Add(CreateGrowthPoint(monthStart.ToString("MMM yyyy"), revenueAmount, prevRevenue));
            bookingGrowth.Add(CreateGrowthPoint(monthStart.ToString("MMM yyyy"), bookingCount, prevBookings));

            prevMembers = memberCount;
            prevRevenue = revenueAmount;
            prevBookings = bookingCount;
        }

        return new GrowthTrendDto(
            memberGrowth, revenueGrowth, bookingGrowth,
            CalculateOverallGrowthRate(memberGrowth),
            CalculateOverallGrowthRate(revenueGrowth),
            CalculateOverallGrowthRate(bookingGrowth)
        );
    }

    public async Task<RetentionAnalysisDto> GetRetentionAnalysisAsync(Guid clubId)
    {
        var memberships = await _context.Memberships.IgnoreQueryFilters()
            .Where(m => m.ClubId == clubId)
            .Include(m => m.MembershipType)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var expired = memberships.Where(m => m.EndDate < now).ToList();
        var renewed = expired.Count(m => memberships.Any(m2 => m2.MemberId == m.MemberId && m2.StartDate > m.EndDate));
        var churned = expired.Count - renewed;

        var overallRetentionRate = expired.Count > 0 ? (decimal)renewed / expired.Count * 100 : 100;
        var churnRate = expired.Count > 0 ? (decimal)churned / expired.Count * 100 : 0;

        var byMembershipType = memberships
            .GroupBy(m => m.MembershipType?.Name ?? "Unknown")
            .Select(g =>
            {
                var expiredInGroup = g.Where(m => m.EndDate < now).ToList();
                var renewedInGroup = expiredInGroup.Count(m => memberships.Any(m2 => m2.MemberId == m.MemberId && m2.StartDate > m.EndDate));
                return new RetentionByTypeDto(
                    g.Key, g.Count(), renewedInGroup, expiredInGroup.Count - renewedInGroup,
                    expiredInGroup.Count > 0 ? (decimal)renewedInGroup / expiredInGroup.Count * 100 : 100
                );
            })
            .ToList();

        var monthlyTrend = Enumerable.Range(0, 12)
            .Select(i =>
            {
                var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
                var monthEnd = monthStart.AddMonths(1);
                var expiringInMonth = memberships.Where(m => m.EndDate >= monthStart && m.EndDate < monthEnd).ToList();
                var renewedInMonth = expiringInMonth.Count(m => memberships.Any(m2 => m2.MemberId == m.MemberId && m2.StartDate >= m.EndDate));
                return new MonthlyRetentionDto(
                    monthStart.ToString("MMM yyyy"),
                    expiringInMonth.Count,
                    renewedInMonth,
                    expiringInMonth.Count - renewedInMonth,
                    expiringInMonth.Count > 0 ? (decimal)renewedInMonth / expiringInMonth.Count * 100 : 100
                );
            })
            .Reverse()
            .ToList();

        return new RetentionAnalysisDto(
            overallRetentionRate, churnRate, renewed, churned,
            byMembershipType, monthlyTrend
        );
    }

    private static GrowthDataPointDto CreateGrowthPoint(string period, decimal value, decimal? previous)
    {
        var change = previous.HasValue ? value - previous.Value : 0;
        var percentageChange = previous.HasValue && previous.Value > 0 ? change / previous.Value * 100 : 0;
        return new GrowthDataPointDto(period, value, change, percentageChange);
    }

    private static decimal CalculateOverallGrowthRate(List<GrowthDataPointDto> points)
    {
        if (points.Count < 2) return 0;
        var first = points.First().Value;
        var last = points.Last().Value;
        return first > 0 ? (last - first) / first * 100 : 0;
    }
}
