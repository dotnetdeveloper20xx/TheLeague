namespace TheLeague.Api.DTOs;

public record MembershipStatsDto(
    int TotalMembers,
    int ActiveMembers,
    int PendingMembers,
    int ExpiredMembers,
    int SuspendedMembers,
    int CancelledMembers,
    int NewMembersThisMonth,
    int NewMembersThisYear,
    double RetentionRate,
    IEnumerable<MembershipTypeStatsDto> ByMembershipType,
    IEnumerable<MonthlyMemberStatsDto> MonthlyTrend
);

public record MembershipTypeStatsDto(
    Guid MembershipTypeId,
    string MembershipTypeName,
    int ActiveCount,
    int TotalCount,
    decimal Revenue
);

public record MonthlyMemberStatsDto(
    string Month,
    int NewMembers,
    int ChurnedMembers,
    int TotalActive
);

public record FinancialSummaryDto(
    decimal TotalRevenue,
    decimal TotalRevenueThisMonth,
    decimal TotalRevenueThisYear,
    decimal OutstandingPayments,
    decimal AveragePaymentAmount,
    IEnumerable<MonthlyRevenueDto> MonthlyRevenue,
    IEnumerable<RevenueByTypeDto> RevenueByType,
    IEnumerable<RevenueByMethodDto> RevenueByMethod
);

public record MonthlyRevenueDto(
    string Month,
    decimal Amount,
    int PaymentCount
);

public record RevenueByTypeDto(
    string Type,
    decimal Amount,
    int Count,
    decimal Percentage
);

public record RevenueByMethodDto(
    string Method,
    decimal Amount,
    int Count,
    decimal Percentage
);

public record AttendanceReportDto(
    int TotalSessions,
    int TotalBookings,
    int TotalAttended,
    decimal OverallAttendanceRate,
    IEnumerable<SessionAttendanceDto> SessionStats,
    IEnumerable<MemberAttendanceDto> TopAttendees,
    IEnumerable<CategoryAttendanceDto> ByCategory
);

public record SessionAttendanceDto(
    Guid SessionId,
    string SessionTitle,
    DateTime Date,
    int Capacity,
    int Booked,
    int Attended,
    decimal AttendanceRate
);

public record MemberAttendanceDto(
    Guid MemberId,
    string MemberName,
    int TotalBookings,
    int TotalAttended,
    decimal AttendanceRate
);

public record CategoryAttendanceDto(
    string Category,
    int TotalSessions,
    int TotalBookings,
    int TotalAttended,
    decimal AttendanceRate
);

public record GrowthTrendDto(
    IEnumerable<GrowthDataPointDto> MemberGrowth,
    IEnumerable<GrowthDataPointDto> RevenueGrowth,
    IEnumerable<GrowthDataPointDto> BookingGrowth,
    decimal MemberGrowthRate,
    decimal RevenueGrowthRate,
    decimal BookingGrowthRate
);

public record GrowthDataPointDto(
    string Period,
    decimal Value,
    decimal ChangeFromPrevious,
    decimal PercentageChange
);

public record RetentionAnalysisDto(
    decimal OverallRetentionRate,
    decimal ChurnRate,
    int TotalRenewals,
    int TotalChurns,
    IEnumerable<RetentionByTypeDto> ByMembershipType,
    IEnumerable<MonthlyRetentionDto> MonthlyTrend
);

public record RetentionByTypeDto(
    string MembershipType,
    int TotalMembers,
    int Renewed,
    int Churned,
    decimal RetentionRate
);

public record MonthlyRetentionDto(
    string Month,
    int TotalExpiring,
    int Renewed,
    int Churned,
    decimal RetentionRate
);
