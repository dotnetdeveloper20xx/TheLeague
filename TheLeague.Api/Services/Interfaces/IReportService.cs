using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface IReportService
{
    Task<MembershipStatsDto> GetMembershipStatsAsync(Guid clubId);
    Task<FinancialSummaryDto> GetFinancialSummaryAsync(Guid clubId);
    Task<AttendanceReportDto> GetAttendanceReportAsync(Guid clubId, DateTime? fromDate = null, DateTime? toDate = null);
    Task<GrowthTrendDto> GetGrowthTrendsAsync(Guid clubId, int months = 12);
    Task<RetentionAnalysisDto> GetRetentionAnalysisAsync(Guid clubId);
}
