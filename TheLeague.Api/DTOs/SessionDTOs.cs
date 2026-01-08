using System.ComponentModel.DataAnnotations;
using TheLeague.Core.Enums;

namespace TheLeague.Api.DTOs;

public record SessionDto(
    Guid Id,
    string Title,
    string? Description,
    SessionCategory Category,
    DateTime StartTime,
    DateTime EndTime,
    int Capacity,
    int CurrentBookings,
    int AvailableSpots,
    bool IsRecurring,
    Guid? RecurringScheduleId,
    decimal? SessionFee,
    bool IsCancelled,
    string? CancellationReason,
    VenueDto? Venue,
    IEnumerable<SessionBookingDto>? Bookings
);

public record SessionListDto(
    Guid Id,
    string Title,
    SessionCategory Category,
    DateTime StartTime,
    DateTime EndTime,
    int Capacity,
    int CurrentBookings,
    int AvailableSpots,
    decimal? SessionFee,
    bool IsCancelled,
    string? VenueName
);

public record SessionCreateRequest(
    [Required] string Title,
    string? Description,
    SessionCategory Category,
    [Required] DateTime StartTime,
    [Required] DateTime EndTime,
    [Required] int Capacity,
    Guid? VenueId,
    decimal? SessionFee
);

public record SessionUpdateRequest(
    string? Title,
    string? Description,
    SessionCategory? Category,
    DateTime? StartTime,
    DateTime? EndTime,
    int? Capacity,
    Guid? VenueId,
    decimal? SessionFee,
    bool? IsCancelled,
    string? CancellationReason
);

public record SessionBookingDto(
    Guid Id,
    Guid MemberId,
    string MemberName,
    Guid? FamilyMemberId,
    string? FamilyMemberName,
    DateTime BookedAt,
    BookingStatus Status,
    bool Attended,
    DateTime? CheckedInAt,
    string? Notes
);

public record BookSessionRequest(
    Guid? FamilyMemberId,
    string? Notes
);

public record AttendanceUpdateRequest(
    [Required] Guid BookingId,
    bool Attended
);

public record BulkAttendanceRequest(
    IEnumerable<AttendanceUpdateRequest> Attendances
);

// Recurring Schedule
public record RecurringScheduleDto(
    Guid Id,
    string Title,
    string? Description,
    SessionCategory Category,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int Capacity,
    DateTime ScheduleStartDate,
    DateTime? ScheduleEndDate,
    bool IsActive,
    decimal? SessionFee,
    VenueDto? Venue,
    int GeneratedSessionCount
);

public record RecurringScheduleCreateRequest(
    [Required] string Title,
    string? Description,
    SessionCategory Category,
    [Required] DayOfWeek DayOfWeek,
    [Required] TimeSpan StartTime,
    [Required] TimeSpan EndTime,
    [Required] int Capacity,
    Guid? VenueId,
    [Required] DateTime ScheduleStartDate,
    DateTime? ScheduleEndDate,
    decimal? SessionFee
);

public record RecurringScheduleUpdateRequest(
    string? Title,
    string? Description,
    SessionCategory? Category,
    TimeSpan? StartTime,
    TimeSpan? EndTime,
    int? Capacity,
    Guid? VenueId,
    DateTime? ScheduleEndDate,
    bool? IsActive,
    decimal? SessionFee
);

public record GenerateSessionsRequest(
    [Required] DateTime FromDate,
    [Required] DateTime ToDate
);

// Recurring Booking
public record RecurringBookingDto(
    Guid Id,
    Guid MemberId,
    string MemberName,
    Guid? FamilyMemberId,
    string? FamilyMemberName,
    Guid RecurringScheduleId,
    string RecurringScheduleTitle,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsActive,
    string? Notes,
    DateTime CreatedAt
);

public record RecurringBookingCreateRequest(
    [Required] Guid RecurringScheduleId,
    Guid? FamilyMemberId,
    [Required] DateTime StartDate,
    DateTime? EndDate,
    string? Notes
);

// Waitlist
public record WaitlistDto(
    Guid Id,
    Guid SessionId,
    Guid MemberId,
    string MemberName,
    Guid? FamilyMemberId,
    string? FamilyMemberName,
    int Position,
    DateTime JoinedAt,
    bool NotificationSent,
    DateTime? NotificationSentAt
);

public record SessionFilterRequest(
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    SessionCategory? Category = null,
    Guid? VenueId = null,
    bool? IncludeCancelled = null,
    int Page = 1,
    int PageSize = 20
);
