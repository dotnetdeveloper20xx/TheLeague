using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record ClubDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string? LogoUrl,
    string PrimaryColor,
    string SecondaryColor,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website,
    ClubType ClubType,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? RenewalDate,
    int MemberCount,
    ClubSettingsDto? Settings
);

public record ClubCreateRequest(
    [Required] string Name,
    [Required] string Slug,
    string? Description,
    string? LogoUrl,
    string PrimaryColor,
    string SecondaryColor,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website,
    ClubType ClubType
);

public record ClubUpdateRequest(
    string? Name,
    string? Description,
    string? LogoUrl,
    string? PrimaryColor,
    string? SecondaryColor,
    string? ContactEmail,
    string? ContactPhone,
    string? Address,
    string? Website,
    ClubType? ClubType,
    bool? IsActive
);

public record ClubSettingsDto(
    Guid Id,
    bool AllowOnlineRegistration,
    bool RequireEmergencyContact,
    bool RequireMedicalInfo,
    bool AllowFamilyAccounts,
    bool AllowOnlinePayments,
    bool AllowManualPayments,
    bool AutoSendPaymentReminders,
    int PaymentReminderDaysBefore,
    int PaymentReminderFrequency,
    bool AllowMemberBookings,
    int MaxAdvanceBookingDays,
    int CancellationNoticePeriodHours,
    bool EnableWaitlist,
    bool SendWelcomeEmail,
    bool SendBookingConfirmations,
    bool SendPaymentReceipts
);

public record ClubSettingsUpdateRequest(
    bool? AllowOnlineRegistration,
    bool? RequireEmergencyContact,
    bool? RequireMedicalInfo,
    bool? AllowFamilyAccounts,
    bool? AllowOnlinePayments,
    bool? AllowManualPayments,
    bool? AutoSendPaymentReminders,
    int? PaymentReminderDaysBefore,
    int? PaymentReminderFrequency,
    bool? AllowMemberBookings,
    int? MaxAdvanceBookingDays,
    int? CancellationNoticePeriodHours,
    bool? EnableWaitlist,
    bool? SendWelcomeEmail,
    bool? SendBookingConfirmations,
    bool? SendPaymentReceipts
);

public record ClubDashboardDto(
    int TotalMembers,
    int ActiveMembers,
    int NewMembersThisMonth,
    int ExpiredMemberships,
    decimal TotalRevenueThisMonth,
    decimal TotalRevenueThisYear,
    decimal OutstandingPayments,
    int UpcomingSessions,
    int UpcomingEvents,
    decimal AttendanceRate,
    IEnumerable<MembershipBreakdownDto> MembershipBreakdown,
    IEnumerable<RevenueByMonthDto> RevenueByMonth,
    IEnumerable<RecentMemberDto> RecentMembers,
    IEnumerable<UpcomingSessionDto> UpcomingSessionsList
);

public record MembershipBreakdownDto(
    string MembershipType,
    int Count,
    decimal Percentage
);

public record RevenueByMonthDto(
    string Month,
    decimal Amount
);

public record RecentMemberDto(
    Guid Id,
    string FullName,
    string Email,
    DateTime JoinedDate,
    string MembershipType
);

public record UpcomingSessionDto(
    Guid Id,
    string Title,
    DateTime StartTime,
    int BookedCount,
    int Capacity
);
