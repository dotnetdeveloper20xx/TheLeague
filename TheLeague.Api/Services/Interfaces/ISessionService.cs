using TheLeague.Api.DTOs;

namespace TheLeague.Api.Services.Interfaces;

public interface ISessionService
{
    // Sessions
    Task<PagedResult<SessionListDto>> GetSessionsAsync(Guid clubId, SessionFilterRequest filter);
    Task<SessionDto?> GetSessionByIdAsync(Guid clubId, Guid id);
    Task<SessionDto> CreateSessionAsync(Guid clubId, SessionCreateRequest request);
    Task<SessionDto?> UpdateSessionAsync(Guid clubId, Guid id, SessionUpdateRequest request);
    Task<bool> CancelSessionAsync(Guid clubId, Guid id, string? reason);

    // Bookings
    Task<IEnumerable<SessionBookingDto>> GetSessionBookingsAsync(Guid clubId, Guid sessionId);
    Task<IEnumerable<MemberBookingDto>> GetMemberBookingsAsync(Guid clubId, Guid memberId);
    Task<SessionBookingDto> BookSessionAsync(Guid clubId, Guid sessionId, Guid memberId, BookSessionRequest request);
    Task<bool> CancelBookingAsync(Guid clubId, Guid sessionId, Guid memberId, Guid? familyMemberId);
    Task<bool> UpdateAttendanceAsync(Guid clubId, Guid sessionId, BulkAttendanceRequest request);

    // Waitlist
    Task<IEnumerable<WaitlistDto>> GetSessionWaitlistAsync(Guid clubId, Guid sessionId);
    Task<WaitlistDto> JoinWaitlistAsync(Guid clubId, Guid sessionId, Guid memberId, Guid? familyMemberId);
    Task<bool> LeaveWaitlistAsync(Guid clubId, Guid sessionId, Guid memberId, Guid? familyMemberId);

    // Recurring Schedules
    Task<IEnumerable<RecurringScheduleDto>> GetRecurringSchedulesAsync(Guid clubId);
    Task<RecurringScheduleDto?> GetRecurringScheduleByIdAsync(Guid clubId, Guid id);
    Task<RecurringScheduleDto> CreateRecurringScheduleAsync(Guid clubId, RecurringScheduleCreateRequest request);
    Task<RecurringScheduleDto?> UpdateRecurringScheduleAsync(Guid clubId, Guid id, RecurringScheduleUpdateRequest request);
    Task<bool> DeleteRecurringScheduleAsync(Guid clubId, Guid id);
    Task<int> GenerateSessionsAsync(Guid clubId, Guid scheduleId, GenerateSessionsRequest request);

    // Recurring Bookings
    Task<IEnumerable<RecurringBookingDto>> GetMemberRecurringBookingsAsync(Guid clubId, Guid memberId);
    Task<RecurringBookingDto> CreateRecurringBookingAsync(Guid clubId, Guid memberId, RecurringBookingCreateRequest request);
    Task<bool> CancelRecurringBookingAsync(Guid clubId, Guid id);
}
