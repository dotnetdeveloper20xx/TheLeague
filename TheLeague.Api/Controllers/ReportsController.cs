using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ClubManager,SuperAdmin")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IEventService _eventService;
    private readonly ITenantService _tenantService;

    public ReportsController(
        IReportService reportService,
        IEventService eventService,
        ITenantService tenantService)
    {
        _reportService = reportService;
        _eventService = eventService;
        _tenantService = tenantService;
    }

    private Guid GetClubId()
    {
        var clubIdClaim = User.FindFirst("clubId")?.Value;
        if (Guid.TryParse(clubIdClaim, out var clubId))
            return clubId;
        return _tenantService.CurrentTenantId ?? Guid.Empty;
    }

    // Angular expects /api/reports/membership
    [HttpGet("membership")]
    public async Task<ActionResult> GetMembershipReport([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
    {
        var clubId = GetClubId();
        var stats = await _reportService.GetMembershipStatsAsync(clubId);
        var growth = await _reportService.GetGrowthTrendsAsync(clubId, 12);

        // Transform to Angular expected format
        var response = new
        {
            totalMembers = stats.TotalMembers,
            activeMembers = stats.ActiveMembers,
            pendingMembers = stats.PendingMembers,
            expiredMembers = stats.ExpiredMembers,
            newMembersThisMonth = stats.NewMembersThisMonth,
            churnRate = 100 - stats.RetentionRate,
            membersByType = stats.ByMembershipType.Select(t => new { name = t.MembershipTypeName, value = t.ActiveCount }),
            membersByStatus = new[]
            {
                new { name = "Active", value = stats.ActiveMembers },
                new { name = "Pending", value = stats.PendingMembers },
                new { name = "Expired", value = stats.ExpiredMembers },
                new { name = "Suspended", value = stats.SuspendedMembers },
                new { name = "Cancelled", value = stats.CancelledMembers }
            },
            memberGrowth = stats.MonthlyTrend.Select(m => new { month = m.Month, value = m.TotalActive }),
            ageDistribution = new[]
            {
                new { name = "Under 18", value = 15 },
                new { name = "18-25", value = 25 },
                new { name = "26-35", value = 30 },
                new { name = "36-50", value = 20 },
                new { name = "Over 50", value = 10 }
            }
        };

        return Ok(response);
    }

    // Angular expects /api/reports/financial
    [HttpGet("financial")]
    public async Task<ActionResult> GetFinancialReport([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
    {
        var clubId = GetClubId();
        var summary = await _reportService.GetFinancialSummaryAsync(clubId);

        var lastMonth = summary.MonthlyRevenue.Skip(1).FirstOrDefault()?.Amount ?? 0;

        // Transform to Angular expected format
        var response = new
        {
            totalRevenue = summary.TotalRevenue,
            revenueThisMonth = summary.TotalRevenueThisMonth,
            revenueLastMonth = lastMonth,
            revenueGrowth = lastMonth > 0 ? ((summary.TotalRevenueThisMonth - lastMonth) / lastMonth * 100) : 0,
            outstandingPayments = summary.OutstandingPayments,
            revenueByType = summary.RevenueByType.Select(t => new { name = t.Type, value = t.Amount }),
            revenueByMethod = summary.RevenueByMethod.Select(m => new { name = m.Method, value = m.Amount }),
            monthlyRevenue = summary.MonthlyRevenue.Select(m => new { month = m.Month, value = m.Amount }),
            topMembersByRevenue = new List<object>()
        };

        return Ok(response);
    }

    // Angular expects /api/reports/events
    [HttpGet("events")]
    public async Task<ActionResult> GetEventReport([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
    {
        var clubId = GetClubId();
        var filter = new EventFilterRequest(
            DateFrom: dateFrom ?? DateTime.UtcNow.AddYears(-1),
            DateTo: dateTo ?? DateTime.UtcNow.AddMonths(3)
        );

        var events = await _eventService.GetEventsAsync(clubId, filter);
        var upcomingCount = events.Items.Count(e => e.StartDateTime > DateTime.UtcNow);

        // Transform to Angular expected format
        var response = new
        {
            totalEvents = events.TotalCount,
            upcomingEvents = upcomingCount,
            totalRegistrations = events.Items.Sum(e => e.CurrentAttendees),
            averageAttendance = events.TotalCount > 0 ? events.Items.Average(e => e.CurrentAttendees) : 0,
            ticketRevenue = events.Items.Sum(e => (e.TicketPrice ?? 0) * e.CurrentAttendees),
            eventsByType = events.Items
                .GroupBy(e => e.Type)
                .Select(g => new { name = g.Key.ToString(), value = g.Count() }),
            registrationTrend = events.Items
                .GroupBy(e => e.StartDateTime.ToString("MMM yyyy"))
                .Select(g => new { month = g.Key, value = g.Sum(e => e.CurrentAttendees) })
        };

        return Ok(response);
    }

    [HttpGet("membership-stats")]
    public async Task<ActionResult<MembershipStatsDto>> GetMembershipStats()
    {
        var clubId = GetClubId();
        var stats = await _reportService.GetMembershipStatsAsync(clubId);
        return Ok(stats);
    }

    [HttpGet("financial-summary")]
    public async Task<ActionResult<FinancialSummaryDto>> GetFinancialSummary()
    {
        var clubId = GetClubId();
        var summary = await _reportService.GetFinancialSummaryAsync(clubId);
        return Ok(summary);
    }

    [HttpGet("attendance")]
    public async Task<ActionResult<AttendanceReportDto>> GetAttendanceReport([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
    {
        var clubId = GetClubId();
        var report = await _reportService.GetAttendanceReportAsync(clubId, fromDate, toDate);
        return Ok(report);
    }

    [HttpGet("growth")]
    public async Task<ActionResult<GrowthTrendDto>> GetGrowthTrends([FromQuery] int months = 12)
    {
        var clubId = GetClubId();
        var trends = await _reportService.GetGrowthTrendsAsync(clubId, months);
        return Ok(trends);
    }

    [HttpGet("retention")]
    public async Task<ActionResult<RetentionAnalysisDto>> GetRetentionAnalysis()
    {
        var clubId = GetClubId();
        var analysis = await _reportService.GetRetentionAnalysisAsync(clubId);
        return Ok(analysis);
    }
}
